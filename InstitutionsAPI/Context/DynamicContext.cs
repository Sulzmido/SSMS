using InstitutionsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Contexts
{
    public class DynamicContext : DbContext
    {
        // What is the cost of creating a context for every request ??
        // Check if connection string can be changed on an already initialized context.
        //  code simplicity vs speed.

        private readonly string _connectionString;
        public DynamicContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<InstitutionsAPI.Models.Student> Students { get; set; }
        public DbSet<InstitutionsAPI.Models.SubjectCategory> SubjectCategories { get; set; }
        public DbSet<InstitutionsAPI.Models.Subject> Subjects { get; set; }
    }
}
