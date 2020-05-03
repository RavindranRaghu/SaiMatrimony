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
        public KeyValuePair<bool, string> SetMatch(SaiMatrimonyDb db, string proposedFromUserId, string proposedToUserId, string actionType, int reviewId)
        {
            ProfileReview profileReview = new ProfileReview();
            ProfileDetails fromUser = db.ProfileDetails.Where(x => x.ProfileUserId == proposedFromUserId).FirstOrDefault();
            ProfileDetails toIdUser = db.ProfileDetails.Where(x => x.ProfileUserId == proposedToUserId).FirstOrDefault();            
            ProfileComment comment = new ProfileComment();
            CommonFunction common = new CommonFunction();
            string commentText = string.Empty;
            string msg = "";
            bool result = false;

            try
            {                
                if (actionType == "makeproposal")
                {
                    profileReview.ProposedFromUserId = proposedFromUserId;
                    profileReview.ProposedToUserId = proposedToUserId;
                    profileReview.HasMadeProposal = true;
                    profileReview.DateMadeProposal = DateTime.UtcNow;
                    profileReview.HasAcceptedDiscussion = false;
                    profileReview.HasAcceptedProposal = false;
                    profileReview.HasRejectedProposal = false;

                    db.ProfileReview.Add(profileReview);
                    db.SaveChanges();

                    comment = new ProfileComment();
                    comment.ProfileReviewId = profileReview.ProfileReviewId;
                    comment.CommentText = common.GetName(fromUser) + " proposed to " + common.GetName(toIdUser);
                    comment.CommentByUserName = common.GetName(fromUser) ;
                    comment.CommentByUserId = fromUser.ProfileUserId;
                    comment.CommentDate = DateTime.UtcNow;
                    db.ProfileComment.Add(comment);

                    db.SaveChanges();
                    return new KeyValuePair<bool, string>(true, "Proposal has been made, You will be notified once the Match accepts discussion or your proposal");
                }

                var hasprofile = db.ProfileReview.Where(x => x.ProfileReviewId == reviewId);
                if (hasprofile.Any())
                {
                    profileReview = hasprofile.FirstOrDefault();
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "System could not match the profile, Contact Support for details");
                }

                if (actionType == "acceptdiscussion" & profileReview.HasMadeProposal)
                {
                    profileReview.HasAcceptedDiscussion = true;
                    profileReview.DateAcceptedDiscussion = DateTime.UtcNow;
                    db.SaveChanges();
                    commentText = common.GetName(fromUser) + " accepted to discuss " + common.GetName(toIdUser);
                    result = true;
                    msg = "You have accepted to dicuss with the match";
                    //return new KeyValuePair<bool, string>(true, "You have accepted to dicuss with the match");
                }
                else if (actionType == "acceptproposal" & profileReview.HasMadeProposal)
                {
                    profileReview.HasAcceptedProposal = true;
                    profileReview.DateAcceptedProposal = DateTime.UtcNow;
                    db.SaveChanges();
                    commentText = common.GetName(fromUser) + " accepted proposal " + common.GetName(toIdUser);
                    result = true;
                    msg = "You have accepted the proposal with the match";
                    //return new KeyValuePair<bool, string>(true, "Match has been accepted. You will no longer have access to other profiles");
                }
                else if (actionType == "rejectproposal" & profileReview.HasMadeProposal)
                {
                    profileReview.HasRejectedProposal = true;
                    profileReview.DateRejectedProposal = DateTime.UtcNow;
                    db.SaveChanges();
                    commentText = common.GetName(fromUser) + " rejected proposal " + common.GetName(toIdUser);
                    result = true;
                    msg = "You have rejected the proposal with the match";
                    //return new KeyValuePair<bool, string>(true, "Match has not been accepted. You will no longer have access to this profile");
                }

                comment = new ProfileComment();
                comment.ProfileReviewId = reviewId;
                comment.CommentText = commentText;
                comment.CommentByUserName = common.GetName(fromUser);
                comment.CommentByUserId = fromUser.ProfileUserId;
                comment.CommentDate = DateTime.UtcNow;
                db.ProfileComment.Add(comment);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                string ex = e.Message;
                return new KeyValuePair<bool, string>(false, "System could not match the profile, Contact Support for details");
            }

            if (result)
            {
                return new KeyValuePair<bool, string>(result, msg);
            }

            return new KeyValuePair<bool, string>(false, "System could not match the profile, Contact Support for details");
        }


    }

}
