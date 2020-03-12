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

namespace SchoolManager.Controllers
{
    public class SubjectCategoriesController : Controller
    {
        private static HttpClient _client = new HttpClient() {};
        private static readonly string _baseUrl = "http://localhost/InstitutionsAPI/api";
        private static readonly string _institutionCode = "2";
        private static readonly string _apiControllerName = "SubjectCategories";

        public SubjectCategoriesController()
        {
            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: {_apiControllerName}
        public async Task<IActionResult> Index()
        {
            string apiUrl = $"/{_apiControllerName}/{_institutionCode}";

            List<SubjectCategory> categories = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<SubjectCategory>>();
            }
            
            return View(categories);
        }

        // GET: {_apiControllerName}/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"/{_apiControllerName}/{_institutionCode}/{id}";

            var subjectCategory = new SubjectCategory();

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
            }
            
            if (subjectCategory == null)
            {
                return NotFound();
            }

            return View(subjectCategory);
        }

        // GET: {_apiControllerName}/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: {_apiControllerName}/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] SubjectCategory subjectCategory)
        {
            string apiUrl = $"/{_apiControllerName}/{_institutionCode}";

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(apiUrl, subjectCategory);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(subjectCategory);
        }

        // GET: {_apiControllerName}/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"/{_apiControllerName}/{_institutionCode}/{id}";

            SubjectCategory subjectCategory = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
            }

            if (subjectCategory == null)
            {
                return NotFound();
            }

            return View(subjectCategory);
        }

        // POST: {_apiControllerName}/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] SubjectCategory subjectCategory)
        {
            if (id != subjectCategory.ID)
            {
                return NotFound();
            }

            string apiUrl = $"/{_apiControllerName}/{_institutionCode}/{id}";

            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await _client.PutAsJsonAsync(apiUrl, subjectCategory);
                    response.EnsureSuccessStatusCode();

                    // subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SubjectCategoryExists(subjectCategory.ID))
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
            return View(subjectCategory);
        }

        // GET: {_apiControllerName}/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"/{_apiControllerName}/{_institutionCode}/{id}";

            SubjectCategory subjectCategory = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
            }

            if (subjectCategory == null)
            {
                return NotFound();
            }

            return View(subjectCategory);
        }

        // POST: {_apiControllerName}/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string apiUrl = $"/{_apiControllerName}/{_institutionCode}/{id}";            

            HttpResponseMessage response = await _client.DeleteAsync(apiUrl);

            var statusCode = response.StatusCode;
            // Do something with status code ??

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubjectCategoryExists(int id)
        {
            string apiUrl = $"/{_apiControllerName}/{_institutionCode}/{id}";

            SubjectCategory subjectCategory = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                subjectCategory = await response.Content.ReadAsAsync<SubjectCategory>();
                return subjectCategory != null;
            }
            else
            {
                return false;
            }   
        }
    }
}
