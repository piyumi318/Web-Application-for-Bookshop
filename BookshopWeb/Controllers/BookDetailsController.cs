using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookshopWeb.Models;

namespace BookshopWeb.Controllers
{
    public class BookDetailsController : Controller
    {
        private BookShopDatabaseEntities2 db = new BookShopDatabaseEntities2();

        // GET: BookDetails
        public ActionResult Index()
        {
            if (Session["AdminId"] == null) { return RedirectToAction("Index", "Home"); }
            var bookDetails = db.BookDetails.Include(b => b.Category);
            return View(bookDetails.ToList());
        }

        // GET: BookDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookDetail bookDetail = db.BookDetails.Find(id);
            if (bookDetail == null)
            {
                return HttpNotFound();
            }
            return View(bookDetail);
        }

        // GET: BookDetails/Create
        public ActionResult Create()


        {
            if (Session["AdminId"] == null) { return RedirectToAction("Index", "Home"); }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: BookDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookID,BookName,Author,Description,Image,Price,Quantity,CategoryID,ImageFile")] BookDetail bookDetail)
        {
           string path = Path.GetFileNameWithoutExtension(bookDetail.ImageFile.FileName);
           string extension = Path.GetExtension(bookDetail.ImageFile.FileName);
           path = path + extension;
           bookDetail.Image = path;
            path = Path.Combine(Server.MapPath("~/Book/"), path);
            bookDetail.ImageFile.SaveAs(path);


            if (ModelState.IsValid)
            {


                db.BookDetails.Add(bookDetail);
                Session["Message"] = "Book Is added  suceessfully";
                db.SaveChanges();


                ModelState.Clear();
                return RedirectToAction("Create");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", bookDetail.CategoryID);
            return View(bookDetail);
        }
           

        // GET: BookDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookDetail bookDetail = db.BookDetails.Find(id);
            if (bookDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", bookDetail.CategoryID);
            return View(bookDetail);
        }

        // POST: BookDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,BookName,Author,Description,Image,Price,Quantity,CategoryID")] BookDetail bookDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", bookDetail.CategoryID);
            return View(bookDetail);
        }

        // GET: BookDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookDetail bookDetail = db.BookDetails.Find(id);
            if (bookDetail == null)
            {
                return HttpNotFound();
            }
            return View(bookDetail);
        }

        // POST: BookDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookDetail bookDetail = db.BookDetails.Find(id);
            db.BookDetails.Remove(bookDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
