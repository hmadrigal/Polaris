using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Polaris.Extensions;

namespace Polaris.UnityInterceptors.Handlers
{
    public class LogCallHandler : ICallHandler
    {
		private LogWriter logWriter;
		private int eventId;
		private bool logBeforeCall;
		private bool logAfterCall;
		private string beforeMessage;
		private string afterMessage;
		private List<string> categories;
		private bool includeParameters;
		private bool includeCallStack;
		private bool includeCallTime;
		private int priority;
		private TraceEventType severity;
		private int order;
		/// <summary>
		/// Event ID to include in log entry
		/// </summary>
		/// <value>event id</value>
		public int EventId
		{
			get
			{
				return this.eventId;
			}
			set
			{
				this.eventId = value;
			}
		}
		/// <summary>
		/// Should there be a log entry before calling the target?
		/// </summary>
		/// <value>true = yes, false = no</value>
		public bool LogBeforeCall
		{
			get
			{
				return this.logBeforeCall;
			}
			set
			{
				this.logBeforeCall = value;
			}
		}
		/// <summary>
		/// Should there be a log entry after calling the target?
		/// </summary>
		/// <value>true = yes, false = no</value>
		public bool LogAfterCall
		{
			get
			{
				return this.logAfterCall;
			}
			set
			{
				this.logAfterCall = value;
			}
		}
		/// <summary>
		/// Message to include in a pre-call log entry.
		/// </summary>
		/// <value>The message</value>
		public string BeforeMessage
		{
			get
			{
				return this.beforeMessage;
			}
			set
			{
				this.beforeMessage = value;
			}
		}
		/// <summary>
		/// Message to include in a post-call log entry.
		/// </summary>
		/// <value>the message.</value>
		public string AfterMessage
		{
			get
			{
				return this.afterMessage;
			}
			set
			{
				this.afterMessage = value;
			}
		}
		/// <summary>
		/// Gets the collection of categories to place the log entries into.
		/// </summary>
		/// <remarks>The category strings can include replacement tokens. See
		/// the <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.CategoryFormatter" /> class for the list of tokens.</remarks>
		/// <value>The list of category strings.</value>
		public List<string> Categories
		{
			get
			{
				return this.categories;
			}
			set
			{
				this.categories.Clear();
				this.categories.AddRange(value);
			}
		}
		/// <summary>
		/// Should the log entry include the parameters to the call?
		/// </summary>
		/// <value>true = yes, false = no</value>
		public bool IncludeParameters
		{
			get
			{
				return this.includeParameters;
			}
			set
			{
				this.includeParameters = value;
			}
		}
		/// <summary>
		/// Should the log entry include the call stack?
		/// </summary>
		/// <remarks>Logging the call stack requires full trust code access security permissions.</remarks>
		/// <value>true = yes, false = no</value>
		public bool IncludeCallStack
		{
			get
			{
				return this.includeCallStack;
			}
			set
			{
				this.includeCallStack = value;
			}
		}
		/// <summary>
		/// Should the log entry include the time to execute the target?
		/// </summary>
		/// <value>true = yes, false = no</value>
		public bool IncludeCallTime
		{
			get
			{
				return this.includeCallTime;
			}
			set
			{
				this.includeCallTime = value;
			}
		}
		/// <summary>
		/// Priority for the log entry.
		/// </summary>
		/// <value>priority</value>
		public int Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
			}
		}
		/// <summary>
		/// Severity to log at.
		/// </summary>
		/// <value><see cref="T:System.Diagnostics.TraceEventType" /> giving the severity.</value>
		public TraceEventType Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = value;
			}
		}
		/// <summary>
		/// Log writer to log to.
		/// </summary>
		public LogWriter LogWriter
		{
			get
			{
				return this.logWriter;
			}
		}
		/// <summary>
		/// Gets or sets the order in which the handler will be executed
		/// </summary>
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		/// <summary>
		/// Creates a <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler" /> with default settings that writes
		/// to the default log writer.
		/// </summary>
		/// <remarks>See the <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandlerDefaults" /> class for the default values.</remarks>
		public LogCallHandler() : this(Logger.Writer)
		{
		}
		/// <summary>
		/// Creates a <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler" /> with default settings that writes
		/// to the given <see cref="P:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler.LogWriter" />.
		/// </summary>
		/// <remarks>See the <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandlerDefaults" /> class for the default values.</remarks>
		/// <param name="logWriter"><see cref="P:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler.LogWriter" /> to write logs to.</param>
        public LogCallHandler(LogWriter logWriter)
		{
			this.logBeforeCall = true;
			this.logAfterCall = true;
			this.beforeMessage = LogCallHandlerDefaults.BeforeMessage;
			this.afterMessage = LogCallHandlerDefaults.AfterMessage;
			this.categories = new List<string>();
			this.includeParameters = true;
			this.includeCallTime = true;
			this.priority = -1;
			this.severity = TraceEventType.Information;
			//base..ctor();
			this.logWriter = logWriter;
		}
		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler" /> that writes to the specified <see cref="P:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler.LogWriter" />
		/// using the given logging settings.
		/// </summary>
		/// <param name="logWriter"><see cref="P:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler.LogWriter" /> to write log entries to.</param>
		/// <param name="eventId">EventId to include in log entries.</param>
		/// <param name="logBeforeCall">Should the handler log information before calling the target?</param>
		/// <param name="logAfterCall">Should the handler log information after calling the target?</param>
		/// <param name="beforeMessage">Message to include in a before-call log entry.</param>
		/// <param name="afterMessage">Message to include in an after-call log entry.</param>
		/// <param name="includeParameters">Should the parameter values be included in the log entry?</param>
		/// <param name="includeCallStack">Should the current call stack be included in the log entry?</param>
		/// <param name="includeCallTime">Should the time to execute the target be included in the log entry?</param>
		/// <param name="priority">Priority of the log entry.</param>
        //public LogCallHandler(LogWriter logWriter, int eventId, bool logBeforeCall, bool logAfterCall, string beforeMessage, string afterMessage, bool includeParameters, bool includeCallStack, bool includeCallTime, int priority)
        //{
        //    this.logBeforeCall = true;
        //    this.logAfterCall = true;
        //    this.beforeMessage = LogCallHandlerDefaults.BeforeMessage;
        //    this.afterMessage = LogCallHandlerDefaults.AfterMessage;
        //    this.categories = new List<string>();
        //    this.includeParameters = true;
        //    this.includeCallTime = true;
        //    this.priority = -1;
        //    this.severity = TraceEventType.Information;
        //    //base..ctor();
        //    this.logWriter = logWriter;
        //    this.eventId = eventId;
        //    this.logBeforeCall = logBeforeCall;
        //    this.logAfterCall = logAfterCall;
        //    this.beforeMessage = beforeMessage;
        //    this.afterMessage = afterMessage;
        //    this.includeParameters = includeParameters;
        //    this.includeCallStack = includeCallStack;
        //    this.includeCallTime = includeCallTime;
        //    this.priority = priority;
        //}
		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler" /> that writes to the specified <see cref="P:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler.LogWriter" />
		/// using the given logging settings.
		/// </summary>
		/// <param name="logWriter"><see cref="P:Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler.LogWriter" /> to write log entries to.</param>
		/// <param name="eventId">EventId to include in log entries.</param>
		/// <param name="logBeforeCall">Should the handler log information before calling the target?</param>
		/// <param name="logAfterCall">Should the handler log information after calling the target?</param>
		/// <param name="beforeMessage">Message to include in a before-call log entry.</param>
		/// <param name="afterMessage">Message to include in an after-call log entry.</param>
		/// <param name="includeParameters">Should the parameter values be included in the log entry?</param>
		/// <param name="includeCallStack">Should the current call stack be included in the log entry?</param>
		/// <param name="includeCallTime">Should the time to execute the target be included in the log entry?</param>
		/// <param name="priority">Priority of the log entry.</param>
		/// <param name="order">Order in which the handler will be executed.</param>
        //public LogCallHandler(LogWriter logWriter, int eventId, bool logBeforeCall, bool logAfterCall, string beforeMessage, string afterMessage, bool includeParameters, bool includeCallStack, bool includeCallTime, int priority, int order) : this(logWriter, eventId, logBeforeCall, logAfterCall, beforeMessage, afterMessage, includeParameters, includeCallStack, includeCallTime, priority)
        //{
        //    this.order = order;
        //}
		/// <summary>
		/// Executes the call handler.
		/// </summary>
		/// <param name="input"><see cref="T:Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation" /> containing the information about the current call.</param>
		/// <param name="getNext">delegate to get the next handler in the pipeline.</param>
		/// <returns>Return value from the target method.</returns>
		public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
		{
			if (getNext == null)
			{
				throw new ArgumentNullException("getNext");
			}
			this.LogPreCall(input);
			Stopwatch sw = new Stopwatch();
			if (this.includeCallTime)
			{
				sw.Start();
			}
			IMethodReturn result = getNext()(input, getNext);
			if (this.includeCallTime)
			{
				sw.Stop();
			}
			this.LogPostCall(input, sw, result);
			return result;
		}
		private void LogPreCall(IMethodInvocation input)
		{
			if (this.logBeforeCall)
			{
				TraceLogEntry logEntry = this.GetLogEntry(input);
				logEntry.Message = this.beforeMessage;
                logEntry.Title = string.Format(" {0}.{1}",input.MethodBase.ReflectedType.FullName, input.MethodBase.Name);
				this.logWriter.TryWrite(logEntry.ToString());
			}
		}
		private void LogPostCall(IMethodInvocation input, Stopwatch sw, IMethodReturn result)
		{
			if (this.logAfterCall)
			{
				TraceLogEntry logEntry = this.GetLogEntry(input);
				logEntry.Message = this.afterMessage;
				logEntry.ReturnValue = null;
				if (result.ReturnValue != null && this.includeParameters)
				{
					logEntry.ReturnValue = result.ReturnValue.ToString();
				}
				if (result.Exception != null)
				{
					logEntry.Exception = result.Exception.ToString();
				}
				if (this.includeCallTime)
				{
					logEntry.CallTime = new TimeSpan?(sw.Elapsed);
				}
                logEntry.Title = string.Format(" {0}.{1}", input.MethodBase.ReflectedType.FullName, input.MethodBase.Name);
				this.logWriter.TryWrite(logEntry.ToString());
			}
		}
		private TraceLogEntry GetLogEntry(IMethodInvocation input)
		{
			TraceLogEntry logEntry = new TraceLogEntry();
			CategoryFormatter formatter = new CategoryFormatter(input.MethodBase);
			foreach (string category in this.categories)
			{
				logEntry.Categories.Add(formatter.FormatCategory(category));
			}
			logEntry.EventId = this.eventId;
			logEntry.Priority = this.priority;
			logEntry.Severity = this.severity;
			logEntry.Title = LogCallHandlerDefaults.Title;
			if (this.includeParameters)
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>();
				for (int i = 0; i < input.Arguments.Count; i++)
				{
					parameters[input.Arguments.GetParameterInfo(i).Name] = input.Arguments[i];
				}
				logEntry.ExtendedProperties = parameters;
			}
			if (this.includeCallStack)
			{
				logEntry.CallStack = Environment.StackTrace;
			}
			logEntry.TypeName = input.Target.GetType().FullName;
			logEntry.MethodName = input.MethodBase.Name;
			return logEntry;
		}
	}
}
