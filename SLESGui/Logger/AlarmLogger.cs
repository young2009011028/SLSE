using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLSE.Logger
{
    class AlarmLogger
    {
        private static readonly AlarmLogger instance = new AlarmLogger();
        private BindingList<LogEntry> realtime_log; 
        private int alarm_sample_num = 5;
        Dictionary<DateTime, List<string>> alarm_table;
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AlarmLogger()
        {
          
        }

        private AlarmLogger()
        {
            realtime_log = new BindingList<LogEntry>();
            alarm_table = new Dictionary<DateTime, List<string>>();
        }

        public static AlarmLogger Instance
        {
            get
            {
                return instance;
            }
        }

        #region public function

        public void GetAlarmSignal(DateTime time, List<string> signal_names)
        {
            alarm_table[time] = signal_names;
            if (alarm_table.Count > alarm_sample_num)
            {
                alarm_table.Remove(alarm_table.Keys.First());
            }
            List<List<string>> templist = new List<List<string>>();
            foreach(var alarm in alarm_table)
            {
                templist.Add(alarm.Value);
            }

            var signal_list = from list in templist
                    from signal_name in list
                    where templist.All(l => l.Any(o => o == signal_name))
                    orderby signal_name
                    select signal_name;

            foreach (var signal in signal_list)
            {
                LogEntry newlog = new LogEntry(time, "Alarm",signal + " is exceeding limit!");
                realtime_log.Add(newlog);
            }
        }

        public BindingList<LogEntry> GetEntries()
        {
            return realtime_log;
        }

        public void SetAlarmThreshold(int num)
        {
            alarm_sample_num = num;
        }
        #endregion
    }
}
