using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    [Table("ProfileReview")]

    public partial class ProfileReview
    {
        [Key]
        public int ProfileReviewId { get; set; }

        public string ProposedFromUserId { get; set; }

        public string ProposedToUserId { get; set; }

        public bool HasMadeProposal { get; set; }

        public DateTime DateMadeProposal { get; set; }

        public bool HasAcceptedDiscussion { get; set; }

        public DateTime? DateAcceptedDiscussion { get; set; }

        public bool HasAcceptedProposal { get; set; }

        public DateTime? DateAcceptedProposal { get; set; }

        public bool HasRejectedProposal { get; set; }

        public DateTime? DateRejectedProposal { get; set; }

        public bool HasCancelledProposal { get; set; }

        public DateTime? DateCancelledProposal { get; set; }
    }
}
