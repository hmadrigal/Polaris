namespace Polaris
{
    using System.ComponentModel;
    using Microsoft.Practices.Prism.Events;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// An abstract class that serves as the base for all view models.
    /// </summary>
    /// <remarks>
    /// This class implements <see cref="INotifyPropertyChanged"/>, making
    /// the data binding possible down the tree.
    /// In addition to this, view models are optionally allowed to notify their
    /// children about the parent view model they live in, enabling them to
    /// have easy access to the properties of the parent in case of nested
    /// view models. If you want the children in a collection to be aware of
    /// their parent, make sure to implement an
    /// <see cref="ObservableCollection&lt;T&gt;"/> and attach the
    /// <see cref="ChildCollectionChangedHandler"/> event handler to the
    /// <see cref="ObaservableCollection&lt;T&gt;.CollectionChanged"/> event.
    /// </remarks>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the unity container for this view model.
        /// </summary>
        protected IUnityContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the event aggregator for this view model.
        /// </summary>
        protected IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Gets or sets the parent ViewModel for this instance.
        /// </summary>
        public ViewModelBase Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        /// <summary>
        /// The parent view model for this instance.
        /// </summary>
        private ViewModelBase parent;

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members

        /// <summary>
        /// Event handler that notifies the new children of the parent
        /// view model they live in and removes the parent when they are
        /// removed from the collection.
        /// </summary>
        /// <param name="sender">Child view model rising the event.</param>
        /// <param name="e">Changes made to the collection.</param>
        protected void ChildCollectionChangedHandler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((ViewModelBase)item).Parent = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((ViewModelBase)item).Parent = null;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// by injecting a dependency on the Unity container.
        /// </summary>
        /// <param name="container">
        /// The unity container for this application.
        /// </param>
        public ViewModelBase(IUnityContainer container)
        {
            Container = container;
            ResolveDependencies();
            Load();
        }

        /// <summary>
        /// Loads the view model.
        /// </summary>
        private void Load()
        {
            BeforeLoad();
            OnLoad();
        }

        /// <summary>
        /// Resolves the dependencies for this view model.
        /// </summary>
        private void ResolveDependencies()
        {
            BeforeResolveDependencies();
            EventAggregator = Container.Resolve<IEventAggregator>();
            OnResolveDependencies();
        }

        /// <summary>
        /// Gives inheritors the opportunity to perform an operation before the dependencies are resolved.
        /// </summary>
        protected virtual void BeforeResolveDependencies()
        {
        }

        /// <summary>
        /// Gives inheritors the opportunity to resolve dependencies of their own.
        /// </summary>
        protected virtual void OnResolveDependencies()
        {
        }

        /// <summary>
        /// Gives inheritors the opportunity to perform an operation before the view model loads.
        /// </summary>
        protected virtual void BeforeLoad()
        {
        }

        /// <summary>
        /// Gives inheritors the opportunity to perform specific operations when the view model loads for the first time.
        /// </summary>
        protected virtual void OnLoad()
        {
        }

        /// <summary>
        /// Exposes a way for external users to pass a boxed payload
        /// to this view model for initialization purposes.
        /// Inheritors can override this method to perform specific
        /// initialization and validation operations depending on the
        /// payload sent from the outside.
        /// </summary>
        /// <param name="payload">
        /// Represents the boxed payload the view model uses
        /// for initialization purposes.
        /// </param>
        public virtual void Initialize(object payload)
        {
        }
    }
}