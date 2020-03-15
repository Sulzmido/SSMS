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
        [HttpGet("{institutionCode}/{id}")]

        public async Task<IActionResult> GetLevel([FromRoute]string institutionCode, [FromRoute]int id)
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

            _levelDAO.ConnectionString = connectionString;

            try
            {
                var level = await _levelDAO.FindAsync(id);

                if (level == null)
                {
                    return NotFound();
                }

                return Ok(level);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        // PUT: api/Levels/5
        [HttpPut("{institutionCode}/{id}")]

        public async Task<IActionResult> PutLevel([FromRoute] string institutionCode, [FromRoute] int id, [FromBody] Level level)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != level.ID)
            {
                return BadRequest();
            }


            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            _levelDAO.ConnectionString = connectionString;

            try
            {
                await _levelDAO.UpdateAsync(level);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("A system error occured " + ex.Message);
            }

            return NoContent();
        }

        // POST: api/Levels
        [HttpPost("{institutionCode}")]
        public async Task<IActionResult> PostLevel([FromRoute] string institutionCode, [FromBody] Level level)
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

            _levelDAO.ConnectionString = connectionString;

            try
            {
                level = await _levelDAO.InsertAsync(level);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return CreatedAtAction("GetsubjectCategory", new { id = level.ID }, level);
        }

        // DELETE: api/Levels/5
        [HttpDelete("{institutionCode}/{id}")]

        public async Task<IActionResult> DeleteLevel([FromRoute] string institutionCode, [FromRoute] int id)
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

            _levelDAO.ConnectionString = connectionString;

            var level = await _levelDAO.FindAsync(id);

            if (level == null)
            {
                return NotFound();
            }

            try
            {
                await _levelDAO.DeleteAsync(level);
            }
            catch (Exception ex)
            {
                // Log exception.
                return BadRequest(ex.ToString());
            }

            return Ok(level);
        }

        //private bool LevelExists(int id)
        //{
        //    return _context.Level.Any(e => e.ID == id);
        //}
    }
}