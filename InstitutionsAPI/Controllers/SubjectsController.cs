using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionsAPI.Contexts;
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
        private readonly string _tableName = "Subjects";

        public SubjectsController()
        {

        }

        // GET: api/Subjects/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<Subject> GetSubjects([FromRoute] string institutionCode)
        {
            IList<Subject> subjects = new List<Subject>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return subjects;
            try
            {
                var subjectsData = DatabaseHelper.ExecuteSelectQuery(connectionString, $"Select * from [dbo].[{_tableName}]");

                return subjectsData.Select(s => SerializationHelper.UnboxEntity<Subject>(s));
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

            IDictionary<string, object> subjectData = new Dictionary<string, object>();
            try
            {
                subjectData = await DatabaseHelper.ExecuteSelectFindQueryAsync(connectionString, $"Select * from [dbo].[{_tableName}] where ID={id}");

                if (subjectData.Count < 1)
                {
                    return NotFound();
                }

                var subject = SerializationHelper.UnboxEntity<Subject>(subjectData);

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

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            if (id != subject.ID)
            {
                return BadRequest();
            }

            // edit will be a little tricky  
            // a retrieval will happen first
            // we iterate through the values provided in the Subject parameter.

            try
            {
                if (subject.Category.GetType() == typeof(string))
                {
                    subject.Category = Convert.ToString(Convert.ToInt32(subject.Category));
                }
                else
                {
                    var category = ((JObject)subject.Category).ToObject<SubjectCategory>();
                    subject.Category = Convert.ToString(category.ID);
                }

                var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(Subject));
                var serializedEntity = JsonConvert.SerializeObject(subject, serializerSettings);

                await DatabaseHelper.ExecuteQueryAsync(connectionString, $@"Update [dbo].[{_tableName}] set [SerializedEntity] = '{serializedEntity}' where ID = {id}");
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

            try
            {
                var tableExists = DatabaseHelper.ExecuteTableCheck(connectionString, _tableName);

                if (!tableExists)
                {
                    DatabaseHelper.CreateTable(connectionString, _tableName);
                }

                // this part of the code can be astracted away
                if (subject.Category.GetType() == typeof(string))
                {
                    subject.Category = Convert.ToString(Convert.ToInt32(subject.Category));
                }
                else
                {
                    var category = ((JObject)subject.Category).ToObject<SubjectCategory>();
                    subject.Category = Convert.ToString(category.ID);
                } 

                var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(Subject));
                var serializedEntity = JsonConvert.SerializeObject(subject, serializerSettings);

                subject.ID = await DatabaseHelper.ExecuteInsertQueryAsync(connectionString, $@"INSERT INTO [dbo].[{_tableName}]
                                                                    ([SerializedEntity]) output INSERTED.ID VALUES ('{serializedEntity}')");
            }
            catch (Exception ex)
            {
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

            var subjectData = await DatabaseHelper.ExecuteSelectFindQueryAsync(connectionString, $"Select * from [dbo].[{_tableName}] where ID={id}");
            // find a better way of figuring out if the data exists in the database
            var subject = SerializationHelper.UnboxEntity<Subject>(subjectData);
            
            if (subject == null || subject.Name == null)
            {
                return NotFound();
            }

            try
            {
                await DatabaseHelper.ExecuteQueryAsync(connectionString, $@"DELETE FROM [dbo].[{_tableName}] WHERE ID='{subject.ID}'");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok(subject);
        }
    }
}