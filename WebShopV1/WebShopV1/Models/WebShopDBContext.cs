using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebShopV1.Models;

namespace WebShop1.Models
{
    public class WebShopDbContext : DbContext
    {
        public WebShopDbContext() : base("WebShop1") { }

        public static WebShopDbContext Create()
        {
            return new WebShopDbContext();
        }

        public DbSet<Customer> Persons { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> DetailInfos { get; set; }

        public DbSet<ShoppingCart> OrderHistorys { get; set; }
    }
}