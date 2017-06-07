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
        public List<Doorbell> Doorbells { get; set; }

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

        public static User LogInCheck(string username, string password)
        {
            string query = "SELECT * FROM Person WHERE Username = '" + username + "' AND PersonPassword = '" + password + "'";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                return new User(id,name,email,username,password,phonenumber,adminstatus);
            }
            return null;
        }

        public static DataTable GetUsers()
        {
            DataTable dt;
            string query = "SELECT * FROM Person";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                //adapter.SelectCommand.Parameters.AddWithValue("@FilterValue", filterValue);

                dt = new DataTable();
                adapter.Fill(dt);
            }
            return dt;
        }

        //public static bool UpdateUsers(DataTable dt)
        //{

        //}

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

        //public List<Building> GetBuildings()
        //{

        //}
    }
}
