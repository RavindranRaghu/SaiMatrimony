using SaiMatrimony.Models;
using SaiMatrimony.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Dto
{
    public class ProfileDto
    {
        public List<Profile> GetProfilesEndPoint(SaiMatrimonyDb db, string edu, string pro, string gen, string location, string category)
        {
            List<Profile> allProfiles = db.Profile.ToList();
            return GetProfilesLogic(allProfiles, edu, pro, gen, location, category);
        }

        // Logic should be Unit Tested
        public List<Profile> GetProfilesLogic(List<Profile> profiles, string edu, string pro, string gen, string location, string category)
        {            
            StringFunctions stringFunctions = new StringFunctions();
            if (!stringFunctions.CheckIsNullOrEmpty(edu))
            {
                profiles = profiles.Where(x => x.Education.ToLower().Contains(edu.ToLower())).ToList();
            }
            if (!stringFunctions.CheckIsNullOrEmpty(pro))
            {
                profiles = profiles.Where(x => x.Profession.ToLower().Contains(pro.ToLower())).ToList();
            }
            if (gen != "Select Gender")
            {
                gen = (gen.ToLower() == "male") ? "m" : "f";
                profiles = profiles.Where(x => x.Gender.ToLower().Contains(gen.ToLower())).ToList();
            }
            return profiles;
        }

    }

}
