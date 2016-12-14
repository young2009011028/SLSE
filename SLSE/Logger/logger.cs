using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using log4net.Appender;
using System.Threading.Tasks;
using System.Windows;

namespace SLSE.Logger
{

    public class LogEntry
    {
        private string message;
        private string type;
        private DateTime time;
        public LogEntry(DateTime time, string type, string message)
        {
          this.message  = message;
          this.type = type;
          this.time = time;
        }
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
        public DateTime Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
            }
        }
        public override string ToString()
        {
            return this.Message;
        }

    }


    /// <summary>
    /// Logs to wpf logging window
    /// </summary>
   // public class WpfAppender : AppenderSkeleton
    //{

        /// <summary>
        /// Log target
        /// </summary>
       // public LoggingWindow LoggingWindow { get; set; }

        /// <summary>
        /// Addes a log entry to the Execution Control's LogEntries property
        /// </summary>
        /// <remarks>
        /// Appender does not log debug messages
        /// </remarks>

        //protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        //{
        //    if (loggingEvent.MessageObject.GetType().IsAssignableFrom(typeof(LogEntry)))
        //    {
        //        dynamic logEntry = (LogEntry)loggingEvent.MessageObject;

        //        if (this.LoggingWindow.LogEntries.Count == 0)
        //        {
        //            this.LoggingWindow.LogEntries.Add(logEntry);
        //        }
        //        else
        //        {
        //            this.LoggingWindow.LogEntries.Insert(0, logEntry);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format("The logging window received a log message of type '{0}' which was not convertable to an LogEntry", loggingEvent.MessageObject.GetType().Name));
        //    }
        //}

   // }
   
}
