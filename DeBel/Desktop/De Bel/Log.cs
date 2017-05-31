using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Log
    {
        public LogType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string PicturePath { get; set; }
        public string ErrorMessage { get; set; }

        public Log(LogType type, DateTime dateTime, string picturePath, string errorMessage)
        {
            Type = type;
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
    }
}
