using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstitutionsAPI.Contexts;
using InstitutionsAPI.Models;
using InstitutionsAPI.DAL;

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelsController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly LevelDAO _levelDAO = new LevelDAO();

        public LevelsController()
        {
        }

        // GET: api/Levels
        [HttpGet("{institutionCode}")]
        public IEnumerable<Level> GetLevel([FromRoute] string institutionCode)
        {
            IEnumerable<Level> levels = new List<Level>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return levels;

            _levelDAO.ConnectionString = connectionString;
            try
            {
                levels = _levelDAO.GetAll();
                return levels;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return levels;
            }
        }

        // GET: api/Levels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var level = await _context.Level.FindAsync(id);

            if (level == null)
            {
                return NotFound();
            }

            return Ok(level);
        }

        // PUT: api/Levels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLevel([FromRoute] int id, [FromBody] Level level)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != level.ID)
            {
                return BadRequest();
            }

            _context.Entry(level).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LevelExists(id))
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

        // POST: api/Levels
        [HttpPost]
        public async Task<IActionResult> PostLevel([FromBody] Level level)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Level.Add(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLevel", new { id = level.ID }, level);
        }

        // DELETE: api/Levels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var level = await _context.Level.FindAsync(id);
            if (level == null)
            {
                return NotFound();
            }

            _context.Level.Remove(level);
            await _context.SaveChangesAsync();

            return Ok(level);
        }

        private bool LevelExists(int id)
        {
            return _context.Level.Any(e => e.ID == id);
        }
    }
}