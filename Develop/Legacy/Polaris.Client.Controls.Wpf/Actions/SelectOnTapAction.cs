//-----------------------------------------------------------------------
// <copyright file="SelectOnTapAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using Polaris.Windows.Extensions;

    [TypeConstraint(typeof(FrameworkElement))]
    public class SelectOnTapAction : TargetedTriggerAction<UIElement>
    {
        private Point initialPosition;
        private DateTime startTime;

        private const int xThreshold = 3;
        private const int yThreshold = 3;
        private readonly TimeSpan timeThreshold = new TimeSpan(0, 0, 0, 0, 300);

        private FrameworkElement AssociatedItemsControl
        {
            get
            {
                if (associatedItemsControl == null)
                {
                    associatedItemsControl = AssociatedObject as FrameworkElement;
                }
                return associatedItemsControl;
            }
        }

        private FrameworkElement associatedItemsControl;

        #region Command

        /// <summary>
        /// Command Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SelectOnTapAction),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Command property.  This dependency property
        /// indicates the command that will be executed on tap.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion Command

        public Panel AssociatedPanel
        {
            get
            {
                if (associatedPanel == null)
                {
                    associatedPanel = AssociatedItemsControl.FindPanel();
                }
                return associatedPanel;
            }
        }

        private Panel associatedPanel;

        public string ButtonName { get; set; }

        protected override void Invoke(object parameter)
        {
        }

        protected override void OnTargetChanged(UIElement oldTarget, UIElement newTarget)
        {
            if (oldTarget != null)
            {
                oldTarget.TouchUp -= new EventHandler<TouchEventArgs>(Target_TouchUp);
                oldTarget.TouchDown -= new EventHandler<TouchEventArgs>(Target_TouchDown);
            }
            if (newTarget != null)
            {
                newTarget.TouchUp += new EventHandler<TouchEventArgs>(Target_TouchUp);
                newTarget.TouchDown += new EventHandler<TouchEventArgs>(Target_TouchDown);
#if DEBUG
                newTarget.MouseDown += new MouseButtonEventHandler(newTarget_MouseDown);
                newTarget.MouseUp += new MouseButtonEventHandler(newTarget_MouseUp);
#endif
            }
            base.OnTargetChanged(oldTarget, newTarget);
        }

        private void newTarget_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var targetPosition = e.GetPosition(GetMainContainerElement());
            HandleTouchUp(targetPosition);
        }

        private void newTarget_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var targetPosition = e.GetPosition(GetMainContainerElement());
            targetPosition = HandleTouchDown(targetPosition);
        }

        private void Target_TouchUp(object sender, TouchEventArgs e)
        {
            var targetPosition = e.GetTouchPoint(GetMainContainerElement()).Position;
            HandleTouchUp(targetPosition);
        }

        private void HandleTouchUp(Point targetPosition)
        {
            var finalPosition = targetPosition;
            var endTime = DateTime.Now;
            var xDelta = Math.Abs(finalPosition.X - initialPosition.X);
            var yDelta = Math.Abs(finalPosition.Y - initialPosition.Y);
            var timeDelta = endTime.Subtract(startTime);
            if (xDelta <= xThreshold && yDelta <= yThreshold && timeDelta <= timeThreshold)
            {
                if (AssociatedPanel != null)
                {
                    var targetPanel = AssociatedPanel;
                    var originalPosition = initialPosition;

                    FindSelectedChild(targetPanel, originalPosition);
                }
            }
        }

        private void Target_TouchDown(object sender, TouchEventArgs e)
        {
            var targetPosition = e.GetTouchPoint(GetMainContainerElement()).Position;
            targetPosition = HandleTouchDown(targetPosition);
        }

        private Point HandleTouchDown(Point targetPosition)
        {
            initialPosition = targetPosition;
            startTime = DateTime.Now;
            return targetPosition;
        }

        private void FindSelectedChild(Panel targetPanel, Point originalPosition)
        {
            var visibleChildren = from FrameworkElement child
                                      in targetPanel.Children
                                  where child.IsVisible
                                  select child;

            foreach (var child in visibleChildren)
            {
                if (ContainsPoint(child, originalPosition))
                {
                    var childButton = child.FindVisualChild<ButtonBase>(ButtonName);
                    ICommand targetCommand = Command;
                    object targetCommandParameter = child.DataContext;
                    if (childButton != null && childButton.Command != null)
                    {
                        targetCommand = childButton.Command;
                    }
                    if (childButton != null && childButton.CommandParameter != null)
                    {
                        targetCommandParameter = childButton.CommandParameter;
                    }
                    if (targetCommand == null) { return; }
                    if (targetCommand.CanExecute(targetCommandParameter))
                    {
                        //var view = GetView(AssociatedObject);
                        //var position = originalPosition;
                        //if (view != null)
                        //{
                        //    position = GetPosition(view);
                        //}
                        targetCommand.Execute(targetCommandParameter);
                    }
                    else
                    {
                        var nestedItemsControl = child.FindVisualChild<ItemsControl>();
                        if (nestedItemsControl != null)
                        {
                            var nestedPanel = nestedItemsControl.FindPanel();
                            FindSelectedChild(nestedPanel, originalPosition);
                        }
                    }
                    //Since a visible child was found,
                    //it isn't necessary to continue searching for items.
                    break;
                }
            }
        }

        public bool ContainsPoint(FrameworkElement target, Point point)
        {
            GeneralTransform gt = GetMainContainerElement().TransformToVisual(target);
            var targetPoint = gt.Transform(point);
            var hit = VisualTreeHelper.HitTest(target, targetPoint);
            return hit != null;
        }

        private Point GetPosition(FrameworkElement target)
        {
            UIElement mainContainerElement = GetMainContainerElement();
            var position = target.TranslatePoint(new Point(0, 0), mainContainerElement);
            return position;
        }

        private UIElement GetMainContainerElement()
        {
            return Application.Current.MainWindow;
        }

        private UserControl GetView(DependencyObject source)
        {
            var parent = VisualTreeHelper.GetParent(source);
            if (parent == null) { return null; }
            if (parent is UserControl)
            {
                return parent as UserControl;
            }
            else
            {
                return GetView(parent);
            }
        }
    }
}