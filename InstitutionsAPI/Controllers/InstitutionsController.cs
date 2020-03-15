using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstitutionsAPI.Contexts;
using InstitutionsAPI.Models;
using InstitutionsAPI.Utilities;

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionsController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        public InstitutionsController()
        {
            _context.Database.Migrate();
        }

        // GET: api/Institutions
        [HttpGet]
        public IEnumerable<Institution> GetInstitution()
        {
            return _context.Institutions;
        }

        // GET: api/Institutions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstitution([FromRoute] int id)
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

            return Ok(institution);
        }

        // PUT: api/Institutions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstitution([FromRoute] int id, [FromBody] Institution institution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != institution.ID)
            {
                return BadRequest();
            }

            _context.Entry(institution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstitutionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Institutions
        [HttpPost]
        public async Task<IActionResult> PostInstitution([FromBody] Institution institution)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbName = institution.Name.Replace(' ', '_');
            institution.ConnectionString = @"Data Source =.\SQLEXPRESS; Initial Catalog = "+dbName+"; User ID = sa; Password = P@ssw0rd";

            try
            {
                DatabaseHelper.CreateDatabase(dbName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            

            _context.Institutions.Add(institution);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstitution", new { id = institution.ID }, institution);
        }

        // DELETE: api/Institutions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstitution([FromRoute] int id)
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

        private bool InstitutionExists(int id)
        {
            return _context.Institutions.Any(e => e.ID == id);
        }
    }
}