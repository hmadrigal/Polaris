using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Polaris.PhoneLib.Diagnostics
{
    /// <summary>
    /// Stores information about a crash exception. It is set up by 
    /// calling:
    /// Polaris.PhoneLib.Diagnostics.UnhandledExceptionDiagnosticsHelper.Instance.CheckForUnhandledExceptions(RootFrame);
    /// right after setting the RootVisual in your Windows Phone App.
    /// </summary>
    public sealed class UnhandledExceptionDiagnosticsHelper
    {
        public System.IO.IsolatedStorage.IsolatedStorageSettings ApplicationSettings
        {
            get { return _applicationSettings ?? (_applicationSettings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings); }
        }
        private System.IO.IsolatedStorage.IsolatedStorageSettings _applicationSettings;

        private PhoneApplicationFrame _rootFrame;

        public string CrashReportMessage { get; set; }

        public string CrashReportTitle { get; set; }

        public string CrashReportSubjectFormat { get; set; }

        public string CrashReportBodyFormat { get; set; }

        public string CrashReportEmail { get; set; }

        private void Initialize()
        {
            SetDefaultText();
            Application.Current.UnhandledException += OnApplicationUnhandledException;
        }

        public void CheckForUnhandledExceptions(PhoneApplicationFrame rootFrame)
        {
            if (rootFrame != null)
            {
                _rootFrame = rootFrame;
                rootFrame.Navigated += OnRootFrameNavigated;
            }
        }

        private void OnRootFrameNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            _rootFrame.Navigated -= OnRootFrameNavigated;
            _rootFrame = null;
            // Verifying if there was an error in previous execution
            if (ApplicationSettings.Contains("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException"))
            {
                var unhandledException = ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException"] as Exception;
                var unhandledExceptionMessage = ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.Message"];
                var unhandledExceptionStackTrace = ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.StackTrace"];
                var unhandledExceptionSource = ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.Source"];
                var unhandledExceptionHResult = ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.HResult"];
                var unhandledExceptionInnerException = ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.InnerException"];
                var messageBoxResult = MessageBox.Show(CrashReportMessage, CrashReportTitle, MessageBoxButton.OKCancel);
                if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
                {
                    var emailComposeTask = new Microsoft.Phone.Tasks.EmailComposeTask();
                    emailComposeTask.Subject = string.Format(CrashReportSubjectFormat, unhandledExceptionMessage, unhandledExceptionStackTrace, unhandledExceptionSource, unhandledExceptionHResult, unhandledExceptionInnerException);
                    emailComposeTask.Body = string.Format(CrashReportBodyFormat, unhandledExceptionMessage, unhandledExceptionStackTrace, unhandledExceptionSource, unhandledExceptionHResult, unhandledExceptionInnerException);
                    emailComposeTask.To = CrashReportEmail;
                    emailComposeTask.Show();
                }
                ApplicationSettings.Remove("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.Message");
                ApplicationSettings.Remove("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.StackTrace");
                ApplicationSettings.Remove("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.Source");
                ApplicationSettings.Remove("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.HResult");
                ApplicationSettings.Remove("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.InnerException");
                ApplicationSettings.Remove("76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException");
                ApplicationSettings.Save();
            }
        }

        private void OnApplicationUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            var exceptionType = e.ExceptionObject.GetType();
            ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException"] = exceptionType.IsSerializable ? e.ExceptionObject : null;
            ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.Message"] = e.ExceptionObject.Message;
            ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.StackTrace"] = e.ExceptionObject.StackTrace;
            ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.Source"] = e.ExceptionObject.Source;
            ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.HResult"] = e.ExceptionObject.HResult;
            var innerExceptionTypeIsSerializable = e.ExceptionObject.InnerException != null && e.ExceptionObject.InnerException.GetType().IsSerializable;
            ApplicationSettings["76991776-7356-4006-87CF-0DF0E46655AE_UnhandledException.InnerException"] = innerExceptionTypeIsSerializable ? e.ExceptionObject.InnerException : null;
            ApplicationSettings.Save();
        }

        private void SetDefaultText()
        {
            CrashReportBodyFormat = @"Module:
{2}

Mesage:{0}

StackTrace:
{1}

HResult:{3}

InnerException:
{3}";
            CrashReportEmail = @"Herberth.Madrigal@popagency.com";
            CrashReportMessage = @"It seems that the app has crashed. Do you want to send an email with details about the error?";
            CrashReportSubjectFormat = @"Application  Unhandled Exception: {0} at {2}";
            CrashReportTitle = @"Diagnostics Unhandled App Exception";
        }

        static UnhandledExceptionDiagnosticsHelper()
        {
            // forces to create the instance on start up
            var instance = UnhandledExceptionDiagnosticsHelper.Instance;
        }

        #region Singleton Pattern w/ Constructor
        private UnhandledExceptionDiagnosticsHelper()
            : base()
        {
            Initialize();
        }
        public static UnhandledExceptionDiagnosticsHelper Instance
        {
            get
            {
                return SingletonUnhandledExceptionDiagnosticsHelperCreator._Instance;
            }
        }
        private class SingletonUnhandledExceptionDiagnosticsHelperCreator
        {
            private SingletonUnhandledExceptionDiagnosticsHelperCreator() { }
            public static UnhandledExceptionDiagnosticsHelper _Instance = new UnhandledExceptionDiagnosticsHelper();
        }
        #endregion
    }


}
