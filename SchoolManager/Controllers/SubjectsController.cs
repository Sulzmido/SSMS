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

            List<Subject> subjects = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subjects = await response.Content.ReadAsAsync<List<Subject>>();
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
