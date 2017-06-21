using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class LogErrorLabel : Log
    {
        public string BuildingName { get; set; }
        public string DoorbellName { get; set; }
        public LogErrorLabel(string street, string houseNumber, string doorbellName, DateTime dateTime, string errorMessage) : base(null, -1, dateTime, null, errorMessage)
        {
            BuildingName = street + " " + houseNumber;
            DoorbellName = doorbellName;
        }

        public override string ToString()
        {
            return DateTime.ToString("dd-MM-yyyy HH:mm:ss - ") + BuildingName + " - " + DoorbellName + ": " + ErrorMessage;
        }
    }
}
