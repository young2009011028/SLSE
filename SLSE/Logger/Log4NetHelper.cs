
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using System.Collections.ObjectModel;

namespace SLSE.Logger
{


    public sealed class Log4NetHelper
    {
        private static readonly Log4NetHelper instance = new Log4NetHelper();
        private ObservableCollection<LogEntry> realtime_log; 
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Log4NetHelper()
        {
          
        }

        private Log4NetHelper()
        {
            realtime_log = new ObservableCollection<LogEntry>();
        }

        public static Log4NetHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #region public function
        public void LogEntries(LogEntry entry)
        {
            realtime_log.Add(entry);
        }
        public ObservableCollection<LogEntry> GetEntries()
        {
            return realtime_log;
        }
        #endregion
    }

   
 

}
