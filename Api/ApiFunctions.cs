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

        public async Task<List<Stock>> GetStocks(List<String> stockTickers)
        {

            List<Stock> stockList = new List<Stock>();

            using (HttpClient httpClient = new HttpClient())
            {
                foreach (String ticker in stockTickers)
                {
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