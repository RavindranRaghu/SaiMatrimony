using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    [Table("ProfileApproved")]

    public partial class ProfileApproved
    {
        [Key]
        public int ApprovalId { get; set; }

        public string ProfileUserId { get; set; }

        public string ApprovedByName { get; set; }

        public string ApprovedById { get; set; }

        public DateTime ApprovedDate { get; set; }

    }
}
