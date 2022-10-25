using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WallStreetBets.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace WallStreetBets.Api
{
    public class ApiFunctions
    {
        // gets all the stocks in the parameter list and returns the List of stocks
        public async Task<List<Stock>> GetStocks(List<String> stockTickers)
        {
            String date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            List<Stock> stockList = new List<Stock>();

            using (HttpClient httpClient = new HttpClient())
            {
                // calls api for each ticker
                foreach (String ticker in stockTickers)
                {
                    // gets the api response and stores the result in the Stock model and adds it to the list
                    using (var response = await httpClient.GetAsync($""))
                    {
                        String apiResponse = await response.Content.ReadAsStringAsync();
                        Stock currentStock = JsonConvert.DeserializeObject<Stock>(apiResponse);
                        stockList.Add(currentStock);
                    }
                }
            }

            return stockList;
        }
        
        // gets the wallstreetbets api and return a list of the reddit stock info
        public async Task<List<RedditStock>> GetRedditStocks()
        {
            List<RedditStock> redditStockList = new List<RedditStock>();

            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://tradestie.com/api/v1/apps/reddit"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    redditStockList = JsonConvert.DeserializeObject<List<RedditStock>>(apiResponse);
                }
            }
            return redditStockList;
        }
        


        
    }
}