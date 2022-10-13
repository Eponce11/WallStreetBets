using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WallStreetBets.Models
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
        public string Logo { get; set; }
        public string Low { get; set; }
        public string High { get; set; }
    }
}