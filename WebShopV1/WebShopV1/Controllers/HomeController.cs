using Microsoft.AspNet.Identity;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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

        public ActionResult NewItem(string name, decimal cost, string description, Product product)
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
            var userInfo = di.Users.ToList(); 

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
            ApplicationUser currentUser = userInfo.FirstOrDefault(x => x.Id == currentUserId);

            order.customerId = currentUser.Id;

            //var theUser = uderInfo.Single(p => p.Id.ToString() == currentUserId);

            customer.firstName = currentUser.UserName;
            customer.lastName = currentUser.UserName;
            customer.email = currentUser.Email;
            customer.adress = "comig soon";
            customer.orderHistory.Add(order);
            db.DetailInfos.Add(order);
            db.Persons.Add(customer);
            db.SaveChanges();
           
            var result = new { order = order.productList , cus = customer };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveFile()
        {
            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            strPath += "\\";

            var storeInfo = db.Products.ToList();
            var userInfo = di.Users.ToList();

            string sessionToString = (string)Session["order"];
            var spliter = sessionToString.Split(',').ToList();
            int total = spliter.Count();
            decimal totalCost = 0;

            //List<Product> cartList = new List<Product>();
            Order order = new Order();
            //Customer customer = new Customer();





            using (StreamWriter writer = new StreamWriter(strPath + "receipt.txt", true))
            {
                writer.WriteLine("Thanks for shopping!");
                foreach (var item in spliter)
                {
                    var theProduct = storeInfo.Single(p => p.Id.ToString() == item);
                    //cartList.Add(storeInfo.Single(p => p.Id.ToString() == item));
                    order.productList.Add(theProduct);
                    totalCost += theProduct.cost;
                    string printItem = "product name: " + theProduct.name + ", Product cost:" + theProduct.cost ;
                    writer.WriteLine(printItem);
                }

                string noi = "Number of items: " + total.ToString();
                string tC = "total cost: " + totalCost.ToString();
                writer.WriteLine(noi);
                writer.WriteLine(tC);
            }
            return Json("File Saved", JsonRequestBehavior.AllowGet);
        }


        //public JsonResult PdfSharpConvert()
        //{
        //    // Create a new PDF document
        //    PdfDocument document = new PdfDocument();

        //    // Create an empty page
        //    PdfPage page = document.AddPage();

        //    // Get an XGraphics object for drawing
        //    XGraphics gfx = XGraphics.FromPdfPage(page);

        //    // Create a font
        //    XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

        //    // Draw the text
        //    gfx.DrawString("Hello, World!", font, XBrushes.Black,
        //      new XRect(0, 0, page.Width, page.Height),
        //      XStringFormat.Center);

        //    // Save the document...
        //    string filename = "HelloWorld.pdf";
        //    document.Save(filename);
        //    // ...and start a viewer.
        //    Process.Start(filename);
        //    return Json("File Saved", JsonRequestBehavior.AllowGet);

        //}


    }




}
