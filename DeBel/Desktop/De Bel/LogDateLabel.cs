using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class LogDateLabel : Log
    {
        public LogDateLabel(Doorbell doorbell, int userId, DateTime dateTime, string picturePath, string errorMessage) : base(doorbell, userId, dateTime, picturePath, errorMessage)
        {

        }

        public override string ToString()
        {
            return "--==   " + DateTime.ToString("dd-MM-yyyy") + "   ==--";
        }
    }
}
