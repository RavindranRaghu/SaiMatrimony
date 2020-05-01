using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SaiMatrimony.Models
{
    [Table("ProfileComment")]

    public partial class ProfileComment
    {       
        [Key]
        public int CommentId { get; set; }

        public int ProfileReviewId { get; set; }

        public string CommentText { get; set; }

        public string CommentByUserId { get; set; }

        public string CommentByUserName { get; set; }

        public DateTime CommentDate { get; set; }

    }
}
