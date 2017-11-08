using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class TaxCategoriesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: TaxCategories
        public ActionResult Index()
        {
            return View(db.TaxCategories.Where(x => x.IsDeleted != true).ToList());
        }

        // GET: TaxCategories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxCategory taxCategory = db.TaxCategories.Find(id);
            if (taxCategory == null)
            {
                return HttpNotFound();
            }
            return View(taxCategory);
        }

        // GET: TaxCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaxCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryName,CategoryDescription,CreatedAt,CreatedBy,IsDeleted,IsActive")] TaxCategory taxCategory)
        {
            if (ModelState.IsValid)
            {
                taxCategory.CreatedAt= DateTime.Now;
                taxCategory.CreatedBy = User.Identity.GetUserId<int>();
                taxCategory.IsDeleted = false;
                taxCategory.IsActive = true;
                db.TaxCategories.Add(taxCategory);
                db.SaveChanges();
                Session["success"] = "Tax Category created";
                return RedirectToAction("Index");
            }

            return View(taxCategory);
        }

        // GET: TaxCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxCategory taxCategory = db.TaxCategories.Find(id);
            if (taxCategory == null)
            {
                return HttpNotFound();
            }
            return View(taxCategory);
        }

        public JsonResult GetCategoryDetails(FormCollection coll)
        {
            var id = Int64.Parse(coll["id"]);
            return Json(db.TaxCategories.Find(id));
        }

        // POST: TaxCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryName,CategoryDescription")] TaxCategory taxCategory)
        {
            if (ModelState.IsValid)
            {
                taxCategory.IsActive = true;
                taxCategory.CreatedAt = DateTime.Now;
                taxCategory.IsDeleted = false;
                db.Entry(taxCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taxCategory);
        }

    
        // POST: TaxCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TaxCategory taxCategory = db.TaxCategories.Find(id);
//            db.TaxCategories.Remove(taxCategory);
            taxCategory.IsDeleted = true;
            taxCategory.IsActive = false;
            db.SaveChanges();
            Session["success"] = "Category deleted";

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
