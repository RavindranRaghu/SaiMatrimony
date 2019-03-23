using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Common
{
    public class StringFunctions
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
    }
}
