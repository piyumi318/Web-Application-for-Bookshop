using BookshopWeb.Common;
using BookshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookshopWeb.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                EncryptPassword encryptPassword = new EncryptPassword();
                using (BookShopDatabaseEntities2 db = new BookShopDatabaseEntities2())
                {
                    if (userDetail.Username == null || userDetail.password == null) { ModelState.AddModelError("", "Enter Both username and Password"); }
                    else
                    {
                        string pass = encryptPassword.Encrypt(userDetail.password);
                        var obj1 = db.UserDetails.Where(a => a.Username.Equals(userDetail.Username) && a.password.Equals(pass)).FirstOrDefault();
                        if (obj1 != null)
                        {
                            Session["userId"] = obj1.UserID.ToString();
                            Session["Username"] = obj1.Username.ToString();
                            return RedirectToAction("Index", "Home");
                        }
                        else { ModelState.AddModelError("", "The username or Password is incorrect"); }
                    }
                }

            }

            return View(userDetail);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session["Message"] = "sucessfylly Loggout";
            return RedirectToAction("Index", "UserLogin");


        }
    }
}