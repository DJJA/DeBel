using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public string HouseNumber { get; set; }
        public List<Doorbell> Doorbells { get; set; }

        public Building()
        {

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

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
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
    }
}
