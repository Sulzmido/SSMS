using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionsAPI.Contexts;
using InstitutionsAPI.Models;
using InstitutionsAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstitutionsAPI.Extensions;

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

        // GET: api/Institutions/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<Student> GetStudents([FromRoute] string institutionCode)
        {
            IList<Student> students = new List<Student>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return students;

            var studentsData = DatabaseHelper.ExecuteQuery(connectionString, "Select * from [dbo].[Students]");

            return studentsData.Select(s => s.ToObject<Student>());
          
        }

        // GET: api/Institutions/5
        [HttpGet("{institutionCode}/{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] string institutionCode, [FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            var studentExpando = DatabaseHelper.ExecuteFindQuery(connectionString, $"Select * from [dbo].[Students] where ID={id}");

            var student = studentExpando.ToObject<Student>();

            if (student == null || student.Name == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Institutions/5
        [HttpPut("{institutionCode}/{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] string institutionCode, [FromRoute] int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            if (id != student.ID)
            {
                return BadRequest();
            }

            try
            {
                DatabaseHelper.ExecutePureQuery(connectionString, $@"Update [dbo].[Students] set [Name] = '{student.Name}' where ID = {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return NoContent();
        }

        // POST: api/Institutions
        [HttpPost("{institutionCode}")]
        public async Task<IActionResult> PostStudent([FromRoute] string institutionCode, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            try
            {
                DatabaseHelper.ExecutePureQuery(connectionString, $@"INSERT INTO [dbo].[Students]
                                                                    ([Name]) VALUES ('{student.Name}')");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return CreatedAtAction("GetStudent", new { id = student.ID }, student);
        }

        // DELETE: api/Institutions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var institution = await _context.Institutions.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }

            _context.Institutions.Remove(institution);
            await _context.SaveChangesAsync();

            return Ok(institution);
        }
    }
}