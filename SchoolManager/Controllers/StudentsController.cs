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
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace SchoolManager.Controllers
{
    public class StudentsController : Controller
    {
        private static HttpClient _client = GetHttpClient();

        private static readonly string _apiControllerName = "Students";

        private readonly IConfiguration _config;
        private readonly string _institutionCode;
        private readonly string _baseUrl;

        public StudentsController(IConfiguration configuration)
        {
            _config = configuration;

            _institutionCode = _config.GetValue<string>("InstitutionCode");
            _baseUrl = _config.GetValue<string>("InstitutionsAPIUrl");
        }

        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}";

            List<Student> students = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                students = JsonConvert.DeserializeObject<List<Student>>(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}/{id}";

            Student subject = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                subject = await response.Content.ReadAsAsync<Student>();
            }

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Sex,Age,Religion,DateOfBirth,Parent")] Student model)
        {
            var student = new
            {
                model.Name,
                Sex = model.Sex.ToString(),
                model.Age,
                model.Religion,
                model.DateOfBirth,

                Parent = new
                {
                    model.Parent.NameOfParents,
                    model.Parent.ParentDOB,
                    model.Parent.ParentOccupation,
                    MarriageStatus = model.Parent.MarriageStatus.ToString()
                }
            };

            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}";

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(apiUrl, student);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}/{id}";

            Student student = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                student = await response.Content.ReadAsAsync<Student>();
            }

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Sex,Age,Religion,DateOfBirth,Parent")] Student model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            var student = new
            {
                model.ID,
                model.Name,
                Sex = model.Sex.ToString(),
                model.Age,
                model.Religion,
                model.DateOfBirth,

                Parent = new
                {
                    model.Parent.NameOfParents,
                    model.Parent.ParentDOB,
                    model.Parent.ParentOccupation,
                    MarriageStatus = model.Parent.MarriageStatus.ToString()
                }
            };

            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}/{id}";

            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await _client.PutAsJsonAsync(apiUrl, student);
                    response.EnsureSuccessStatusCode();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StudentExists(model.ID))
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
            return View(model);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}/{id}";

            Student student = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                student = await response.Content.ReadAsAsync<Student>();
            }

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}/{id}";

            HttpResponseMessage response = await _client.DeleteAsync(apiUrl);

            var statusCode = response.StatusCode;

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StudentExists(int id)
        {
            string apiUrl = $"{_baseUrl}/{_apiControllerName}/{_institutionCode}/{id}";

            Student student = null;

            HttpResponseMessage response = await _client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                student = await response.Content.ReadAsAsync<Student>();
                return student != null;
            }
            else
            {
                return false;
            }
        }
    }
}
