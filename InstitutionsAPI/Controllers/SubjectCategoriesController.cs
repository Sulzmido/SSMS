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
                var subjectCategoriesData = DatabaseHelper.ExecuteSelectQuery(connectionString, "Select * from [dbo].[SubjectCategories]");
                return subjectCategoriesData.Select(s => s.ToObject<SubjectCategory>());
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
                subjectCategoryData = DatabaseHelper.ExecuteSelectFindQuery(connectionString, $"Select * from [dbo].[SubjectCategories] where ID={id}");

                if(subjectCategoryData.Count < 1)
                {
                    return NotFound();
                }

                var subjectCategory = subjectCategoryData.ToObject<SubjectCategory>();

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
                DatabaseHelper.ExecuteQuery(connectionString, $@"Update [dbo].[subjectCategories] set [Name] = '{subjectCategory.Name}' where ID = {id}");
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
                string tableName = "SubjectCategories";

                var tableExists = DatabaseHelper.ExecuteTableCheck(connectionString, tableName);

                if (!tableExists)
                {
                    DatabaseHelper.CreateTable(connectionString, tableName);
                }

                var serializerSettings = SerializationHelper.GetIgnnoreIDSerializerSetting(typeof(SubjectCategory));
                var serializedEntity = JsonConvert.SerializeObject(subjectCategory, serializerSettings);

                subjectCategory.ID = DatabaseHelper.ExecuteInsertQuery(connectionString, $@"INSERT INTO [dbo].[SubjectCategories]
                                                                    ([SerializedEntity]) output INSERTED.ID VALUES ('{serializedEntity}')");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return CreatedAtAction("GetsubjectCategory", new { id = subjectCategory.ID }, subjectCategory);
        }
    }
}