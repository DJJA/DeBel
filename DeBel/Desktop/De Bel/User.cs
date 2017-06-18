using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class User : Database
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public bool AdminStatus { get; set; }
        private List<Doorbell> _doorbells = null;
        public List<Doorbell> Doorbells
        {
            get
            {
                if (_doorbells == null)
                    _doorbells = GetDoorbells();
                return _doorbells;
            }
        }

        private List<Doorbell> GetDoorbells()
        {
            var list = new List<Doorbell>();
            string query = "SELECT * FROM DoorBell d, Doorbell_Person dp WHERE d.ID = dp.DoorBell_ID AND dp.Person_ID = @Person_ID";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Person_ID", Id);


                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["DoorBell_ID"]);
                        int buildingID = Convert.ToInt32(dt.Rows[i]["Building_ID"]);
                        string name = (string)dt.Rows[i]["doorbellName"];
                        list.Add(new Doorbell(id, name, buildingID));
                    }
                    catch (Exception) { }
                }
            }
            return list;
        }

        MySqlCommandBuilder builder;

        private static MySqlConnection connection = new MySqlConnection
        (@"Server=studmysql01.fhict.local;Uid=dbi338083;Database=dbi338083;Pwd=bossmonster;");

        public User(int id, string name, string email, string username, string password, int phonenumber, bool adminstatus)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PhoneNumber = phonenumber;
            this.AdminStatus = adminstatus;
        }

        public User(int id)
        {
            this.Id = id;
        }

        public static User LogInCheck(string username, string password)
        {
            string query = "SELECT * FROM Person WHERE Username = '" + username + "' AND PersonPassword = '" + password + "'";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                int id = Convert.ToInt32(dtbl.Rows[0]["ID"]);
                string name = (string)dtbl.Rows[0]["PersonName"];
                string email = (string)dtbl.Rows[0]["EMail"];
                int phoneNumber = Convert.ToInt32(dtbl.Rows[0]["PhoneNumber"]);
                string usrname = (string)dtbl.Rows[0]["Username"];
                string pssword = (string)dtbl.Rows[0]["PersonPassword"];
                bool adminStatus = Convert.ToBoolean(dtbl.Rows[0]["AdminStatus"]);
                return new User(id, name, email, usrname, pssword, phoneNumber, adminStatus);
            }
            return null;
        }

        public static DataTable GetUsers()
        {
            MySqlDataAdapter dataAdp;
            DataTable dt;
            connection.Open();
            string Query = "SELECT * FROM person";
            MySqlCommand createCommand = new MySqlCommand(Query, connection);
            createCommand.ExecuteNonQuery();

            dataAdp = new MySqlDataAdapter(createCommand);
            dt = new DataTable("person");
            dataAdp.Fill(dt);
            connection.Close();
            return dt;
        }

        public static List<User> GetUsersAsList()
        {
            var list = new List<User>();
            string query = "SELECT * FROM Person";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int id = Convert.ToInt32(dt.Rows[i]["ID"]);
                    string name = (string)dt.Rows[i]["PersonName"];
                    string email = (string)dt.Rows[i]["EMail"];
                    int phoneNumber = Convert.ToInt32(dt.Rows[i]["PhoneNumber"]);
                    string usrname = (string)dt.Rows[i]["Username"];
                    string pssword = (string)dt.Rows[i]["PersonPassword"];
                    bool adminStatus = Convert.ToBoolean(dt.Rows[i]["AdminStatus"]);
                    list.Add(new User(id, name, email, usrname, pssword, phoneNumber, adminStatus));
                }
            }
            return list;
        }

        public static bool UpdateUsers(DataTable dt)
        {
            bool update = true;
            try
            {
                MySqlDataAdapter dataAdp;
                string Query = "SELECT * FROM person";
                MySqlCommand createCommand = new MySqlCommand(Query, connection);
                dataAdp = new MySqlDataAdapter(createCommand);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(dataAdp);
                dataAdp.Update(dt);
            }
            catch (Exception)
            { update = false; }
            return update;
        }

        //public static List<Doorbell> GetDoorbells(Building b)
        //{

        //}

        //public List<User> GetUsers(Building b)
        //{
        //    DataTable dt;
        //    string query = "SELECT * FROM Person";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
        //    {
        //        connection.Open();

        //        //adapter.SelectCommand.Parameters.AddWithValue("@FilterValue", filterValue);

        //        dt = new DataTable();
        //        adapter.Fill(dt);
        //    }
        //    return dt;
        //}

        public List<Building> GetBuildings()
        {
            var list = new List<Building>();
            //string query = "SELECT * FROM Building b, Building_Person bp WHERE b.ID = bp.Building_ID AND bp.Person_ID = @Person_ID ORDER BY Street ASC;";
            //string query = "SELECT * FROM Building ORDER BY Street ASC;";
            string query = "SELECT * FROM building WHERE ID IN( SELECT DISTINCT Building_ID FROM doorbell WHERE ID IN( SELECT Doorbell_ID FROM `doorbell_person` WHERE Person_ID = @Person_ID ) )";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Person_ID", Id);

                var dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["ID"]);
                        //int companyID = Convert.ToInt32(dt.Rows[i]["Company_ID"]);
                        string street = (string)dt.Rows[i]["Street"];
                        string zipcode = (string)dt.Rows[i]["Zipcode"];
                        int houseNumber = Convert.ToInt32(dt.Rows[i]["HouseNumber"]);
                        //var houseNumber = (string)dt.Rows[i]["HouseNumber"];
                        list.Add(new Building(id, -1, street, zipcode, houseNumber));
                    }
                    catch (Exception) { }
                }
            }
            return list;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            var db = (User)obj;
            if (db.Id != Id) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
