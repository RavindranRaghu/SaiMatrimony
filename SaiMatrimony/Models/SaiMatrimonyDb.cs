using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace SaiMatrimony.Models
{
    public partial class SaiMatrimonyDb :  DbContext
    {
        public SaiMatrimonyDb(DbContextOptions<SaiMatrimonyDb> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<ProfileDetails> ProfileDetails { get; set; }
        public virtual DbSet<ProfileApproved> ProfileApproved { get; set; }
        public virtual DbSet<ProfileReview> ProfileReview { get; set; }
        public virtual DbSet<ProfileComment> ProfileComment { get; set; }
        public virtual DbSet<UserBasic> UserBasic { get; set; }

        public virtual DbSet<UserRoleMap> UserRoleMap { get; set; }


    }
}
