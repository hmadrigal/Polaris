using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;

namespace Polaris.PhoneLib.Toolkit.Actions
{
    /// <summary>
    /// Calls a method on a specified object when invoked. 
    /// 
    /// </summary>
    public class InvokeMethodAction : System.Windows.Interactivity.TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(InvokeMethodAction), new PropertyMetadata(new PropertyChangedCallback(InvokeMethodAction.OnTargetObjectChanged)));
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(InvokeMethodAction), new PropertyMetadata(new PropertyChangedCallback(InvokeMethodAction.OnMethodNameChanged)));
        private List<InvokeMethodAction.MethodDescriptor> methodDescriptors;
        private const string CallMethodActionValidMethodNotFoundExceptionMessage = @"Could not find method named '{0}' on object of type '{1}' that matches the expected signature.";
        /// <summary>
        /// The object that exposes the method of interest. This is a dependency property.
        /// 
        /// </summary>
        public object TargetObject
        {
            get
            {
                return this.GetValue(InvokeMethodAction.TargetObjectProperty);
            }
            set
            {
                this.SetValue(InvokeMethodAction.TargetObjectProperty, value);
            }
        }

        /// <summary>
        /// The name of the method to invoke. This is a dependency property.
        /// 
        /// </summary>
        public string MethodName
        {
            get
            {
                return (string)this.GetValue(InvokeMethodAction.MethodNameProperty);
            }
            set
            {
                this.SetValue(InvokeMethodAction.MethodNameProperty, (object)value);
            }
        }

        private object Target
        {
            get
            {
                return this.TargetObject ?? (object)this.AssociatedObject;
            }
        }

        static InvokeMethodAction()
        {
        }

        public InvokeMethodAction()
        {
            this.methodDescriptors = new List<InvokeMethodAction.MethodDescriptor>();
        }

        /// <summary>
        /// Invokes the action.
        /// 
        /// </summary>
        /// <param name="parameter">The parameter of the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null)
                return;
            InvokeMethodAction.MethodDescriptor bestMethod = this.FindBestMethod(parameter);
            if (bestMethod != null)
            {
                ParameterInfo[] parameters = bestMethod.Parameters;
                if (parameters.Length == 0)
                {
                    bestMethod.MethodInfo.Invoke(this.Target, (object[])null);
                }
                else
                {
                    if (parameters.Length != 2 || this.AssociatedObject == null || (parameter == null || !parameters[0].ParameterType.IsAssignableFrom(this.AssociatedObject.GetType())) || !parameters[1].ParameterType.IsAssignableFrom(parameter.GetType()))
                        return;
                    bestMethod.MethodInfo.Invoke(this.Target, new object[2]
          {
            (object) this.AssociatedObject,
            parameter
          });
                }
            }
            else
            {
                if (this.TargetObject == null)
                    return;
                throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, CallMethodActionValidMethodNotFoundExceptionMessage, new object[2]
        {
          (object) this.MethodName,
          (object) this.TargetObject.GetType().Name
        }));
            }
        }

        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.UpdateMethodInfo();
        }

        /// <summary>
        /// Called when the action is getting detached from its AssociatedObject, but before it has actually occurred.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            this.methodDescriptors.Clear();
            base.OnDetaching();
        }

        private InvokeMethodAction.MethodDescriptor FindBestMethod(object parameter)
        {
            if (parameter != null)
                parameter.GetType();
            return Enumerable.FirstOrDefault<InvokeMethodAction.MethodDescriptor>((IEnumerable<InvokeMethodAction.MethodDescriptor>)this.methodDescriptors, (Func<InvokeMethodAction.MethodDescriptor, bool>)(methodDescriptor =>
            {
                if (!methodDescriptor.HasParameters)
                    return true;
                if (parameter != null)
                    return methodDescriptor.SecondParameterType.IsAssignableFrom(parameter.GetType());
                else
                    return false;
            }));
        }

        private void UpdateMethodInfo()
        {
            this.methodDescriptors.Clear();
            if (this.Target == null || string.IsNullOrEmpty(this.MethodName))
                return;
            foreach (MethodInfo methodInfo in this.Target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (this.IsMethodValid(methodInfo))
                {
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    if (InvokeMethodAction.AreMethodParamsValid(parameters))
                        this.methodDescriptors.Add(new InvokeMethodAction.MethodDescriptor(methodInfo, parameters));
                }
            }
            this.methodDescriptors = Enumerable.ToList<InvokeMethodAction.MethodDescriptor>((IEnumerable<InvokeMethodAction.MethodDescriptor>)Enumerable.OrderByDescending<InvokeMethodAction.MethodDescriptor, int>((IEnumerable<InvokeMethodAction.MethodDescriptor>)this.methodDescriptors, (Func<InvokeMethodAction.MethodDescriptor, int>)(methodDescriptor =>
            {
                int local_0 = 0;
                if (methodDescriptor.HasParameters)
                {
                    for (Type local_1 = methodDescriptor.SecondParameterType; local_1 != typeof(EventArgs); local_1 = local_1.BaseType)
                        ++local_0;
                }
                return methodDescriptor.ParameterCount + local_0;
            })));
        }

        private bool IsMethodValid(MethodInfo method)
        {
            return string.Equals(method.Name, this.MethodName, StringComparison.Ordinal) ;//&& method.ReturnType == typeof(void);
        }

        private static bool AreMethodParamsValid(ParameterInfo[] methodParams)
        {
            if (methodParams.Length == 2)
            {
                if (methodParams[0].ParameterType != typeof(object) || !typeof(EventArgs).IsAssignableFrom(methodParams[1].ParameterType))
                    return false;
            }
            else if (methodParams.Length != 0)
                return false;
            return true;
        }

        private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((InvokeMethodAction)sender).UpdateMethodInfo();
        }

        private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((InvokeMethodAction)sender).UpdateMethodInfo();
        }

        private class MethodDescriptor
        {
            public MethodInfo MethodInfo { get; private set; }

            public bool HasParameters
            {
                get
                {
                    return this.Parameters.Length > 0;
                }
            }

            public int ParameterCount
            {
                get
                {
                    return this.Parameters.Length;
                }
            }

            public ParameterInfo[] Parameters { get; private set; }

            public Type SecondParameterType
            {
                get
                {
                    if (this.Parameters.Length >= 2)
                        return this.Parameters[1].ParameterType;
                    else
                        return (Type)null;
                }
            }

            public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParams)
            {
                this.MethodInfo = methodInfo;
                this.Parameters = methodParams;
            }
        }
    }
}
