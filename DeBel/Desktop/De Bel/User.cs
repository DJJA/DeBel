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
        public List<Doorbell> Doorbells { get; set; }

        public User()
        {

        }

        public static User LogInCheck(string username, string password)
        {
            
        }

        public static DataTable GetUsers()
        {
            DataTable dt;
            string query = "SELECT * FROM Person";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@FilterValue", filterValue);

                dt = new DataTable();
                adapter.Fill(dt);
            }
            return dt;
        }

        public static bool UpdateUsers(DataTable dt)
        {

        }

        public static List<Doorbell> GetDoorbells(Building b)
        {

        }

        public List<User> GetUsers(Building b)
        {
            DataTable dt;
            string query = "SELECT * FROM Person";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@FilterValue", filterValue);

                dt = new DataTable();
                adapter.Fill(dt);
            }
            return dt;
        }

        public List<Building> GetBuildings()
        {

        }
    }
}
