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
    public class CustomerTaxCategoriesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: CustomerTaxCategories
        public ActionResult Index()
        {
            return View(db.CustomerTaxCategories.ToList());
        }



        // GET: CustomerTaxCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerTaxCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustCatName,CustCatDescription")] CustomerTaxCategory customerTaxCategory)
        {
            if (ModelState.IsValid)
            {
                customerTaxCategory.ModifiedBy = User.Identity.GetUserId<int>();
                customerTaxCategory.DateCreated = DateTime.Now;
                customerTaxCategory.IsDeleted = false;
                customerTaxCategory.IsActive = true;
                db.CustomerTaxCategories.Add(customerTaxCategory);
                db.SaveChanges();
                Session["success"] = "Customer Tax Category created";
                return RedirectToAction("Index");
            }

            return View(customerTaxCategory);
        }

        // GET: CustomerTaxCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTaxCategory customerTaxCategory = db.CustomerTaxCategories.Find(id);
            if (customerTaxCategory == null)
            {
                return HttpNotFound();
            }
            return View(customerTaxCategory);
        }

        // POST: CustomerTaxCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustCatName,CustCatDescription,IsActive")] CustomerTaxCategory customerTaxCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerTaxCategory).State = EntityState.Modified;
                db.SaveChanges();
                Session["success"] = "Category details updated";
                return RedirectToAction("Index");
            }
            return View(customerTaxCategory);
        }

        // GET: CustomerTaxCategories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerTaxCategory customerTaxCategory = db.CustomerTaxCategories.Find(id);
            if (customerTaxCategory == null)
            {
                return HttpNotFound();
            }
            return View(customerTaxCategory);
        }

        // POST: CustomerTaxCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CustomerTaxCategory customerTaxCategory = db.CustomerTaxCategories.Find(id);
//            customerTaxCategory.IsDeleted = true;
            db.CustomerTaxCategories.Remove(customerTaxCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetCategoryDetails(FormCollection coll)
        {
            var id = Int64.Parse(coll["id"]);
            return Json(db.CustomerTaxCategories.Find(id));
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
