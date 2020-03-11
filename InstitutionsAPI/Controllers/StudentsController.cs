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
using Newtonsoft.Json;
using InstitutionsAPI.DAL;

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly StudentDAO _dao = new StudentDAO();

        public StudentsController()
        {
        }

        // GET: api/Students/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<Student> GetStudents([FromRoute] string institutionCode)
        {
            IEnumerable<Student> students = new List<Student>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return students;

            _dao.ConnectionString = connectionString;

            try
            {
                students = _dao.GetAll();
                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return students;
            }
        }

        // GET: api/Students/{institutionCode}/5
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

            _dao.ConnectionString = connectionString;

            try
            {
                var student = await _dao.FindAsync(id);

                if (student == null)
                {
                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // PUT: api/Students/{institutionCode}/5
        [HttpPut("{institutionCode}/{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] string institutionCode, [FromRoute] int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.ID)
            {
                return BadRequest();
            }

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            _dao.ConnectionString = connectionString;

            try
            {
                await _dao.UpdateAsync(student);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);// Log exception.
                return BadRequest("A system error occured " + ex.Message);
            }

            return NoContent();
        }

        // POST: api/Students/{institutionCode}
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

            _dao.ConnectionString = connectionString;

            try
            {
                student = await _dao.InsertAsync(student);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return CreatedAtAction("GetStudent", new { id = student.ID }, student);
        }

        // DELETE: api/Students/{institutionCode}/5
        [HttpDelete("{institutionCode}/{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] string institutionCode, [FromRoute] int id)
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

            _dao.ConnectionString = connectionString;

            var student = await _dao.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            try
            {
                await _dao.DeleteAsync(student);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return Ok(student);
        }
    }
}