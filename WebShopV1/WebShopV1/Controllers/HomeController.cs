﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private ApplicationDbContext di = new ApplicationDbContext();
        private WebShopDbContext db = new WebShopDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewItem(string name, decimal cost, string description, int quantity, Product product)
        {
            if (ModelState.IsValid)
            {

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return Content("success");
        }

        public JsonResult AjaxThatReturnsJson()
        {
            var storeInfo = db.Products.ToList();

            return Json(storeInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AjaxThatReturnsJsonItem(int? Id)
        {
            object itemInfo = null;

            itemInfo = db.Products.Single(p => p.Id == Id);

            if (itemInfo == null)
            {
                itemInfo = new { Id = 0, name = "Not", cost = "Found" };
            }

            return Json(itemInfo, JsonRequestBehavior.AllowGet);
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

        public ActionResult Edit([Bind(Include = "Id,name,cost,description,quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? Id)
        {
            Product product = db.Products.Find(Id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public JsonResult Buy()
        {
            var storeInfo = db.Products.ToList();
            var uderInfo = di.Users.ToList(); 

            string sessionToString = (string)Session["order"];
            var spliter = sessionToString.Split(',').ToList();
            int total = spliter.Count();
            decimal totalCost = 0;

            List<Product> cartList = new List<Product>();
            Order order = new Order();
            Customer customer = new Customer();

            foreach (var item in spliter)
            {
                var theProduct = storeInfo.Single(p => p.Id.ToString() == item);
                //cartList.Add(storeInfo.Single(p => p.Id.ToString() == item));
                order.productList.Add(theProduct);
                totalCost += theProduct.cost;

            }
            order.orderDate = DateTime.Now;
            order.totalCost = Convert.ToInt32(totalCost);
            order.totalCount = total;

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = di.Users.FirstOrDefault(x => x.Id == currentUserId);

            customer.firstName = currentUser.UserName;
            customer.lastName = currentUser.UserName;
            customer.email = currentUser.Email;
            customer.adress = "comig soon";
            customer.orderHistory.Add(order);

            //return Json(order.productList, JsonRequestBehavior.AllowGet);

            var result = new { order = order.productList , cus = customer };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}

//string getQuantity = "";
//string getCost = "";
//getQuantity = total.ToString(CultureInfo.InvariantCulture);
//getCost = totalCost.ToString(CultureInfo.InvariantCulture);
//return Json(new { getQuantity, getCost }, JsonRequestBehavior.AllowGet);