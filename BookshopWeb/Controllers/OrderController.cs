using BookshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookshopWeb.Controllers
{
    public class OrderController : Controller
    {
        private BookShopDatabaseEntities2 db = new BookShopDatabaseEntities2();
        // GET: Order
        public ActionResult Index()
        {
            if (Session["AdminId"] == null) { return RedirectToAction("Index", "Home"); }
            return View(db.Orders.ToList());
        }
        public ActionResult Create()
        {
            if (Session["userId"] == null) { return RedirectToAction("Index", "Home"); }
            ViewBag.BookID = new SelectList(db.BookDetails, "BookID", "BookName");
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserID,BookID,Quantity,TotalPrice,OrderDate")] Order orderDetails)
        {
            if (ModelState.IsValid)
            {
               
                db.Orders.Add(orderDetails);
                db.SaveChanges();
                Session["Message"] = "order made suceessfully";
                return RedirectToAction("Index", "Home"); ;
               
            }

            ViewBag.BookID = new SelectList(db.BookDetails, "BookID", "BookName", orderDetails.BookID);
            ;
            return View();
        }
        public ActionResult ShowUserOwnOrders()
        {
            if (Session["userId"] == null) { return RedirectToAction("Index", "Home"); }
            {

                var obj1 = db.Orders.ToList().Where(a => a.UserID.Equals(Convert.ToInt32(Session["userId"])));

                return View(obj1);


            }
        }
    }
}