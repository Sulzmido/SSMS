using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using InstitutionsAPI.Contexts;
using InstitutionsAPI.DAL;
using InstitutionsAPI.Extensions;
using InstitutionsAPI.Models;
using InstitutionsAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectCategoriesController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly SubjectCategoryDAO _dao = new SubjectCategoryDAO();

        public SubjectCategoriesController()
        {
        }

        // GET: api/SubjectCategories/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<SubjectCategory> GetSubjectCategories([FromRoute] string institutionCode)
        {
            IEnumerable<SubjectCategory> subjectCategories = new List<SubjectCategory>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return subjectCategories;

            _dao.ConnectionString = connectionString;

            try
            {                
                subjectCategories = _dao.GetAll();
                return subjectCategories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return subjectCategories;
            }
        }

        // GET: api/SubjectCategories/{institutionCode}/5
        [HttpGet("{institutionCode}/{id}")]
        public async Task<IActionResult> GetSubjectCategory([FromRoute] string institutionCode, [FromRoute]int id)
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
                var subjectCategory = await _dao.FindAsync(id);

                if (subjectCategory == null)
                {
                    return NotFound();
                }

                return Ok(subjectCategory);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }  
        }

        // PUT: api/SubjectCategories/{institutionCode}/5
        [HttpPut("{institutionCode}/{id}")]
        public async Task<IActionResult> PutSubjectCategory([FromRoute] string institutionCode, [FromRoute] int id, [FromBody] SubjectCategory subjectCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != subjectCategory.ID)
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
                await _dao.UpdateAsync(subjectCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);// Log exception.
                return BadRequest("A system error occured " + ex.Message);
            }

            return NoContent();
        }

        // POST: api/SubjectCategories/{institutionCode}
        [HttpPost("{institutionCode}")]
        public async Task<IActionResult> PostSubjectCategory([FromRoute] string institutionCode, [FromBody] SubjectCategory subjectCategory)
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
                subjectCategory = await _dao.InsertAsync(subjectCategory);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return CreatedAtAction("GetsubjectCategory", new { id = subjectCategory.ID }, subjectCategory);
        }

        // DELETE: api/SubjectCategories/{institutionCode}/5
        [HttpDelete("{institutionCode}/{id}")]
        public async Task<IActionResult> DeleteSubjectCategory([FromRoute] string institutionCode, [FromRoute] int id)
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

            var subjectCategory = await _dao.FindAsync(id);

            if (subjectCategory == null)
            {
                return NotFound();
            }

            try
            {             
                await _dao.DeleteAsync(subjectCategory);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return Ok(subjectCategory);
        }
    }
}