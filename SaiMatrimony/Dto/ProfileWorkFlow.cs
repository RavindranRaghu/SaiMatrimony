using SaiMatrimony.Models;
using SaiMatrimony.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Dto
{
    public class ProfileWorkFlow
    {
        public bool SetPhase (SaiMatrimonyDb db, string loggedInUserId, string toUserId, string actionType)
        {
            bool success = false;
            bool hasExistingProfile = false;
            bool isByUser = false;
            ProfileReview profileReview = new ProfileReview();

            if (actionType == "initiatediscussion")
            {
                success = true;
                profileReview.ProposedByUserId = loggedInUserId;
                profileReview.ProposedToUserId = toUserId;
                profileReview.HasInitiatedDiscussion = true;
                profileReview.DateAcceptedDiscussion = DateTime.UtcNow;
                db.ProfileReview.Add(profileReview);
                db.SaveChanges();
                return true;
            }
            

            var hasprofile = db.ProfileReview.Where(x => x.ProposedByUserId == loggedInUserId && x.ProposedToUserId == toUserId);
            if (hasprofile.Any())
            {
                profileReview = hasprofile.FirstOrDefault();
                hasExistingProfile = true;
                isByUser = true;
            }
            else
            {
                var hasReverseProfile = db.ProfileReview.Where(x => x.ProposedByUserId == toUserId && x.ProposedToUserId == loggedInUserId);
                if (hasReverseProfile.Any())
                {
                    profileReview = hasReverseProfile.FirstOrDefault();
                    hasExistingProfile = true;
                }
            }

            
            if (actionType == "acceptdiscussion" && hasExistingProfile && ! isByUser )
            {
                success = true;
                profileReview.HasAcceptedDiscussion = true;
                profileReview.DateAcceptedDiscussion = DateTime.UtcNow;
                db.SaveChanges();
            }
            else if (actionType == "initiateproposal" && hasExistingProfile && isByUser)
            {
                success = true;
                profileReview.HasMadeProposal = true;
                profileReview.DateMadeProposal = DateTime.UtcNow;
                db.SaveChanges();
            }
            else if (actionType == "acceptproposal" && hasExistingProfile && ! isByUser)
            {
                success = true;
                profileReview.HasAcceptedProposal = true;
                profileReview.DateAcceptedProposal = DateTime.UtcNow;
                db.SaveChanges();
            }
            else
            {
                return false;
            }
            return success;
        }


    }

}
