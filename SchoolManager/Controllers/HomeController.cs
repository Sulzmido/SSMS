using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using InstitutionsAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManager.Models;

namespace SchoolManager.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient _client = GetHttpClient();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Institution()
        {
            string apiUrl = $"Institutions/7";

            Institution institution = null;

            HttpResponseMessage response = _client.GetAsync(apiUrl).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                institution = response.Content.ReadAsAsync<Institution>().GetAwaiter().GetResult();
            }

            if (institution == null)
            {
                return NotFound();
            }

            return View(institution);
        }

        private static HttpClient GetHttpClient()
        {
            var baseUrl = "http://localhost/InstitutionsAPI/api/";

            var client = new HttpClient();

            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
