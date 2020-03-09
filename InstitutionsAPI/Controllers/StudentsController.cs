using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionsAPI.Contexts;
using InstitutionsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        public StudentsController()
        {
        }

        // GET: api/Institutions
        [HttpGet("{institutionCode}")]
        public IEnumerable<Student> GetStudents([FromRoute] string institutionCode)
        {
            // get institution's connection string.
            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            return new List<Student>();
            // check appropriate database

        }
    }
}