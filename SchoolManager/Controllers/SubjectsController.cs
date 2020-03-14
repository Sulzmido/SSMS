using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InstitutionsAPI.Core.Models;
using SchoolManager.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using SchoolManager.Extensions;
using SchoolManager.ViewModels;

namespace SchoolManager.Controllers
{
    public class SubjectsController : Controller
    {
        private static HttpClient _client = GetHttpClient();

        private static readonly string _institutionCode = "2";
        private static readonly string _apiControllerName = "Subjects";

        private List<SelectListItem> _categorySelectListItems = GetSubjectCategories().GetAwaiter().GetResult(); 

        private static HttpClient GetHttpClient()
        {
            var baseUrl = "http://localhost/InstitutionsAPI/api/";

            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }        

        public SubjectsController()
        {
            
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            string apiUrl = $"{_apiControllerName}/{_institutionCode}";

            IList<IDictionary<string, object>> entities = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                entities = await response.Content.ReadAsAsync<List<IDictionary<string, object>>>();
            }

            var subjects = new List<Subject>();

            foreach(var entity in entities)
            {
                var subject = await IncludeSubEntities(entity);
                subjects.Add(subject);
            }
            
            return View(subjects);
        }
       
        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            IDictionary<string, object> entity = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<IDictionary<string, object>>();
            }

            if (entity == null)
            {
                return NotFound();
            }

            var subject = await IncludeSubEntities(entity);

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {          
            var model = new SubjectCreateViewModel { Categories = _categorySelectListItems };
            return View(model);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Category")] SubjectCreateViewModel model)
        {
            var subject = new { model.Name, model.Category };
            string apiUrl = $"{_apiControllerName}/{_institutionCode}";

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(apiUrl, subject);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            IDictionary<string, object> entity = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<IDictionary<string, object>>();
            }

            if (entity == null)
            {
                return NotFound();
            }

            var subject = await IncludeSubEntities(entity);

            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Subject subject)
        {
            if (id != subject.ID)
            {
                return NotFound();
            }

            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await _client.PutAsJsonAsync(apiUrl, subject);
                    response.EnsureSuccessStatusCode();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SubjectExists(subject.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            IDictionary<string, object> entity = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<IDictionary<string, object>>();
            }

            if (entity == null)
            {
                return NotFound();
            }

            var subject = await IncludeSubEntities(entity);

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            HttpResponseMessage response = await _client.DeleteAsync(apiUrl);

            var statusCode = response.StatusCode;

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubjectExists(int id)
        {
            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            Subject subject = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                subject = await response.Content.ReadAsAsync<Subject>();
                return subject != null;
            }
            else
            {
                return false;
            }
        }

        private static async Task<List<SelectListItem>> GetSubjectCategories()
        {
            string apiUrl = $"SubjectCategories/{_institutionCode}";

            List<SubjectCategory> categories = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<SubjectCategory>>();
            }

            return categories.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.Name
            }).ToList();
        }

        private async Task<Subject> IncludeSubEntities(IDictionary<string, object> subject)
        {
            subject["id"] = Convert.ToInt32(subject["id"]);

            var defaultCategory = new SubjectCategory { Name = "NIL" };

            if (subject["category"] != null)
            {
                var subjectCategoryId = Convert.ToString(subject["category"]);

                string apiUrl = $"SubjectCategories/{_institutionCode}/{subjectCategoryId}";

                SubjectCategory subjectCategory = null;

                HttpResponseMessage response = await _client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
                }

                subject["category"] = subjectCategory ?? defaultCategory;
            }
            else
            {
                subject["category"] = defaultCategory;
            }

            return subject.ToObject<Subject>();
        }
    }
}
