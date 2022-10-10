using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WallStreetBets.Models
{
    public class ListStock
    {
        [Key]
        public int ListStockId { get; set; }

        [Required]
        public string Ticker { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public int UserId { get; set; }
        public User Creator { get; set; }



    }
}