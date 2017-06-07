using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Doorbell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Building Building { get; set; }
        public List<User> Users { get; set; }
        public List<Log> Logs { get; set; }

        public Doorbell()
        {

        }

        //public static bool UpdateDoorbells(DataTable dt)
        //{

        //}

        //public List<Log> GetErrors()
        //{

        //}

        //public List<Log> GetLog()
        //{

        //}
    }
}
