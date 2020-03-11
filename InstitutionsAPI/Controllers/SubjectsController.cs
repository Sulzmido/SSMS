using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionsAPI.Contexts;
using InstitutionsAPI.DAL;
using InstitutionsAPI.Models;
using InstitutionsAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly SubjectDAO _dao = new SubjectDAO();

        public SubjectsController()
        {

        }

        // GET: api/Subjects/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<Subject> GetSubjects([FromRoute] string institutionCode)
        {
            IEnumerable<Subject> subjects = new List<Subject>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return subjects;

            _dao.ConnectionString = connectionString;

            try
            {
                subjects = _dao.GetAll();
                return subjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return subjects;
            }
        }

        // GET: api/Subjects/{institutionCode}/5
        [HttpGet("{institutionCode}/{id}")]
        public async Task<IActionResult> GetSubject([FromRoute] string institutionCode, [FromRoute]int id)
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
                var subject = await _dao.FindAsync(id);

                if (subject == null)
                {
                    return NotFound();
                }

                return Ok(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // PUT: api/Subjects/{institutionCode}/5
        [HttpPut("{institutionCode}/{id}")]
        public async Task<IActionResult> PutSubject([FromRoute] string institutionCode, [FromRoute] int id, [FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subject.ID)
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
                await _dao.UpdateAsync(subject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("An system error occured " + ex.Message);
            }

            return NoContent();
        }

        // POST: api/Subject/{institutionCode}
        [HttpPost("{institutionCode}")]
        public async Task<IActionResult> PostSubject([FromRoute] string institutionCode, [FromBody] Subject subject)
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
                subject = await _dao.InsertAsync(subject);
            }
            catch (Exception ex)
            {
                // Log Exception.
                return BadRequest(ex.ToString());
            }

            return CreatedAtAction("GetSubject", new { id = subject.ID }, subject);
        }

        // DELETE: api/Subjects/{institutionCode}/5
        [HttpDelete("{institutionCode}/{id}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] string institutionCode, [FromRoute] int id)
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

            var subject = await _dao.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            try
            {
                await _dao.DeleteAsync(subject);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return Ok(subject);
        }
    }
}