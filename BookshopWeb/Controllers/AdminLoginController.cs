using BookshopWeb.Common;
using BookshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookshopWeb.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: AdminLogin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AdminDetail adminDetails)
        {
            EncryptPassword encryptPassword = new EncryptPassword();
            if (ModelState.IsValid)
            {
                using (BookShopDatabaseEntities2 db = new BookShopDatabaseEntities2())
                    if (adminDetails.Username == null || adminDetails.password == null) { ModelState.AddModelError("", "Enter Both username and Password"); }
                    else
                    {
                        {
                            string pass = encryptPassword.Encrypt(adminDetails.password);
                            var obj1 = db.AdminDetails.Where(a => a.Username.Equals(adminDetails.Username) && a.password.Equals(pass)).FirstOrDefault();
                            if (obj1 != null)
                            {
                                Session["AdminId"] = obj1.AdminID.ToString();
                                Session["AdminName"] = obj1.AdminName.ToString();
                                return RedirectToAction("Index", "AdminReg");
                            }
                            else { ModelState.AddModelError("", "The username or Password is incorrect"); }
                        }
                    }

            }

            return View(adminDetails);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "UserLogin");


        }
    }
}