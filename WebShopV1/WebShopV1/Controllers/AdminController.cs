using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShopV1.Models;

namespace WebShopV1.Controllers
{
    public class AdminController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public string GetAdmin()
        {
            string role;
            if (User.IsInRole("SuperAdmin"))
            {
                role = "SuperAdmin";
            }
            else
            {
                role = "User";
            }
            return role;
        }

        public JsonResult AjaxThatReturnsAdmin()
        {
            string role;
            if (User.IsInRole("SuperAdmin"))
            {
                role = "SuperAdmin";
            }
            else
            {
                role = "User";
            }

            return Json(role, JsonRequestBehavior.AllowGet);
        }

        //public List<string> GetRoles(string UserName)
        //{
        //    List<string> roles = new List<string>();
        //    if (!string.IsNullOrWhiteSpace(UserName))
        //    {
        //        ApplicationUser user = db.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        //        var userStore = new UserStore<ApplicationUser>(db);
        //        var userManager = new UserManager<ApplicationUser>(userStore);

        //        System.Web.HttpContext.Current.User.Identity.Name.ToString();
        //        System.Web.HttpContext.Current.User.IsInRole("SuperAdmin");
        //        roles = account.UserManager.GetRoles(user.Id);
        //    }
        //    return roles;
        //}
    }
}