using InstitutionsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Data.Contexts
{
    public class ApplicationContext : DbContext
    {
        // public DbSet<Institution> Institutions;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(@"Data Source =.\SQLEXPRESS; Initial Catalog = InstitutionsDB; User ID = sa; Password = P@ssw0rd");
        }
        public DbSet<Student> Students { get; set; }
    }
}
