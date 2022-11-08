using BookshopWeb.Common;
using BookshopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookshopWeb.Controllers
{
    public class UserRegController : Controller
    {
        BookShopDatabaseEntities2 db = new BookShopDatabaseEntities2();
        // GET: UserReg
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserID,FirstName,LastName,No,Street,city,country,Gender,BDay,Username,password,Email")] UserDetail userDetai)
        {
            EncryptPassword encryptPassword = new EncryptPassword();
            if (ModelState.IsValid)
            {
                userDetai.password = encryptPassword.Encrypt(userDetai.password);
                db.UserDetails.Add(userDetai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetai);
        }
    }
}