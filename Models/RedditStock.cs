using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WallStreetBets.Models
{
    public class RedditStock
    {
        public string No_Of_Comments { get; set; }
        public string Sentiment { get; set; }
        public string Ticker { get; set; }
    }
}