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

namespace SchoolManager.Controllers
{
    public class SubjectsController : Controller
    {
        private static HttpClient _client = GetHttpClient();

        private static readonly string _institutionCode = "2";
        private static readonly string _apiControllerName = "Subjects";

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

            IList<IDictionary<string, object>> subjects = null;
            IList<IDictionary<string, object>> modifiedSubjects = new List<IDictionary<string, object>>();
            IList<Subject> allSubjects = new List<Subject>();

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subjects = await response.Content.ReadAsAsync<List<IDictionary<string, object>>>();
            }

            foreach(var actualSubject in subjects)
            {
                var subjectToBeModified = actualSubject.ToDictionary(entry => entry.Key, 
                                                                    entry => entry.Value);
                                                                                                    
                foreach (KeyValuePair<string, object> keyValuePair in actualSubject)
                {
                    if(keyValuePair.Key == "id")
                    {
                        subjectToBeModified["id"] = Convert.ToInt32(actualSubject["id"]);
                    }

                    if(keyValuePair.Key == "category")
                    {
                        // Get Category from the object, 
                        var subjectCategoryId = Convert.ToString(keyValuePair.Value);

                        apiUrl = $"SubjectCategories/{_institutionCode}/{subjectCategoryId}";

                        SubjectCategory subjectCategory = null;

                        response = await _client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
                        }

                        if (subjectCategory != null)
                        {
                            // set the new object                            
                            subjectToBeModified["category"] = subjectCategory;
                        }
                        else
                        {
                            subjectToBeModified["category"] = new SubjectCategory { Name = "NIL" };
                        }
                    }
                }

                allSubjects.Add(subjectToBeModified.ToObject<Subject>());
            }

            return View(allSubjects);
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_apiControllerName}/{_institutionCode}/{id}";

            Subject subject = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subject = await response.Content.ReadAsAsync<Subject>();
            }

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Subject subject)
        {
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

            Subject subject = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subject = await response.Content.ReadAsAsync<Subject>();
            }

            if (subject == null)
            {
                return NotFound();
            }
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

            Subject subject = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subject = await response.Content.ReadAsAsync<Subject>();
            }

            if (subject == null)
            {
                return NotFound();
            }

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
    }
}
