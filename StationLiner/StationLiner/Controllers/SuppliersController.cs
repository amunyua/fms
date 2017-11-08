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
    public class SuppliersController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Suppliers
        public ActionResult Index()
        {
            ViewBag.Countries = db.Countries.ToList();
            ViewBag.Terms = db.PaymentTerms.OrderByDescending(x => x.Id).ToList();
            return View(db.Supppliers.Where(x=>x.IsDeleted == false).OrderByDescending(x=>x.Id).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (db.Supppliers.Any(x => x.Name == supplier.Name && x.IsDeleted == false))
                {
                    Session["warning"] = "The supplier name already exist";
                    return RedirectToAction("Index");
                }
                Supplier s = new Supplier();
                s.ModifiedBy = Library.UserId;
                s.Name = supplier.Name;
                s.City = supplier.City;
                s.Contact1 = supplier.Contact1;
                s.Contact2 = supplier.Contact2;
                s.ContactPersonName = supplier.ContactPersonName;
                s.CountryId = supplier.CountryId;
                s.CreditLimit = supplier.CreditLimit;
                s.DateCreated = DateTime.Now;
                s.Email = supplier.Email;
                s.IsActive = true;
                s.PostalCode = supplier.PostalCode;
                s.PinNumber = supplier.PinNumber;
                s.Notes = supplier.Notes;
                s.PaymentTerms = supplier.PaymentTerms;
                s.IsActive = true;
                s.PostalAddress = supplier.PostalAddress;
                s.ContactOneName = supplier.ContactOneName;
                s.ContactOneDesignation = supplier.ContactOneDesignation;
                s.ContactOneEmail = supplier.ContactOneEmail;
                s.ContactOneTelephone = supplier.ContactOneTelephone;
                s.ContactTwoName = s.ContactTwoName;
                s.ContactTwoDesignation = s.ContactTwoDesignation;
                s.ContactTwoEmail = supplier.ContactTwoEmail;
                s.ContactTwoTelephone = s.ContactTwoTelephone;
                s.MOQ = supplier.MOQ;
                s.IsDeleted = false;
//                s.DateCreated

                db.Supppliers.Add(s);
                try
                {
                    db.SaveChanges();
                    Session["success"] = "Supplier created";
                }
                catch (Exception e)
                {
                    Session["warning"] = "Something went wrong, please contact the admin";
                   Library.Log(e.Message,e.StackTrace);
                }
               
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        

        public JsonResult GetSupplierEditDetails(FormCollection coll)
        {
            var id = Int64.Parse(coll["id"]);
            var data = db.Supppliers.Find(id);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                supplier.IsActive = true;
                supplier.IsDeleted = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Session["warning"] = "Something went wrong";
            return RedirectToAction("Index");
        }
        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Supplier supplier = db.Supppliers.Find(id);
            //get supplier drivers with the above id and disable them
            var drivers = db.Drivers.Where(x => x.SupplierId == supplier.Id);
            if (drivers.Any())
            {
                foreach (SupplierDriver driver in drivers)
                {
                    driver.IsDeleted = true;
                }
            }
            //get this suppliers vehicles and disable them
            var vehicles = db.Vehicles.Where(x => x.SupplierId == supplier.Id);
            if (vehicles.Any())
            {
                foreach (SupplierVehicle vehicle in vehicles)
                {
                    vehicle.IsDeleted = true;
                }
            }

            //get supplier products and disable them
            var supplierProducts = db.SupplierProducts.Where(x => x.SupplierId == supplier.Id);
            if (supplierProducts.Any())
            {
                foreach (SupplierProduct product in supplierProducts)
                {
                    product.IsDeleted = true;
                }
            }
           
            //supplier product prices
//            db.Supppliers.Remove(supplier);

            try
            {
                supplier.IsDeleted = true;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Library.Log(e.Message,e.StackTrace);
            }
            return RedirectToAction("Index");
        }

        public ActionResult SupplierProducts(long? id) //Return the other view??
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            //var products = db.SupplierProducts.Where(p => p.Id == id).Include(p => p.Product);
            var productPrice = db.SupplierProductPrices.Where(x => x.SupplierProductId == id).Include(y => y.SupplierProduct).Include(z => z.SupplierProduct.Product);

            return View(productPrice);
        }

        [HttpPost]
        public ActionResult AddProduct( SupplierProductPrice supplierP) //Add to both tables??
        {
            //check whether the warehouse item already exists
            var supplierId = Int64.Parse(Request.Form["SupplierId"]);
            var productId = Int64.Parse(Request.Form["ProductId"]);
            var price = Double.Parse(Request.Form["Price"]);
            var minquantity = Double.Parse(Request.Form["MOQ"]);
            var startdate = Request.Form["StartDate"];
            var enddate = Request.Form["EndDate"];
            var uom = Int64.Parse(Request.Form["UOM"]);

            if (
                db.SupplierProductPrices.Any(
                    x =>
                        x.SupplierProduct.SupplierId == supplierId&&
                        x.SupplierProduct.Product.Id == productId &&
                        x.Price == price && 
                        x.UOM.Id == uom))
            {
                Session["warning"] = "Item with similar price and unit of measurement is already attached to supplier"; //Same item can be attached to supplier with different prices
            }
            else
            {
                SupplierProduct supplierProduct = new SupplierProduct();
                supplierProduct.SupplierId = supplierId;
                supplierProduct.ProductId = productId;
                supplierProduct.IsActive = true;
                db.SupplierProducts.Add(supplierProduct); //will these save and not cause foreign key conflict??

                supplierP.DateCreated = DateTime.Now;
                db.SupplierProductPrices.Add(supplierP);

                db.SaveChanges();
                Session["success"] = "Item has been added";
            }


            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveSupplierProduct(long? id) //Should also remove supplier product price
        {
            SupplierProduct p = db.SupplierProducts.Find(id);
            db.SupplierProducts.Remove(p);

            db.SaveChanges();
            Session["success"] = "The product has been detached from the supplier";
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult EditSupplierItem(FormCollection collection)
        {
            var id = Int64.Parse(Request.Form["ProductId"]); //Where is this id coming from; test edit??
            var price = int.Parse(Request.Form["Price"]);
           // SupplierProduct p = db.SupplierProducts.Find(id);
            SupplierProductPrice productPrice = db.SupplierProductPrices.Find(id);

            productPrice.Price = price;
//            p.Validity = collection["Validity"];
//            p.MOQ = double.Parse(collection["MOQ"]);
           // p.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }


        //show all drivers
        public ActionResult Drivers(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supp = db.Supppliers.Find(id);
            ViewBag.supplierName = supp.Name;
            return View(db.Drivers.Where(x=>x.SupplierId == id && x.IsDeleted == false).OrderByDescending(x=>x.Id).Include(x=>x.Suppliers));
        }

        public ActionResult AddDriver([Bind(Include = "DriverName,PhoneNumber,SupplierId")] SupplierDriver driver)
        {
            if (ModelState.IsValid)
            {
                if (db.Drivers.Any(x => x.SupplierId == driver.SupplierId && x.DriverName == driver.DriverName &&
                                        x.IsDeleted == false))
                {
                    Session["warning"] = "Driever name already exist";
                }
                try
                {
                    driver.IsDeleted = false;
                    db.Drivers.Add(driver);
                    db.SaveChanges();
                    Session["success"] = "Driver has been added";
                }
                catch (Exception e)
                {
                  SetWarning(e);
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        //function to delete a driver from the supplier
        public ActionResult DeleteDriver(long? id)
        {
            SupplierDriver p = db.Drivers.Find(id);
            p.IsDeleted = true;
//            db.Drivers.Remove(p);

            db.SaveChanges();
            Session["success"] = "The driver details have been deleted";
            return Redirect(Request.UrlReferrer.ToString());
        }


        //Edit driver details
        public ActionResult EditDriverDetails()
        {
            var id = Int64.Parse(Request.Form["DriverId"]);
            SupplierDriver driver = db.Drivers.Find(id);
            try
            {
                driver.DriverName = Request.Form["DriverName"];
                driver.PhoneNumber = Request.Form["PhoneNumber"];
                driver.IsDeleted = false;

                db.SaveChanges();
                Session["success"] = "Driver details have been updated";
            }
            catch (Exception e)
            {
               SetWarning(e);
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        //show all Vehicles
        public ActionResult Vehicles(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supp = db.Supppliers.Find(id);
            ViewBag.supplierName = supp.Name;
            return View(db.Vehicles.Where(x => x.SupplierId == id && x.IsDeleted == false).OrderByDescending(x=>x.Id).Include(x => x.Suppliers));
        }

        public ActionResult AddVehicle([Bind(Include = "RegNumber,MaximumCapacity,SupplierId")] SupplierVehicle v)
        {
            if (ModelState.IsValid)
            {
                if (db.Vehicles.Any(x => x.SupplierId == v.SupplierId && x.RegNumber == v.RegNumber))
                {
                    Session["warning"] = "Vehicle registration already exist";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                v.IsDeleted = false;
                db.Vehicles.Add(v);
                db.SaveChanges();
                Session["success"] = "Vehicle has been added";
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        //function to delete a driver from the supplier
        public ActionResult DeleteVehicle(long? id)
        {
            SupplierVehicle v = db.Vehicles.Find(id);
            v.IsDeleted = true;
//            db.Vehicles.Remove(v);
            try
            {
                db.SaveChanges();
                Session["success"] = "The Vehicle details have been deleted";
            }
            catch (Exception e)
            {
                SetWarning(e);
            }
           
            return Redirect(Request.UrlReferrer.ToString());
        }


        //Edit Vehicle details
        public ActionResult EditVehicleDetails()
        {
            var id = Int64.Parse(Request.Form["VehicleId"]);
            SupplierVehicle v = db.Vehicles.Find(id);

            v.RegNumber = Request.Form["RegNumber"];
            v.MaximumCapacity = Request.Form["MaximumCapacity"];
            db.SaveChanges();
            Session["success"] = "Vehicle details have been updated";

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

        public JsonResult GetEditVehicleDetails(FormCollection collection)
        {
            var id = Int64.Parse(collection["vehicleId"]);

            SupplierVehicle v = db.Vehicles.Find(id);

            return Json(v);
        }

        public JsonResult GetDriverDetails(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);
            var data = db.Drivers.Find(id);
            return Json(data);
        }

        private void SetWarning(Exception e)
        {
            Library.Log(e.Message,e.StackTrace);
            Session["warning"] = "Something went wrong, please contact admin";
        }
    }
}
