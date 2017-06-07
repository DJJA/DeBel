using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Log
    {
        public int DoorbellId { get; set; }
        public int UserId { get; set; }
        public LogType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string PicturePath { get; set; }
        public string ErrorMessage { get; set; }

        public Log(int doorbellId, int userId, DateTime dateTime, string picturePath, string errorMessage)
        {
            DoorbellId = doorbellId;
            UserId = userId;

            if (String.IsNullOrEmpty(picturePath) && String.IsNullOrEmpty(errorMessage))
                Type = LogType.None;
            else if (String.IsNullOrEmpty(errorMessage))
                Type = LogType.DoorbellRinged;
            else
                Type = LogType.Error;

            DateTime = dateTime;
            PicturePath = picturePath;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            if(Type == LogType.None)
                return "--==   " + DateTime.ToString("dd-MM-yyyy") + "   ==--";
            string logType;
            if (Type == LogType.Error)
                logType = "Error";
            else
                logType = "Doorbell Ringed";
            return DateTime.ToString("HH:mm:ss") + " - " + logType;
        }

        public static List<Log> AddDataLabelsToMatchesList(List<Log> LogEntries)
        {
            DateTime lastDate = DateTime.MaxValue;
            for (int i = 0; i < LogEntries.Count; i++)
            {
                if (lastDate.Date != LogEntries[i].DateTime.Date)
                {
                    lastDate = LogEntries[i].DateTime;
                    LogEntries.Insert(i, new Log(-1, -1, lastDate, null, null));
                }
            }
            return LogEntries;
        }
    }
}
