namespace WebShopV1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class AdminConfiguration : DbMigrationsConfiguration<WebShopV1.Models.ApplicationDbContext>
    {
        public AdminConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebShopV1.Models.ApplicationDbContext context)
        {
            // create a variable to store the role in it
            var roleStore = new RoleStore<IdentityRole>(context);
            // create a variable to store the managing data for the role in it
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            // create a new role and and save it in rolerSuperAdminesult and call it SuperAdmin
            var rolerSuperAdminesult = roleManager.Create(new IdentityRole("SuperAdmin"));

            // create new variable to store the information of the user
            var userStore = new UserStore<ApplicationUser>(context);

            // create new variable to store the managed information of the user
            var userManager = new UserManager<ApplicationUser>(userStore);

            // define th information of the user and its login password
            var result = userManager.Create(user: new ApplicationUser() { UserName = "Steve@Steve.com", Email = "Steve@Steve.com" }, password: "Password@123");

            // find the user by the name of "Steve@Steve.com"
            var user = userManager.FindByName("Steve@Steve.com");

            // Set the role of SuperAdmin to "Steve@Steve.com"
            userManager.AddToRole(user.Id, "SuperAdmin");
        }
    }
}
