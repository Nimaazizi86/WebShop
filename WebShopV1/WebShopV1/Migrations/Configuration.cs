namespace WebShopV1.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebShop1.Models.WebShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebShop1.Models.WebShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Products.AddOrUpdate(
              p => p.name,
              new Product {name = "art1", cost=2200 , description = "cole and cabanas A3", quantity = 3 },
              new Product { name = "art2", cost = 2200, description = "pencil A5", quantity = 1 },
              new Product { name = "art3", cost = 2200, description = "hash A3", quantity = 2 },
              new Product { name = "art4", cost = 2200, description = "Watercolor 50*70", quantity = 5 }
              );
            //
        }
    }
}
