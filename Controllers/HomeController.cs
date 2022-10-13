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
using System.Text.Json;

namespace WallStreetBets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext _db;

        private ApiFunctions apiFunctions = new ApiFunctions();
        private List<String> commonTickers = new List<string>();
        // {"AAPL", "ATVI", "MSFT", "GT", "PG"};

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _db = context;
        }

        private int? uid 
        { 
            get
            { 
                return HttpContext.Session.GetInt32("UserId");
            } 
        }
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
            
            List<String> listStockTickers = new List<string>();
            List<ListStock> allListStock= _db.ListStocks.Where(s => s.UserId == uid).ToList();
            foreach (ListStock s in allListStock)
            {
                listStockTickers.Add(s.Ticker);
            }
            List<Stock> watchStockList = await apiFunctions.GetStocks(listStockTickers);
            ViewBag.watchStockList = watchStockList;


            List<RedditStock> redditStocks = await apiFunctions.GetRedditStocks();
            IEnumerable<RedditStock> redditStockList = redditStocks.Take(6);
            ViewBag.redditStockList = redditStockList;
            

            List<Stock> commonStockList = await apiFunctions.GetStocks(commonTickers);
            
            return View("Dashboard", commonStockList);
        }


        



        [HttpPost("/watchlistaddremove")]
        public JsonResult WatchListAddRemove(String ticker)
        {
            ListStock currentStock = _db.ListStocks
                .Where(s => s.UserId == uid)
                .FirstOrDefault(s => s.Ticker == ticker);

            if (currentStock == null)
            {
                // add it
                ListStock newStock = new ListStock();
                newStock.Ticker = ticker;
                newStock.UserId = (int)uid;
                _db.ListStocks.Add(newStock);
            }
            else
            {
                // remove it
                _db.ListStocks.Remove(currentStock);
            }

            _db.SaveChanges();

            return Json("Result");
        }



        [HttpPost("/searchstock")]
        public async Task<JsonResult> SearchStock(String ticker)
        {
            List<Stock> currentStock = await apiFunctions.GetStocks(new List<string>(){ticker});
            Stock searchedStock = currentStock[0];

            if (searchedStock.Symbol == null)
            {
                return Json("Invalid Ticker");
            }

            // string jsonString = JsonSerializer.Serialize(searchedStock);
            return Json(searchedStock);
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
