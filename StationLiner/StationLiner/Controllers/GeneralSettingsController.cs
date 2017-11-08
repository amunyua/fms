using StationLinerMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class GeneralSettingsController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: GeneralSettings
        public ActionResult Index()
        {
            return View(db.GeneralSettings.Where(x=>x.IsDeleted == false).ToList());
        }

    

        // POST: GeneralSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,VAT,Location,PinNumber,Contact1,Contact2")] GeneralSettings generalSettings)
        {
            if (ModelState.IsValid)
            {
                if (!db.GeneralSettings.Any())
                {
                    generalSettings.IsDeleted = false;
                    db.GeneralSettings.Add(generalSettings);
                    db.SaveChanges();
                }
                else
                {
                    Session["warning"] = "A setting already exists";
                }
            }
            return RedirectToAction("Index");
        }

        // GET: GeneralSettings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralSettings generalSettings = db.GeneralSettings.Find(id);
            if (generalSettings == null)
            {
                return HttpNotFound();
            }
            return View(generalSettings);
        }

        // POST: GeneralSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,VAT,Location,PinNumber,Contact1,Contact2")] GeneralSettings generalSettings)
        {
            if (ModelState.IsValid)
            {
                generalSettings.IsDeleted = false;
                db.Entry(generalSettings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generalSettings);
        }

        // GET: GeneralSettings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralSettings generalSettings = db.GeneralSettings.Find(id);
            if (generalSettings == null)
            {
                return HttpNotFound();
            }
            return View(generalSettings);
        }

        // POST: GeneralSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GeneralSettings generalSettings = db.GeneralSettings.Find(id);
//            generalSettings.IsDeleted = true;

            db.GeneralSettings.Remove(generalSettings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult StoreBankDetails(FormCollection collection)
        {
            var accountn = collection["AccountNumber"];
            var b = collection["BankName"];
            if (!db.BankDetails.Any(x => x.AccountNumber == accountn && x.Bank == b))
            {
                BankDetails bank = new BankDetails();
                bank.AccountName = collection["AccountName"];
                bank.AccountNumber = collection["AccountNumber"];
                bank.Branch = collection["Branch"];
                bank.Bank = collection["BankName"];
                bank.DateCreated = DateTime.Now;
                bank.ModifiedBy = Library.UserId;
                bank.IsActive = true;
                db.BankDetails.Add(bank);
                db.SaveChanges();
                Session["tab2"] = "tab2";
                Session["success"] = "Bank details added";
            }
            else
            {
                Session["tab2"] = "tab2";
                Session["warning"] = "Account Number already Exist";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBank(FormCollection collection)
        {
            if (collection["BankId"] != "") { 
                var id = Int64.Parse(collection["BankId"]);
                BankDetails bank = db.BankDetails.Find(id);
                bank.IsDeleted = true;
//                db.BankDetails.Remove(bank);
                db.SaveChanges();
                Session["tab2"] = "tab2";
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetBankDetaiils(FormCollection col)
        {
            var id = Int64.Parse(col["id"]);
            BankDetails d = db.BankDetails.Find(id);
            return Json(d);
        }

        public ActionResult EditBankDetails(FormCollection collection)
        {
            if (collection["BankId"] != "")
            {
                var id = Int64.Parse(collection["BankId"]);
                BankDetails bank = db.BankDetails.Find(id);
                bank.AccountName = collection["AccountName"];
                bank.AccountNumber = collection["AccountNumber"];
                bank.Branch = collection["Branch"];
                bank.Bank = collection["BankName"];
                bank.IsDeleted = false;
                db.SaveChanges();
                Session["tab2"] = "tab2";
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetSetting(FormCollection col)
        {
            var id = Int64.Parse(col["id"]);
            GeneralSettings d = db.GeneralSettings.Find(id);
            return Json(d);
        }

//        public ActionResult EditGeneralSetting()
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
