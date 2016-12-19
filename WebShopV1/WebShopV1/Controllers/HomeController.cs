using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop1.Models;
using WebShopV1.Models;

namespace WebShopV1.Controllers
{
    public class HomeController : Controller
    {
        private WebShopDbContext db = new WebShopDbContext();
        public ActionResult Index()
        {
            return View();
        }
               
        public JsonResult AjaxThatReturnsJson()
        {
            var storeInfo = db.Products.ToList();

            return Json(storeInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AjaxThatReturnsJsonTotal(int? Id)
        {

                if (Session["order"] == null)
                {
                    Session["order"] += Id.ToString();
                }
                else
                {
                    Session["order"] += "," + Id.ToString();
                }

            var myInfo1 = db.Products.ToList();
            Product myInfo = null;

            string sessionToString = (string)Session["order"];
            var spliter = sessionToString.Split(',').ToList();
            string total = spliter.Count().ToString();
            decimal totalCost = 0;

            foreach (var item in spliter)
            {
                myInfo = myInfo1.Single(p => p.Id.ToString() == item);
                totalCost += myInfo.cost;
            }

            string getQuantity = "";
            string getCost = "";
            getQuantity = total.ToString(CultureInfo.InvariantCulture);
            getCost = totalCost.ToString(CultureInfo.InvariantCulture);
            return Json(new { getQuantity, getCost }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AjaxThatReturnsJsonCartList()
        {
            var storeInfo = db.Products.ToList();

            string sessionToString = (string)Session["order"];
            var spliter = sessionToString.Split(',').ToList();
            List<Product> cartList = new List<Product>();

            foreach (var item in spliter)
            {
                cartList.Add(storeInfo.Single(p => p.Id.ToString() == item));
            }

            return Json(cartList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AjaxThatReturnsJsonRemove(int? Id)
        {
            string testId = Id.ToString();
            if (Session["order"] == null)
            {
                return Json("error");
            }
            else
            {
                var myInfo1 = db.Products.ToList();
                Product myInfo = null;

                string sessionToString = (string)Session["order"];
                var spliter = sessionToString.Split(',').ToList();

                spliter.RemoveAll(u => u.Contains(testId));

                string total = spliter.Count().ToString();
                decimal totalCost = 0;

                foreach (var item in spliter)
                {
                    myInfo = myInfo1.Single(p => p.Id.ToString() == item);
                    totalCost += myInfo.cost;
                }
                Session["order"] = string.Join(",", spliter.ToArray());

                string getQuantity = "";
                string getCost = "";
                getQuantity = total.ToString(CultureInfo.InvariantCulture);
                getCost = totalCost.ToString(CultureInfo.InvariantCulture);
                return Json(new { getQuantity, getCost }, JsonRequestBehavior.AllowGet);

            }
        }


    }
}