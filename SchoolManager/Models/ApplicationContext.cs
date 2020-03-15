using InstitutionsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManager.Models
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source =.\SQLEXPRESS; Initial Catalog = Christ_Redeemers_Secondary_School; User ID = sa; Password = P@ssw0rd");
        }

        public DbSet<InstitutionsAPI.Core.Models.SubjectCategory> SubjectCategories { get; set; }

        public DbSet<InstitutionsAPI.Core.Models.Subject> Subjects { get; set; }

        public DbSet<InstitutionsAPI.Core.Models.Student> Student { get; set; }

        public DbSet<InstitutionsAPI.Core.Models.Institution> Institution { get; set; }
    }
}
