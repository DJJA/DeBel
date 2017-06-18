using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De_Bel
{
    public class UserDoorbellPermission
    {
        public User User { get; set; }
        public List<bool> Permissions { get; set; }

        public UserDoorbellPermission(User user, List<Doorbell> allDoorbells)
        {
            this.User = user;
            Permissions = new List<bool>();
            foreach (var db in allDoorbells)
            {
                if (User.Doorbells.Contains(db))
                    Permissions.Add(true);
                else
                    Permissions.Add(false);
            }
        }
    }
}
