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
    public class Building : Database
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public int HouseNumber { get; set; }
        public List<Doorbell> Doorbells { get; set; }

        public Building(int id, int companyId, string street, string zipcode, int housenumber)
        {
            Id = id;
            Street = street;
            Zipcode = zipcode;
            HouseNumber = housenumber;
        }

        public bool AddBuilding()
        {
            return false;
        }

        public bool UpdateBuilding()
        {
            return false;
        }

        public static bool RemoveBuilding(Building b)
        {
            return false;
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
                    catch (Exception ex) { }
                }
            }
            return users;
        }

        public List<Doorbell> GetDoorbells(User usr)
        {
            var list = new List<Doorbell>();
            string query = "SELECT * FROM DoorBell d, DoorBell_perosn dp WHERE d.ID = dp.DoorBell_ID AND d.Building_ID = @Building_ID AND dp.Person_ID = @Person_ID";

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
                        list.Add(new Doorbell(id, name, buildingID));
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
    }
}
