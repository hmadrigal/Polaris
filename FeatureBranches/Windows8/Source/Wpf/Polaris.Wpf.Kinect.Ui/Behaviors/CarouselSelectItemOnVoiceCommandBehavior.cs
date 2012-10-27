using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Polaris.Extensions;
using Polaris.Services;
using Polaris.Windows.Controls;
using Polaris.Windows.Extensions;

namespace Polaris.Windows.Behaviors
{
    public class CarouselSelectItemOnVoiceCommandBehavior : Behavior<CarouselItemsControl>
    {
        #region SpeechRecognitionService

        /// <summary>
        /// SpeechRecognitionService Dependency Property
        /// </summary>
        public static readonly DependencyProperty SpeechRecognitionServiceProperty =
            DependencyProperty.Register("SpeechRecognitionService", typeof(ISpeechRecognitionService), typeof(CarouselSelectItemOnVoiceCommandBehavior),
                new FrameworkPropertyMetadata((ISpeechRecognitionService)default(ISpeechRecognitionService),
                    new PropertyChangedCallback(OnSpeechRecognitionServiceChanged)));

        /// <summary>
        /// Gets or sets the SpeechRecognitionService property.  This dependency property
        /// indicates the speech recognition service.
        /// </summary>
        public ISpeechRecognitionService SpeechRecognitionService
        {
            get { return (ISpeechRecognitionService)GetValue(SpeechRecognitionServiceProperty); }
            set { SetValue(SpeechRecognitionServiceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SpeechRecognitionService property.
        /// </summary>
        private static void OnSpeechRecognitionServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselSelectItemOnVoiceCommandBehavior)d).OnSpeechRecognitionServiceChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SpeechRecognitionService property.
        /// </summary>
        protected virtual void OnSpeechRecognitionServiceChanged(DependencyPropertyChangedEventArgs e)
        {
            TrySubscribe();
        }

        #endregion SpeechRecognitionService

        #region Command

        /// <summary>
        /// Command Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(String), typeof(CarouselSelectItemOnVoiceCommandBehavior),
                new FrameworkPropertyMetadata((String)"Select",
                    new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Gets or sets the Command property.  This dependency property
        /// indicates the command that will be registered by the behavior.
        /// </summary>
        public String Command
        {
            get { return (String)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselSelectItemOnVoiceCommandBehavior)d).OnCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Command property.
        /// </summary>
        protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            TrySubscribe();
        }

        #endregion Command

        private Guid listenerId;

        public CarouselSelectItemOnVoiceCommandBehavior()
        {
            listenerId = Guid.NewGuid();
        }

        bool subscribed = false;

        protected override void OnAttached()
        {
            base.OnAttached();
            TrySubscribe();
        }

        private void TrySubscribe()
        {
            if (subscribed) { return; }
            if (AssociatedObject == null) { return; }
            SpeechRecognitionService.Initialize();
            SpeechRecognitionService.Start();
            if (SpeechRecognitionService != null && !String.IsNullOrEmpty(Command) && AssociatedObject.Items.Count > 0)
            {
                //TODO: Implement an interface on the items to get the text. For now it will be the ToString method.
                var commandArguments = (from object item in AssociatedObject.ItemsSource.AsQueryable()
                                        select item.ToString()).ToArray();
                SpeechRecognitionService.AddSpeechCommandSet(Command, commandArguments, listenerId);
                SpeechRecognitionService.AddSpeechRecognizedListener(OnSpeechRecognized);
                subscribed = true;
            }
        }

        private void OnSpeechRecognized(ISpeechRecognitionPayload servicePayload)
        {
            Dispatcher.BeginInvoke(new Action<ISpeechRecognitionPayload>((payload) =>
            {
                if (String.Compare(payload.Command, Command) != 0) { return; }
                var children = AssociatedObject.FindVisualChild<Canvas>().Children;
                var commandArgumentItem = (from CarouselItem item in children
                                           where String.Compare(item.DataContext.ToString(), payload.Argument.Trim(), true) == 0
                                           select item).FirstOrDefault();
                if (commandArgumentItem != null)
                {
                    var newValue = AssociatedObject.ScrollPosition - commandArgumentItem.ScrollPosition;
                    AssociatedObject.AnimatedScrollPosition = newValue;
                }
            }), servicePayload);
        }
    }
}