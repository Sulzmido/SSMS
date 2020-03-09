using InstitutionsAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionsAPI.Data.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Institution> Institutions;
    }
}
