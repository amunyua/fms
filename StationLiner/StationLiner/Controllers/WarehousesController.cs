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
    public class WarehousesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Warehouses
        public ActionResult Index()
        {
            var warehouses = db.Warehouses.Where(x=>x.IsActive == true).Include(w => w.WarehouseType);
            return View(warehouses.ToList());
        }


        // GET: Warehouses/Create
        public ActionResult Create()
        {
            ViewBag.WarehouseTypeId = new SelectList(db.WarehouseTypes, "Id", "Type");
            return View();
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                warehouse.DateCreated = DateTime.Now;
                warehouse.IsActive = true;
                warehouse.ModifiedBy = Library.UserId;
                db.Warehouses.Add(warehouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WarehouseTypeId = new SelectList(db.WarehouseTypes, "Id", "Type", warehouse.WarehouseTypeId);
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            ViewBag.WarehouseTypeId = new SelectList(db.WarehouseTypes, "Id", "Type", warehouse.WarehouseTypeId);
            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                warehouse.IsActive = true;
                db.Entry(warehouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WarehouseTypeId = new SelectList(db.WarehouseTypes, "Id", "Type", warehouse.WarehouseTypeId);
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse == null)
            {
                return HttpNotFound();
            }
            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Warehouse warehouse = db.Warehouses.Find(id);
            warehouse.IsActive = false;
            db.Warehouses.Remove(warehouse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult WarehouseProducts(long? id)
        {
                        if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            var products = db.WarehouseProducts.Where(p => p.WarehouseId == id).Include(p => p.Product);

            return View(products);
        }

        [HttpPost]
        public ActionResult AddProduct([Bind(Include = "ProductId,ReorderLevel, WarehouseId")] WarehouseProduct warehouseP)
        {
            //check whether the warehouse item already exists
            var warehouse_id = Int64.Parse(Request.Form["WarehouseId"]);
            var product_id = Int64.Parse(Request.Form["ProductId"]);
            
            if (
                db.WarehouseProducts.Any(
                    x =>
                        x.WarehouseId == warehouse_id &&
                        x.ProductId == product_id ))
            {
                Session["warning"] = "This item already exist in this warehouse";
            }
            else
            {
                warehouseP.AvailableStock = 0.0;
                db.WarehouseProducts.Add(warehouseP);
                db.SaveChanges();
                Session["success"] = "Item has been added";
            }


            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveWarehouseProduct(long? id)
        {
           WarehouseProduct p = db.WarehouseProducts.Find(id);
            db.WarehouseProducts.Remove(p);
            
            db.SaveChanges();
            Session["success"] = "The product has been removed from the warehouse";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult EditWarehouseItem()
        {
            var id = Int64.Parse(Request.Form["ProductId"]);
            var reoderLevel = int.Parse(Request.Form["ReorderLevel"]);
            WarehouseProduct p = db.WarehouseProducts.Find(id);
            p.ReorderLevel = reoderLevel;
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
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
