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
            catch (Exception ex)
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
            string query = "SELECT * FROM Building b, Building_Person bp WHERE b.ID = bp.Building_ID AND bp.Person_ID = @Person_ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                connection.Open();

                adapter.SelectCommand.Parameters.AddWithValue("@Person_ID", Id);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        int id = Convert.ToInt32(dt.Rows[i]["ID"]);
                        int companyID = Convert.ToInt32(dt.Rows[i]["Company_ID"]);
                        string street = (string)dt.Rows[i]["Street"];
                        string zipcode = (string)dt.Rows[i]["Zipcode"];
                        int houseNumber = Convert.ToInt32(dt.Rows[i]["HouseNumber"]);
                        list.Add(new Building(id, companyID, street, zipcode, houseNumber));
                    }
                    catch (Exception ex) { }
                }
            }
            return list;
        }
    }
}
