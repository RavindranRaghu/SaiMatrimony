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
        public List<ProfileDetails> GetProfilesEndPoint(SaiMatrimonyDb db, string edu, string pro, string gen, string location, string category)
        {
            List<ProfileDetails> allProfiles = (from p in db.ProfileDetails
                                                where p.IsProfileApproved
                                                select p
                                                ).ToList();
                
            return GetProfilesLogic(allProfiles, edu, pro, gen, location, category);
        }

        // Logic should be Unit Tested
        public List<ProfileDetails> GetProfilesLogic(List<ProfileDetails> profiles, string edu, string pro, string gen, string location, string category)
        {            
            CommonFunction stringFunctions = new CommonFunction();
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
