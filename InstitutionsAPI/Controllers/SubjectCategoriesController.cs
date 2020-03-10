using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using InstitutionsAPI.Contexts;
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
        private readonly string _tableName = "SubjectCategories";

        public SubjectCategoriesController()
        {
        }

        // GET: api/SubjectCategories/{institutionCode}
        [HttpGet("{institutionCode}")]
        public IEnumerable<SubjectCategory> GetSubjectCategories([FromRoute] string institutionCode)
        {
            IList<SubjectCategory> subjectCategories = new List<SubjectCategory>();

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;

            if (connectionString == null || string.IsNullOrEmpty(connectionString))
                return subjectCategories;
            try
            {
                var subjectCategoriesData = DatabaseHelper.ExecuteSelectQuery(connectionString, $"Select * from [dbo].[{_tableName}]");

                return subjectCategoriesData.Select(s => SerializationHelper.UnboxEntity<SubjectCategory>(s));
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

            IDictionary<string, object> subjectCategoryData = new Dictionary<string, object>();
            try
            {
                subjectCategoryData = DatabaseHelper.ExecuteSelectFindQuery(connectionString, $"Select * from [dbo].[{_tableName}] where ID={id}");

                if(subjectCategoryData.Count < 1)
                {
                    return NotFound();
                }

                var subjectCategory = SerializationHelper.UnboxEntity<SubjectCategory>(subjectCategoryData);

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

            var connectionString = _context.Institutions.Single(i => i.Code.Equals(institutionCode)).ConnectionString;
            if (connectionString == null || string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Invalid Institution Code");
            }

            if (id != subjectCategory.ID)
            {
                return BadRequest();
            }

            try
            {
                var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(SubjectCategory));
                var serializedEntity = JsonConvert.SerializeObject(subjectCategory, serializerSettings);

                DatabaseHelper.ExecuteQuery(connectionString, $@"Update [dbo].[{_tableName}] set [SerializedEntity] = '{serializedEntity}' where ID = {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("An system error occured " + ex.Message);
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

            try
            {
                var tableExists = DatabaseHelper.ExecuteTableCheck(connectionString, _tableName);

                if (!tableExists)
                {
                    DatabaseHelper.CreateTable(connectionString, _tableName);
                }

                var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(SubjectCategory));
                var serializedEntity = JsonConvert.SerializeObject(subjectCategory, serializerSettings);

                subjectCategory.ID = DatabaseHelper.ExecuteInsertQuery(connectionString, $@"INSERT INTO [dbo].[{_tableName}]
                                                                    ([SerializedEntity]) output INSERTED.ID VALUES ('{serializedEntity}')");
            }
            catch (Exception ex)
            {
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

            var subjectCategoryData = DatabaseHelper.ExecuteSelectFindQuery(connectionString, $"Select * from [dbo].[{_tableName}] where ID={id}");

            var student = SerializationHelper.UnboxEntity<SubjectCategory>(subjectCategoryData);

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