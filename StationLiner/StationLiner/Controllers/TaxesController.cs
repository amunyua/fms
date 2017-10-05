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
    public class TaxesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Taxes
        public ActionResult Index()
        {
            var taxes = db.Taxes.Where(x => x.IsDeleted != true).OrderByDescending(x => x.Id).Include(t => t.CustomerTaxCategory).Include(t => t.TaxCategory).Include(t => t.TaxRate);
            ViewBag.categories = db.TaxCategories.Where(x=>x.IsDeleted != true).ToList();
            ViewBag.taxRates = db.TaxRates.Where(x => x.IsDeleted != true).ToList();
            ViewBag.customercats = db.CustomerTaxCategories.Where(x => x.IsDeleted != true).ToList();

            return View(taxes.ToList());
        }

      

        // GET: Taxes/Create
        public ActionResult Create()
        {
            ViewBag.CustCatId = new SelectList(db.CustomerTaxCategories, "Id", "CustCatName");
            ViewBag.CategoryId = new SelectList(db.TaxCategories, "Id", "CategoryName");
            ViewBag.TaxRateId = new SelectList(db.TaxRates, "Id", "TaxRateName");
            return View();
        }

        // POST: Taxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaxName,CategoryId,CategoryDescription,TaxRateId,CustCatId")] Tax tax)
        {
            if (ModelState.IsValid)
            {
                tax.CreatedBy = User.Identity.GetUserId<int>();
//                tax.CreatedAt = DateTime.Now;
                tax.IsDeleted = false;
                tax.IsActive = true;
                db.Taxes.Add(tax);
                db.SaveChanges();
                Session["success"] = "Tax Created";
                return RedirectToAction("Index");
            }

            ViewBag.CustCatId = new SelectList(db.CustomerTaxCategories, "Id", "CustCatName", tax.CustCatId);
            ViewBag.CategoryId = new SelectList(db.TaxCategories, "Id", "CategoryName", tax.CategoryId);
            ViewBag.TaxRateId = new SelectList(db.TaxRates, "Id", "TaxRateName", tax.TaxRateId);
            return View(tax);
        }

        // GET: Taxes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustCatId = new SelectList(db.CustomerTaxCategories, "Id", "CustCatName", tax.CustCatId);
            ViewBag.CategoryId = new SelectList(db.TaxCategories, "Id", "CategoryName", tax.CategoryId);
            ViewBag.TaxRateId = new SelectList(db.TaxRates, "Id", "TaxRateName", tax.TaxRateId);
            return View(tax);
        }

        public JsonResult GetTaxListEditDetails(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);
            return Json(db.Taxes.Find(id));
        }

        // POST: Taxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Tax tax)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tax).State = EntityState.Modified;
                tax.IsDeleted = false;
                tax.IsActive = true;
                db.SaveChanges();
                Session["success"] = "Details Updated";
                return RedirectToAction("Index");
            }
            ViewBag.CustCatId = new SelectList(db.CustomerTaxCategories, "Id", "CustCatName", tax.CustCatId);
            ViewBag.CategoryId = new SelectList(db.TaxCategories, "Id", "CategoryName", tax.CategoryId);
            ViewBag.TaxRateId = new SelectList(db.TaxRates, "Id", "TaxRateName", tax.TaxRateId);
            return View(tax);
        }

        // GET: Taxes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tax tax = db.Taxes.Find(id);
            if (tax == null)
            {
                return HttpNotFound();
            }
            return View(tax);
        }

        // POST: Taxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Tax tax = db.Taxes.Find(id);
            tax.IsDeleted = true;
            /*
            tax.IsActive = false;*/
//            db.Taxes.Remove(tax);
            db.SaveChanges();
            Session["success"] = "Tax has been deleted";
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
