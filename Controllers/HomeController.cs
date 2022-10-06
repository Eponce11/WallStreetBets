using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WallStreetBets.Models;
using WallStreetBets.Api;

namespace WallStreetBets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _db;

        private ApiFunctions apiFunctions = new ApiFunctions();
        private List<String> commonTickers = new List<string>()
        {"AAPL", "ATVI", "MSFT", "GT", "PG"};

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _db = context;
        }

        private int? uid { get{ return HttpContext.Session.GetInt32("UserId");} }
        private bool isLoggedIn { get{ return uid != null;} }



        [HttpGet("")]
        public IActionResult Index()
        {
            if (isLoggedIn) { return RedirectToAction("Dashboard"); }

            return View("Index");
        }


        [HttpGet("/dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            if (!isLoggedIn) { return RedirectToAction("Index"); }

            List<Stock> commonStockList = await apiFunctions.GetStocks(commonTickers);
            
            return View("Dashboard", commonStockList);
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
    }
}
