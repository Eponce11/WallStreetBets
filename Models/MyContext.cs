using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WallStreetBets.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        // the dbsets get added as tables in the db
        public DbSet<User> Users { get; set; }
        public DbSet<ListStock> ListStocks { get; set; }

    }
}