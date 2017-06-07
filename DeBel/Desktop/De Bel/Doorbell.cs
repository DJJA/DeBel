using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Doorbell : Database
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Building Building { get; set; }
        public List<User> Users { get; set; }
        public List<Log> Logs { get; set; }

        public Doorbell(int id, string name, Building b)
        {
            Id = id;
            Name = name;
            Building = b;
        }

        public static bool UpdateDoorbells(DataTable dt)
        {
            return null;
        }

        public List<Log> GetErrors()
        {
            var list = new List<Log>();
            string query = "SELECT * FROM EventLog WHERE DoorBell_ID = @DoorBell_ID AND Error IS NOT NULL;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@DoorBell_ID", Id);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int doorbellId = Convert.ToInt32(dt.Rows[i]["DoorBell_ID"]);
                        int userId = Convert.ToInt32(dt.Rows[i]["Person_ID"]);
                        DateTime dateTime = new DateTime(Convert.ToInt64(dt.Rows[i]["EventDate"]));
                        string picturePath = (string)dt.Rows[i]["Picture"];
                        string errorMessage = (string)dt.Rows[i]["Error"];
                        list.Add(new Log(doorbellId, userId, dateTime, picturePath, errorMessage));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }

        public List<Log> GetLog()
        {
            var list = new List<Log>();
            string query = "SELECT * FROM EventLog WHERE DoorBell_ID = @DoorBell_ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Doorbell_ID", Id);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int doorbellId = Convert.ToInt32(dt.Rows[i]["DoorBell_ID"]);
                        int userId = Convert.ToInt32(dt.Rows[i]["Person_ID"]);
                        DateTime dateTime = new DateTime(Convert.ToInt64(dt.Rows[i]["EventDate"]));
                        string picturePath = (string)dt.Rows[i]["Picture"];
                        string errorMessage = (string)dt.Rows[i]["Error"];
                        list.Add(new Log(doorbellId, userId, dateTime, picturePath, errorMessage));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }
    }
}
