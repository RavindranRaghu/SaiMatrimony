using SaiMatrimony.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Common
{
    public class CommonFunction
    {
        public bool CheckIsNullOrEmpty (string parm)
        {
            if (parm == null)
                return true;
            else if (parm.Trim().Length == 0)
                return true;
            else
                return false;
        }

        public string GetName(UserBasic user)
        {
            return (user.FirstName + " " + user.MiddleName + " " + user.LastName);
        }

        public string GetName(ProfileDetails profile)
        {
            return (profile.FirstName + " " + profile.MiddleName + " " + profile.LastName);
        }
    }
}
