using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    
    [Table("UserBasic")]
    public partial class UserBasic
    {
        [Key]
        public int UserIdId { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserIdSystem { get; set; }

        public string PdSystem { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
