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

namespace InstitutionsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly string _tableName = "Students";

        public StudentsController()
        {
        }

        // GET: api/Students/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<Student> GetStudents([FromRoute] string institutionCode)
        {
            IList<Student> students = new List<Student>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return students;
            try
            {
                var studentsData = DatabaseHelper.ExecuteSelectQuery(connectionString, $"Select * from [dbo].[{_tableName}]");

                return studentsData.Select(s => SerializationHelper.UnboxEntity<Student>(s));
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

            IDictionary<string, object> studentData = new Dictionary<string, object>();
            try
            {
                studentData = DatabaseHelper.ExecuteSelectFindQuery(connectionString, $"Select * from [dbo].[{_tableName}] where ID={id}");

                if (studentData.Count < 1)
                {
                    return NotFound();
                }

                var student = SerializationHelper.UnboxEntity<Student>(studentData);

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
                var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(Student));
                var serializedEntity = JsonConvert.SerializeObject(student, serializerSettings);

                DatabaseHelper.ExecuteQuery(connectionString, $@"Update [dbo].[{_tableName}] set [SerializedEntity] = '{serializedEntity}' where ID = {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("An system error occured " + ex.Message);
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

            try
            {
                var tableExists = DatabaseHelper.ExecuteTableCheck(connectionString, _tableName);

                if (!tableExists)
                {
                    DatabaseHelper.CreateTable(connectionString, _tableName);
                }

                var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(Student));
                var serializedEntity = JsonConvert.SerializeObject(student, serializerSettings);

                student.ID = DatabaseHelper.ExecuteInsertQuery(connectionString, $@"INSERT INTO [dbo].[{_tableName}]
                                                                    ([SerializedEntity]) output INSERTED.ID VALUES ('{serializedEntity}')");
            }
            catch (Exception ex)
            {
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

            var studentData = DatabaseHelper.ExecuteSelectFindQuery(connectionString, $"Select * from [dbo].[{_tableName}] where ID={id}");

            var student = SerializationHelper.UnboxEntity<Student>(studentData);

            if (student == null || student.Name == null)
            {
                return NotFound();
            }

            try
            {
                DatabaseHelper.ExecuteQuery(connectionString, $@"DELETE FROM [dbo].[{_tableName}] WHERE ID='{student.ID}'");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok(student);
        }
    }
}