using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Polaris.PhoneLib.Toolkit.Actions
{
    public class PhoneCallerAction : TargetedTriggerAction<ButtonBase>
    {
        #region PhoneNumber

        /// <summary>
        /// PhoneNumber Dependency Property
        /// </summary>
        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(PhoneCallerAction),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the PhoneNumber property. This dependency property 
        /// indicates ....
        /// </summary>
        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }

        #endregion

        #region ContactName

        /// <summary>
        /// ContactName Dependency Property
        /// </summary>
        public static readonly DependencyProperty ContactNameProperty =
            DependencyProperty.Register("ContactName", typeof(string), typeof(PhoneCallerAction),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the ContactName property. This dependency property 
        /// indicates ....
        /// </summary>
        public string ContactName
        {
            get { return (string)GetValue(ContactNameProperty); }
            set { SetValue(ContactNameProperty, value); }
        }
        
        #endregion

        protected override void Invoke(object parameter)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = PhoneNumber;
            phoneCallTask.DisplayName = ContactName;
            phoneCallTask.Show();
        }
    }
}
