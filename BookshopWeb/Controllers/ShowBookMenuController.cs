using BookshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookshopWeb.Controllers
{
    public class ShowBookMenuController : Controller
    {
        private BookShopDatabaseEntities2 db;

        public ShowBookMenuController()
        {
            db = new BookShopDatabaseEntities2();

        }
        // GET: ShowBookMenu
        public ActionResult Index()
        {
            return View(db.BookDetails.ToList());
        }
        public ActionResult Viewcart()
        {
            if (Session["Cart"] == null)
            {
                Session["Message"] = "Cart is empty";
                return RedirectToAction("Index", "ShowBookMenu");
            }
            else
            {
                return View("AddToCart");

            }
        }
        public int IsExsistingId(int id)
        {
            List<Cart> cart = (List<Cart>)Session["Cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].BookID == id) return i;
            return -1;
        }

        [HttpPost]
        public string Getquantity(string Quantity)
        {
            // int quantity = Convert.ToInt32(Request["Quantity"]);
            // string Quantity = form["Quantity"];
            //   int quantity = Int32.Parse(Request["Quantity"]);
            // List<Cart> cart = new List<Cart>();
            // cart.Add(new Cart(quantity));
            return Quantity;
        }

        public ActionResult AddToCart(BookDetail book, int id)

        {
            if (Session["userId"] == null)
            {
                Session["Message"] = "Make sure to Login For order Books";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //  string[] quantity = fc.GetValues("Quantity");
                // int quantity= Convert.ToInt32(Request["Quantity"]);
                //int Total = int.Parse("200");
                //  int quantity=Convert.ToInt32(Quantity);
                if (Session["Cart"] == null)
                {
                    List<Cart> cart = new List<Cart>();
                    cart.Add(new Cart(db.BookDetails.Find(id), 1));
                    Session["Message"] = "Book is added to cart";
                    Session["Cart"] = cart;

                }
                else
                {

                    List<Cart> cart = (List<Cart>)Session["Cart"];
                    for (int i = 0; i < cart.Count; i++)
                    {//cart[i].Quantity = Convert.ToInt32(Request["Quantity"].ToString());
                        int index = IsExsistingId(id);
                        if (index == -1)
                        {
                            cart.Add(new Cart(db.BookDetails.Find(id), 1));
                            Session["Message"] = "Book is added to cart";
                        }
                        else
                        {
                            cart[index].Quantity++;
                            Session["Cart"] = cart;
                        }


                    }
                }
                return View(db.BookDetails.ToList());
            }
        }

        public ActionResult delete(int id)
        {
            int index = IsExsistingId(id);
            List<Cart> cart = (List<Cart>)Session["Cart"];
            cart.RemoveAt(index);
            Session["Cart"] = cart;

            return View("AddToCart");
        }

        public ActionResult Update(FormCollection form)
        {
            string[] quantity = form.GetValues("Quantity");
            List<Cart> cart = (List<Cart>)Session["Cart"];
            for (int i = 0; i < cart.Count; i++)

                cart[i].Quantity = Convert.ToInt32(quantity[i]);


            Session["Cart"] = cart;
            return View("AddToCart");
        }
    }

}
