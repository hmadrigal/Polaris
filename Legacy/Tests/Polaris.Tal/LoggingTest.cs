using Polaris.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Polaris.Portal.Tests
{

    /// <summary>
    ///This is a test class for LoggingTest and is intended
    ///to contain all LoggingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LoggingTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        #region Positive testing
        /// <summary>
        ///A test for LogWarning
        ///</summary>
        [TestMethod()]
        [DeploymentItem("loggingConfiguration.config")]
        public void LogWarningTest()
        {
            string formatMessage = @"This is a WARNING message";
            object[] args = new string[0];
            Logging.LogWarning(formatMessage, args);
            Assert.Inconclusive(@"Please verify that this entry has been written in the appropiated destination.");
        }

        /// <summary>
        ///A test for LogMessage
        ///</summary>
        [TestMethod()]
        [DeploymentItem("loggingConfiguration.config")]
        public void LogMessageTest()
        {
            string formatMessage = @"This is a CUSTOM message. This is the first argument {0}";
            int eventId = 1;
            int priority = 0;
            TraceEventType severity = TraceEventType.Critical;
            string[] categories = new string[] { @"General" };
            object[] args = new string[] { "foo" };

            Logging.LogMessage(eventId, priority, severity, formatMessage, args);
            Logging.LogMessage(severity, formatMessage, args);
            Logging.LogMessage(formatMessage, eventId, priority, severity, categories, args);

            Assert.Inconclusive(@"Please verify that this entry has been written in the appropiated destination.");
        }

        /// <summary>
        ///A test for LogInformation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("loggingConfiguration.config")]
        public void LogInformationTest()
        {
            string formatMessage = @"This is a INFORMATION message";
            object[] args = new string[0];
            Logging.LogWarning(formatMessage, args);
            Assert.Inconclusive(@"Please verify that this entry has been written in the appropiated destination.");
        }

        /// <summary>
        ///A test for LogError
        ///</summary>
        [TestMethod()]
        [DeploymentItem("loggingConfiguration.config")]
        public void LogErrorTest()
        {
            string formatMessage = @"This is a ERROR message";
            object[] args = new string[0];
            Logging.LogWarning(formatMessage, args);
            Assert.Inconclusive(@"Please verify that this entry has been written in the appropiated destination.");
        }
        #endregion

        #region Negative Testing
        /// <summary>
        /// Missing arguments when string.format
        ///</summary>
        [TestMethod()]
        [DeploymentItem("loggingConfiguration.config")]
        public void LogMessageNegativeTest()
        {
            bool hasFailed = false;
            string errorMessage = string.Empty;

            // Mising arguments when formating the message
            string formatMessage = @"Using two argments {0} y {1} but providing only one!";
            int eventId = 1;
            int priority = 0;
            TraceEventType severity = TraceEventType.Critical;
            string[] categories = new string[] { @"General" };
            object[] args = new string[] { "one" };

            try
            {
               Logging.LogMessage(formatMessage, eventId, priority, severity, categories, args);
            }
            catch (System.ApplicationException ex)
            {
                hasFailed = true;
                errorMessage = ex.ToString();
            }
            Assert.IsTrue(hasFailed, "The method 'Logging.LogMessage' did not fail with a incorrect two arguments for the following formated string:\n{0}", formatMessage);

            // Checking an invalid eventId
            formatMessage = @"Using two argments: {0} y {1}";
            hasFailed = false;
            errorMessage = string.Empty;
            eventId = -1;
            args = new string[] { "one", "two" };
            severity = TraceEventType.Information;

            try
            {
                Logging.LogMessage(formatMessage, eventId, priority, severity, categories, args);
            }
            catch (System.ApplicationException ex)
            {
                hasFailed = true;
                errorMessage = ex.ToString();
            }
            Assert.IsTrue(hasFailed, "The EventID set to {0}, is not valid. However the Logging.LogMessage method did not fail.", eventId);

        }
        #endregion
    }
}
