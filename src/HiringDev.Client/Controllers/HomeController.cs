using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HiringDev.Client.Models;
using HiringDev.Client.Services;

namespace HiringDev.Client.Controllers
{
    public class HomeController : Controller
    {
        IServiceHttpClient _httpClient;

        public HomeController(IServiceHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Search(SearchModel model)
        {
            var results = await _httpClient.GetResultsByTerm(model.SearchTerm);
            return PartialView(results);
        }

    }
}
