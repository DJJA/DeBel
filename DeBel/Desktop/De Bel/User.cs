using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class User
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

        public static List<User> GetUsers()
        {

        }

        public static bool UpdateUsers(DataTable dt)
        {

        }

        public static List<Doorbell> GetDoorbells(Building b)
        {

        }

        public List<User> GetUsers(Building b)
        {

        }

        public List<Building> GetBuildings()
        {

        }
    }
}
