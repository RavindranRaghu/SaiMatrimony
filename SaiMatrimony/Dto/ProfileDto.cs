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
        public List<ProfileDetails> GetProfilesEndPoint(SaiMatrimonyDb db, string edu, string pro, string gen, string location, string category, string fromid)
        {
            List<ProfileDetails> allProfiles = (from p in db.ProfileDetails
                                                where p.IsProfileApproved
                                                select p
                                                ).ToList();

            List<ProfileReview> matches = db.ProfileReview.ToList();

            List<string> imatches = (from m in matches
                                            where m.ProposedFromUserId == fromid
                                            select m.ProposedToUserId).ToList();

            List<string> omatches = (from m in matches
                                            where m.ProposedToUserId == fromid
                                            select m.ProposedFromUserId ).ToList();

            return GetProfilesLogic(allProfiles, imatches, omatches, edu, pro, gen, location, category);
        }

        // Logic should be Unit Tested
        public List<ProfileDetails> GetProfilesLogic(List<ProfileDetails> profiles, List<string> imatches, List<string> omatches, string edu, string pro, string gen, string location, string category)
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
            if (!stringFunctions.CheckIsNullOrEmpty(location))
            {
                profiles = profiles.Where(x => (x.City.ToLower()+ x.StateName.ToLower()  + x.Country.ToLower() + x.ZipCode.ToLower()).Contains(location.ToLower())).ToList();
            }

            if (category == "proposedbyme")
            {
                profiles = (from p in profiles
                            join r in imatches on p.ProfileUserId equals r
                            select p
                            ).ToList();
            }
            else if (category == "proposedtome") {
                profiles = (from p in profiles
                            join r in omatches on p.ProfileUserId equals r
                            select p
                ).ToList();
            }

            return profiles;
        }

    }

}
