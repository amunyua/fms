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
    public class TaxRatesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: TaxRates
        public ActionResult Index()
        {
            return View(db.TaxRates.Where(x => x.IsDeleted != true).OrderByDescending(x => x.Id).ToList());
        }


        // GET: TaxRates/Create
        public ActionResult Create()
        {
            return View();
        }

        public JsonResult GetRateDetails(FormCollection coll)
        {
            var id = Int64.Parse(coll["id"]);
            return Json(db.TaxRates.Find(id));
        }

        // POST: TaxRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaxRateName,TaxRateValue,TaxRateDescription")] TaxRate taxRate)
        {
            if (ModelState.IsValid)
            {
                taxRate.CreatedBy = User.Identity.GetUserId<int>();
                taxRate.IsDeleted = false;
                taxRate.CreatedAt = DateTime.Now;
                taxRate.IsActive = true;
                db.TaxRates.Add(taxRate);
                db.SaveChanges();
                Session["success"] = "Tax Rate Created";
                return RedirectToAction("Index");
            }

            return View(taxRate);
        }

        // GET: TaxRates/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxRate taxRate = db.TaxRates.Find(id);
            if (taxRate == null)
            {
                return HttpNotFound();
            }
            return View(taxRate);
        }

        // POST: TaxRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaxRate taxRate)
        {
            if (ModelState.IsValid)
            {
                
                taxRate.IsActive = true;
                taxRate.CreatedAt = DateTime.Now;
                taxRate.IsDeleted = false;
                db.Entry(taxRate).State = EntityState.Modified;
                db.SaveChanges();
                Session["success"] = "Tax Rate Details updated";
                return RedirectToAction("Index");
            }
            return View(taxRate);
        }

        // GET: TaxRates/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaxRate taxRate = db.TaxRates.Find(id);
            if (taxRate == null)
            {
                return HttpNotFound();
            }
            return View(taxRate);
        }

        // POST: TaxRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TaxRate taxRate = db.TaxRates.Find(id);
            taxRate.IsDeleted = true;
//            taxRate.IsActive = false;
//            db.TaxRates.Remove(taxRate);
            db.SaveChanges();
            Session["success"] = "Tax Rate deleted";
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
