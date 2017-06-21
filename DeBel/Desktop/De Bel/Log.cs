using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Log : Database
    {
        //public int DoorbellId { get; set; }
        public Doorbell Doorbell { get; set; }
        public int UserId { get; set; }
        public LogType Type { get; set; }
        public DateTime DateTime { get; set; }
        public string PicturePath { get; set; }
        public string ErrorMessage { get; set; }


        public Log(Doorbell doorbell, int userId, DateTime dateTime, string picturePath, string errorMessage)
        {
            //DoorbellId = doorbellId;
            this.Doorbell = doorbell;
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

        public override string ToString()
        {
            //if (Type == LogType.None)
            //    return "--==   " + DateTime.ToString("dd-MM-yyyy") + "   ==--";
            //string logType;
            //if (!ErrorWindow)
            //{
            //    if (Type == LogType.Error)
            //        logType = "Error";
            //    else
            //        logType = "Doorbell Rang";
            //    return DateTime.ToString("HH:mm:ss") + " - " + logType;

            //}
            //else
            //{
            //    return DateTime.ToString("HH:mm:ss") + " - "
            //        + Environment.NewLine + " wat info";
            //}

            string logType;
            if (Type == LogType.Error)
                logType = "Error";
            else
                logType = "Doorbell Rang";
            return DateTime.ToString("HH:mm:ss") + " - " + logType;
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
                    list.Insert(i, new LogDateLabel(null, -1, lastDate, null, null));
                }
            }
            return list;
        }

        public static List<LogErrorLabel> GetErrors()
        {
            var list = new List<LogErrorLabel>();
            string query = "SELECT b.Street, b.HouseNumber, d.DoorBellName, e.EventDate, e.ErrorMsg FROM eventlog e, doorbell d, building b WHERE e.DoorBell_ID = d.ID AND d.Building_ID = B.ID AND e.ErrorMsg IS NOT NULL;";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                var dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        //int doorbellId = Convert.ToInt32(dt.Rows[i]["DoorBell_ID"]);
                        //int userId = Convert.ToInt32(dt.Rows[i]["Person_ID"]);
                        //DateTime dateTime = new DateTime(Convert.ToInt64(dt.Rows[i]["EventDate"]));
                        //string picturePath = (string)dt.Rows[i]["Picture"];
                        //string errorMessage = (string)dt.Rows[i]["Error"];
                        //list.Add(new LogErrorLabel(this, userId, dateTime, picturePath, errorMessage));

                        var street = (string)dt.Rows[i]["street"];
                        var houseNumber = Convert.ToInt32(dt.Rows[i]["houseNumber"]).ToString();
                        var doorbellName = (string)dt.Rows[i]["DoorbellName"];
                        DateTime dateTime = Convert.ToDateTime(dt.Rows[i]["EventDate"]);
                        var errorMessage = (string)dt.Rows[i]["ErrorMsg"];
                        list.Add(new LogErrorLabel(street, houseNumber, doorbellName, dateTime, errorMessage));
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return list;
        }
    }
}
