using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    [Table("ProfileApproved")]

    public partial class ProfileAdmin
    {
        [Key]
        public int ApprovalId { get; set; }

        public string UserId { get; set; }
        
        public DateTime ApprovedDate { get; set; }

    }
}
