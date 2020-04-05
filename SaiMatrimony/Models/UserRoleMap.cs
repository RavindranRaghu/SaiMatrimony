using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    
    [Table("UserRoleMap")]
    public partial class UserRoleMap
    {
        [Key]
        public int MapId { get; set; }

        [StringLength(255)]
        public string UserIdSystem { get; set; }

        public bool IsAdmin { get; set; }

        public string UpdateByName { get; set; }

        public string UpdateById { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
