using StationLinerMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class WarehouseTypesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: WarehouseTypes
        public ActionResult Index()
        {
            return View(db.WarehouseTypes.ToList());
        }

      

        // GET: WarehouseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WarehouseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Description")] WarehouseType warehouseType)
        {
            if (ModelState.IsValid)
            {
                warehouseType.DateCreated = DateTime.Now;
                db.WarehouseTypes.Add(warehouseType);
//                db.WarehouseTypes.AddOrUpdate(x=>x.Type, warehouseType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(warehouseType);
        }

        // GET: WarehouseTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarehouseType warehouseType = db.WarehouseTypes.Find(id);
            if (warehouseType == null)
            {
                return HttpNotFound();
            }
            return View(warehouseType);
        }

        // POST: WarehouseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Description")] WarehouseType warehouseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouseType);
        }

        // GET: WarehouseTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarehouseType warehouseType = db.WarehouseTypes.Find(id);
            if (warehouseType == null)
            {
                return HttpNotFound();
            }
            return View(warehouseType);
        }

        // POST: WarehouseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            WarehouseType warehouseType = db.WarehouseTypes.Find(id);
            db.WarehouseTypes.Remove(warehouseType);
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
