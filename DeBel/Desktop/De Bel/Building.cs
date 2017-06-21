using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Building : Database
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public int HouseNumber { get; set; }
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

        public Building(int id, int companyId, string street, string zipcode, int housenumber)
        {
            Id = id;
            Street = street;
            Zipcode = zipcode;
            HouseNumber = housenumber;
        }

        public void RefreshDoorbells()
        {
            _doorbells = GetDoorbells();
        }

        public bool AddBuilding()
        {
            bool success = true;
            try
            {
                //return false;
                string query = "INSERT INTO Building(Street, Zipcode, HouseNumber) VALUES(@Street, @Zipcode, @HouseNumber)";

                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Street", Street);
                    command.Parameters.AddWithValue("@Zipcode", Zipcode);
                    command.Parameters.AddWithValue("@HouseNumber", HouseNumber);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        public bool UpdateBuilding()
        {
            return false;
        }

        public bool RemoveBuilding()
        {
            bool success = true;
            try
            {
                string query = "DELETE FROM Building WHERE ID = @Building_ID";

                using (var connection = new MySqlConnection(connectionString))
                using (var command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Building_ID", Id);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();
            string query = "SELECT * FROM Person p, Building_Person bp WHERE p.ID = bp.Person_ID AND bp.Building_ID = @Building_ID";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Building_ID", Id);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["Person_ID"]);
                        string name = (string)dt.Rows[i]["Name"];
                        string email = (string)dt.Rows[i]["EMail"];
                        int phoneNumber = Convert.ToInt32(dt.Rows[i]["PhoneNumber"]);
                        string username = (string)dt.Rows[i]["Username"];
                        string password = (string)dt.Rows[i]["PersonPassword"];
                        bool adminStatus = Convert.ToBoolean(dt.Rows[i]["AdminStatus"]);
                        users.Add(new User(id, name, email, username, password, phoneNumber, adminStatus));
                    }
                    catch (Exception) { }
                }
            }
            return users;
        }

        private List<Doorbell> GetDoorbells()
        {
            var list = new List<Doorbell>();
            string query = "SELECT * FROM DoorBell WHERE Building_ID = @Building_ID ORDER BY DoorbellName ASC";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Building_ID", Id);


                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["ID"]);
                        int buildingID = Convert.ToInt32(dt.Rows[i]["Building_ID"]);
                        string name = (string)dt.Rows[i]["doorbellName"];
                        list.Add(new Doorbell(id, name, this));
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return list;
        }

        public List<Doorbell> GetDoorbells(User usr)
        {
            var list = new List<Doorbell>();
            string query = "SELECT * FROM DoorBell d, DoorBell_person dp WHERE d.ID = dp.DoorBell_ID AND d.Building_ID = @Building_ID AND dp.Person_ID = @Person_ID ORDER BY DoorbellName ASC";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Building_ID", Id);
                adapter.SelectCommand.Parameters.AddWithValue("@Person_ID", usr.Id);


                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["doorbell_ID"]);
                        int buildingID = Convert.ToInt32(dt.Rows[i]["Building_ID"]);
                        string name = (string)dt.Rows[i]["doorbellName"];
                        list.Add(new Doorbell(id, name, this));
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return list;
        }

        public override string ToString()
        {
            return Street + " " + HouseNumber;
        }

        public static List<Building> GetBuildings()                    // This is in the wrong place
        {
            var list = new List<Building>();
            //string query = "SELECT * FROM Building b, Buidling_Person bp WHERE b.ID = bp.Building_ID AND bp.Person_ID = @Person_ID;";
            string query = "SELECT * FROM Building ORDER BY Street ASC;";

            using (var connection = new MySqlConnection(connectionString))
            using (var adapter = new MySqlDataAdapter(query, connection))
            {
                connection.Open();

                //adapter.SelectCommand.Parameters.AddWithValue("@Person_ID", Id);

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
    }
}
