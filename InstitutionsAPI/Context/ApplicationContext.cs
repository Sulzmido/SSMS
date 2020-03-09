using InstitutionsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Contexts
{
    public class ApplicationContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source =.\SQLEXPRESS; Initial Catalog = InstitutionsDB; User ID = sa; Password = P@ssw0rd");
        }
        public DbSet<InstitutionsAPI.Models.Institution> Institutions { get; set; }
        public DbSet<InstitutionsAPI.Models.Student> Students { get; set; }
    }
}
