using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    [Table("ProfileMatch")]

    public partial class ProfileDetails
    {
        [Key]
        public int ProfileId { get; set; }

        public string UserId { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string MiddleName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public string StateName { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string Education { get; set; }

        public string Profession { get; set; }

        public string Interest { get; set; }

        public string Expectation { get; set; }

        public string YearOfBirth { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
