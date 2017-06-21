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
        //public Doorbell Doorbell { get; set; }
        public int UserId { get; set; }
        public LogType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string PicturePath { get; set; }
        public string ErrorMessage { get; set; }

        public bool ErrorWindow { get; set; }

        public Log(int doorbellId, int userId, DateTime dateTime, string picturePath, string errorMessage)
        {
            ErrorWindow = false;
            DoorbellId = doorbellId;
            UserId = userId;

            if (String.IsNullOrEmpty(picturePath) && String.IsNullOrEmpty(errorMessage))
                Type = LogType.None;
            else if (String.IsNullOrEmpty(errorMessage))
                Type = LogType.DoorbellRang;
            else
                Type = LogType.Error;

            DateTime = dateTime;
            PicturePath = picturePath;
            ErrorMessage = errorMessage;
        }

        public Log(int doorbellId, int buildingId, string errormessage)
        {

        }

        public override string ToString()
        {
            if (Type == LogType.None)
                return "--==   " + DateTime.ToString("dd-MM-yyyy") + "   ==--";
            string logType;
            if (!ErrorWindow)
            {
                if (Type == LogType.Error)
                    logType = "Error";
                else
                    logType = "Doorbell Rang";
                return DateTime.ToString("HH:mm:ss") + " - " + logType;

            }
            else
            {
                return DateTime.ToString("HH:mm:ss") + " - "
                    + Environment.NewLine + " wat info";
            }
        }

        public static List<Log> AddDataLabelsToMatchesList(List<Log> logEntries)
        {
            var list = new List<Log>();
            foreach (var item in logEntries)
            {
                list.Add(item);
            }

            var lastDate = DateTime.MaxValue;
            for (int i = 0; i < list.Count; i++)
            {
                if (lastDate.Date != list[i].DateTime.Date)
                {
                    lastDate = list[i].DateTime.Date;
                    list.Insert(i, new Log(-1, -1, lastDate, null, null));
                }
            }
            return list;
        }
    }
}
