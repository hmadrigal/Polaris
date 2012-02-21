//-----------------------------------------------------------------------
// <copyright file="AnimatedLayoutItemsControl.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Controls
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;

    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(AnimatedLayoutControl))]
    public class AnimatedLayoutItemsControl : ItemsControl
    {

        private AnimatedLayoutControl LastAnimatedItem { get; set; }
        private ObservableCollection<Object> NewRequestedItems { get; set; }

        private Boolean isUnloading = false;

        private ObservableCollection<Object> BaseItemsSource;

        private ObservableCollection<Object> ItemsToRemove;

        private Random RandomGenerator = new Random();

        #region ItemsSource

        /// <summary>
        /// ItemsSource Dependency Property
        /// </summary>
        public static new readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AnimatedLayoutItemsControl),
                new PropertyMetadata(null,
                    new PropertyChangedCallback(OnItemsSourceChanged)));

        /// <summary>
        /// Gets or sets a collection used to generate the content of the System.Windows.Controls.ItemsControl.
        /// </summary>
        /// <returns>
        //     The object that is used to generate the content of 
        //     the System.Windows.Controls.ItemsControl.
        //     The default is null.
        /// </returns>
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemsSource property.
        /// </summary>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedLayoutItemsControl)d).OnItemsSourceChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemsSource property.
        /// </summary>
        protected virtual void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldItems = e.OldValue as IEnumerable;
            var newItems = e.NewValue as IEnumerable;

            if (newItems != null)
            {
                if (newItems is INotifyCollectionChanged)
                {
                    var newObservableItems = newItems as INotifyCollectionChanged;
                    newObservableItems.CollectionChanged += new NotifyCollectionChangedEventHandler(ObservableItems_CollectionChanged);

                    var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                        newItems.Cast<object>().ToList());

                    ObservableItems_CollectionChanged(this, eventArgs);
                }
            }

            if (oldItems != null)
            {
                if (oldItems is INotifyCollectionChanged)
                {
                    var oldObservableItems = oldItems as INotifyCollectionChanged;
                    oldObservableItems.CollectionChanged -= new NotifyCollectionChangedEventHandler(ObservableItems_CollectionChanged);
                }
                foreach (var item in oldItems)
                {
                    var visualItem = (from oldItem in VisualItems
                                      where oldItem.DataContext == item
                                      select oldItem).FirstOrDefault();

                    visualItem.LayoutStateChangeCompleted += new EventHandler<LayoutStateChangeEventArgs>(visualItem_LayoutStateChangeCompleted);

                    visualItem.LayoutState = LayoutState.Unloaded;

                    VisualItems.Remove(visualItem);
                }
            }

        }

        #endregion

        private readonly ObservableCollection<AnimatedLayoutControl> VisualItems;

        void visualItem_ApplyTemplateCompleted(object sender, EventArgs e)
        {
            var visualItem = sender as AnimatedLayoutControl;
            visualItem.ApplyTemplateCompleted -= new EventHandler(visualItem_ApplyTemplateCompleted);
            VisualItems.Add(visualItem);
        }

        public AnimatedLayoutItemsControl()
            : base()
        {
            DefaultStyleKey = typeof(AnimatedLayoutItemsControl);
            BaseItemsSource = new ObservableCollection<Object>();
            ItemsToRemove = new ObservableCollection<Object>();
            NewRequestedItems = new ObservableCollection<Object>();
            base.ItemsSource = BaseItemsSource;
            VisualItems = new ObservableCollection<AnimatedLayoutControl>();
            VisualItems.CollectionChanged += new NotifyCollectionChangedEventHandler(VisualItems_CollectionChanged);
            ItemsToRemove.CollectionChanged += new NotifyCollectionChangedEventHandler(ItemsToRemove_CollectionChanged);
        }

        void VisualItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (e.NewItems != null)
            //{
            //    var newVisualItems = from AnimatedLayoutControl item in e.NewItems
            //                         select item as AnimatedLayoutControl;
            //    foreach (var visualItem in newVisualItems)
            //    {
            //        if (LastAnimatedItem != null)
            //        {
            //            if (visualItem.BeginTime > LastAnimatedItem.BeginTime)
            //            {
            //                SetNewLastAnimatedItem(visualItem);
            //            }
            //        }
            //        else
            //        {
            //            SetNewLastAnimatedItem(visualItem);

            //        }
            //        //visualItem.LoadingState = LayoutState.Loaded;
            //    }
            //}

            //if (e.OldItems != null)
            //{
            //    var oldVisualItem = (from AnimatedLayoutControl item in e.OldItems
            //                         orderby item.BeginTime descending
            //                         select item as AnimatedLayoutControl).FirstOrDefault();

            //    if (oldVisualItem == LastAnimatedItem)
            //    {
            //        var newLastItem = (from visualItem in VisualItems
            //                           where visualItem != LastAnimatedItem
            //                           orderby visualItem.BeginTime descending
            //                           select visualItem).FirstOrDefault();
            //        SetNewLastAnimatedItem(newLastItem);
            //    }
            //}

            if (VisualItems.Count == Items.Count)
            {
                foreach (var item in VisualItems)
                {
                    item.Initialize();
                    item.LayoutState = LayoutState.Loaded;
                }
            }
        }


        void visualItem_LayoutStateChangeCompleted(object sender, LayoutStateChangeEventArgs e)
        {
            var visualItem = sender as AnimatedLayoutControl;
            if (e.NewState == LayoutState.Unloaded)
            {
                visualItem.LayoutStateChangeCompleted -= new EventHandler<LayoutStateChangeEventArgs>(visualItem_LayoutStateChangeCompleted);
                ItemsToRemove.Add(visualItem.DataContext);
            }
        }


        void ItemsToRemove_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ItemsToRemove.Count == Items.Count)
            {
                foreach (var item in ItemsToRemove)
                {
                    BaseItemsSource.Remove(item);
                }
            }
        }


        void ObservableItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (isUnloading)
                        {
                            NewRequestedItems.Add(item);
                        }
                        else
                        {
                            BaseItemsSource.Add(item);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (!IsVisible)
                    {
                        BaseItemsSource.Clear();
                        VisualItems.Clear();
                        isUnloading = false;
                    }
                    else if (VisualItems.Count > 0)
                    {
                        isUnloading = true;
                        AnimatedLayoutControl lastItem = null;
                        foreach (var item in VisualItems)
                        {
                            if ((lastItem == null) || (item.BeginTime > lastItem.BeginTime))
                            {
                                lastItem = item;
                            }
                            item.LayoutState = LayoutState.Unloaded;
                        }

                        lastItem.LayoutStateChangeCompleted += new EventHandler<LayoutStateChangeEventArgs>(item_LayoutStateChangeCompleted);
                    }
                    break;
            }
        }

        void item_LayoutStateChangeCompleted(object sender, LayoutStateChangeEventArgs e)
        {
            if (e.NewState == LayoutState.Unloaded)
            {
                var item = sender as AnimatedLayoutControl;
                item.LayoutStateChangeCompleted -= new EventHandler<LayoutStateChangeEventArgs>(item_LayoutStateChangeCompleted);

                BaseItemsSource.Clear();
                VisualItems.Clear();

                isUnloading = false;
                var itemsToAdd = NewRequestedItems.ToArray();
                NewRequestedItems.Clear();
                foreach (var newItem in itemsToAdd)
                {
                    BaseItemsSource.Add(newItem);
                }
            }
        }



        protected override DependencyObject GetContainerForItemOverride()
        {
            var visualItem = new AnimatedLayoutControl();
            visualItem.RandomGenerator = RandomGenerator;
            visualItem.ApplyTemplateCompleted += new EventHandler(visualItem_ApplyTemplateCompleted);
            return visualItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var presenter = ((FrameworkElement)element);
            if (ItemContainerStyle != null)
            {
                presenter.Style = ItemContainerStyle;
            }
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is AnimatedLayoutControl);
        }



    }
}
