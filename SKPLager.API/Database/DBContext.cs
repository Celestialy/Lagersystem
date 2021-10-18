using Microsoft.EntityFrameworkCore;
using SKPLager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ConsumptionItem> ConsumptionItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<LoanItem> LoanItems { get; set; }

    }
}
