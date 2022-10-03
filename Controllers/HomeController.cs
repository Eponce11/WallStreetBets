using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WallStreetBets.Models;

namespace WallStreetBets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _db;

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
        public IActionResult Dashboard()
        {
            if (!isLoggedIn) { return RedirectToAction("Index"); }


            List<Dictionary<string, string>> watchStockList = new List<Dictionary<string, string>>();
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121"},
                    {"close", "121.19"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                watchStockList.Add(new Dictionary<string, string>(){
                    {"ticker", "APPL"},
                    {"open", "121.19"},
                    {"close", "121"},
                });
                
                

            return View("Dashboard", watchStockList);
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
