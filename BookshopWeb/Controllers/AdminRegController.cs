using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookshopWeb.Controllers
{
    public class AdminRegController : Controller
    {
        // GET: AdminReg
        public ActionResult Index()
        {
            if (Session["AdminId"] == null) { return RedirectToAction("Index", "Home"); }
            return View();
        }
    }
}