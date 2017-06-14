using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public Doorbell(int id, string name, int buildingID)
        {
            Id = id;
            Name = name;
        }

        public static bool UpdateDoorbells(DataTable dt)
        {
            return false;
        }

        public List<Log> GetErrors()
        {
            var list = new List<Log>();
            string query = "SELECT * FROM EventLog WHERE DoorBell_ID = @DoorBell_ID AND Error IS NOT NULL;";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
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
                    catch (Exception) { }
                }
            }
            return list;
        }

        public List<Log> GetLog()
        {
            var list = new List<Log>();
            string query = "SELECT * FROM EventLog WHERE DoorBell_ID = @DoorBell_ID ORDER BY EventDate DESC;";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
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
                        DateTime dateTime = Convert.ToDateTime(dt.Rows[i]["EventDate"]);

                        string picturePath = null, errorMessage = null;

                        object o = dt.Rows[i]["Picture"];
                        if(o!= DBNull.Value)
                            picturePath = (string)o;
                        o = dt.Rows[i]["ErrorMsg"];
                        if (o != DBNull.Value)
                            errorMessage = (string)o;

                        list.Add(new Log(doorbellId, userId, dateTime, picturePath, errorMessage));
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return list;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
