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
        private static HttpClient _client = new HttpClient();

        public SubjectCategoriesController()
        {
            //_client.BaseAddress = new Uri("http://localhost:64195/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: SubjectCategories
        public async Task<IActionResult> Index()
        {
            string apiUrl = "http://localhost/InstitutionsAPI/api/SubjectCategories/2";

            List<SubjectCategory> categories = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<SubjectCategory>>();
            }
            
            return View(categories);
        }

        // GET: SubjectCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2/{id}";

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

        // GET: SubjectCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubjectCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] SubjectCategory subjectCategory)
        {
            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2";

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(apiUrl, subjectCategory);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(subjectCategory);
        }

        // GET: SubjectCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2/{id}";

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

        // POST: SubjectCategories/Edit/5
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

            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2/{id}";

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

        // GET: SubjectCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2/{id}";

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

        // POST: SubjectCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2/{id}";            

            HttpResponseMessage response = await _client.DeleteAsync(apiUrl);

            var statusCode = response.StatusCode;
            // Do something with status code ??

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubjectCategoryExists(int id)
        {
            string apiUrl = $"http://localhost/InstitutionsAPI/api/SubjectCategories/2/{id}";

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
