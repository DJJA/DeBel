using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class Building
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

        //public bool AddBuilding()
        //{

        //}

        //public bool UpdateBuilding()
        //{

        //}

        //public static bool RemoveBuilding(Building b)
        //{

        //}
    }
}
