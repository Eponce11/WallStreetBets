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
            // login poge
            if (isLoggedIn) { return RedirectToAction("Dashboard"); }

            return View("Index");
        }


        [HttpGet("/dashboard")]
        public async Task<IActionResult> Dashboard()
        {   
            // return to login page of id is not in session
            if (!isLoggedIn) { return RedirectToAction("Index"); }
            
            // get all the users saved tickers from db and run the stock api
            // store return in viewbag
            List<String> listStockTickers = new List<string>();
            List<ListStock> allListStock= _db.ListStocks.Where(s => s.UserId == uid).ToList();
            foreach (ListStock s in allListStock)
            {
                listStockTickers.Add(s.Ticker);
            }
            List<Stock> watchStockList = await apiFunctions.GetStocks(listStockTickers);
            ViewBag.watchStockList = watchStockList;

            // run the wallstreetbets api and store return in viewbag
            List<RedditStock> redditStocks = await apiFunctions.GetRedditStocks();
            IEnumerable<RedditStock> redditStockList = redditStocks.Take(6);
            ViewBag.redditStockList = redditStockList;
            
            // run the stock api with the default stock list and store return in viewbag
            List<Stock> commonStockList = await apiFunctions.GetStocks(commonTickers);
            ViewBag.commonStockList = commonStockList;
            
            return View("Dashboard");
        }


        


        // Route is called through JS
        // remove a ticker from users stocklist if it already exists or add if it doesnt
        [HttpPost("/watchlistaddremove")]
        public JsonResult WatchListAddRemove(String ticker)
        {
            // find the row in db that has the same User Id as the one stored in session and matches the passed in ticker
            ListStock currentStock = _db.ListStocks
                .Where(s => s.UserId == uid)
                .FirstOrDefault(s => s.Ticker == ticker);

            if (currentStock == null)
            {
                // if the row wasnt found add it to db
                ListStock newStock = new ListStock();
                newStock.Ticker = ticker;
                newStock.UserId = (int)uid;
                _db.ListStocks.Add(newStock);
            }
            else
            {
                // if the row was found remove it from db
                _db.ListStocks.Remove(currentStock);
            }

            // save the changes and return json
            _db.SaveChanges();

            return Json("Result");
        }


        // Route is called through JS
        // search for a stocks info
        [HttpPost("/searchstock")]
        public async Task<JsonResult> SearchStock(String ticker)
        {
            // call the stock api with the passed in ticker
            List<Stock> currentStock = await apiFunctions.GetStocks(new List<string>(){ticker});
            Stock searchedStock = currentStock[0];

            // if the api could not find a result, return JSON
            if (searchedStock.Symbol == null)
            {
                return Json("Invalid Ticker");
            }

            // if the api returns the stock info, pass it back as JSON
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
