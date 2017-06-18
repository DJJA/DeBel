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
        public int BuildingId { get; set; }
        public List<User> Users { get; set; }
        private List<Log> _logs = null;
        public List<Log> Logs
        {
            get
            {
                if (_logs == null)
                    _logs = GetLog();
                return _logs;
            }
        }

        public Doorbell(int id, string name, int buildingID)
        {
            Id = id;
            Name = name;
            BuildingId = buildingID;
        }

        public static bool UpdateDoorbells(DataTable dt)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            var db = (Doorbell)obj;
            if (db.Id != Id) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public bool AddDoorbell()
        {
            bool success = true;
            try
            {
                string query = "INSERT INTO Doorbell(Building_ID, DoorbellName) VALUES(@Building_ID, @DoorbellName)";

                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Building_ID", BuildingId);
                    command.Parameters.AddWithValue("@DoorbellName", Name);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        public bool RemoveDoorbell()
        {
            bool success = true;
            try
            {
                string query = "DELETE FROM Doorbell WHERE ID = @Doorbell_ID";

                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Doorbell_ID", Id);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        public bool RemoveUsersThatDontHaveAccess(List<User> usersThatHaveAccess)
        {
            bool success = true;
            try
            {
                string query = "DELETE FROM Doorbell_Person WHERE Doorbell_ID = @Doorbell_ID AND Person_ID NOT IN(";
                for (int i = 0; i < usersThatHaveAccess.Count; i++)
                {
                    query += "@Person_ID" + i + ",";
                }
                query = query.Substring(0, query.Length - 1);   // Remove last comma
                query += ");";

                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@Doorbell_ID", Id);
                    for (int i = 0; i < usersThatHaveAccess.Count; i++)
                    {
                        command.Parameters.AddWithValue("@Person_ID" + i, usersThatHaveAccess[i].Id);
                    }
                    
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        private List<User> GetUsersThatHaveAccess()
        {
            var list = new List<User>();
            try
            {
                string query = "SELECT Person_ID FROM Doorbell_Person WHERE Doorbell_ID = @Doorbell_ID";

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
                            int userId = Convert.ToInt32(dt.Rows[i]["Person_ID"]);
                            list.Add(new User(userId));
                        }
                        catch (Exception) { }
                    }
                }
            }
            catch (Exception) { }
            return list;
        }

        public bool AddUserAccess(List<User> users)
        {
            bool success = true;
            var usersWithAccessInDB = GetUsersThatHaveAccess();
            foreach (var item in users)
            {
                if (!usersWithAccessInDB.Contains(item))
                    if (!AddUserAccess(item))
                        success = false;
            }
            return success;
        }

        private bool AddUserAccess(User user)
        {
            bool success = true;
            try
            {
                string query = "INSERT INTO Doorbell_Person (Doorbell_ID, Person_ID) VALUES(@Doorbell_ID, @Person_ID)";

                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@Doorbell_ID", Id);
                    command.Parameters.AddWithValue("@Person_ID", user.Id);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
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

        private List<Log> GetLog()
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
                        DateTime dateTime = Convert.ToDateTime(dt.Rows[i]["EventDate"]);


                        int userId = -1; 
                        object o = dt.Rows[i]["Person_ID"];
                        if (o != DBNull.Value)
                            userId = Convert.ToInt32(o);


                        string picturePath = null, errorMessage = null;

                        o = dt.Rows[i]["Picture"];
                        if(o != DBNull.Value)
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
