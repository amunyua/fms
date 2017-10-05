using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    public class ProductsController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Where(x=>x.IsDeleted == false).Include(p => p.ProductCategory).Include(p => p.TaxCategory);
            ViewBag.productCategories = db.ProductCategories.ToList();
            ViewBag.taxes = db.TaxCategories.ToList();
            return View( products.ToList());
        }

        public JsonResult GetProductDetails(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);
            var result = db.Products.Find(id);
            return Json(result);
        }

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Any(x => x.ProductName == product.ProductName))
                {
                    Session["warning"] = "The product name already exist";
                    return RedirectToAction("Index");
                }
                product.DateCreated = DateTime.Now;
                product.IsActive = true;
                product.IsDeleted = false;
                product.ModifiedBy = Library.UserId;
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                product.IsActive = true;
                product.DateCreated = DateTime.Now;
                product.IsDeleted = false;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Product product = await db.Products.FindAsync(id);
            product.IsDeleted = true;
//            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult IsProductsExists(string name)
        {
//check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.
            return Json(!db.Products.Any(x => x.ProductName == name && x.ModifiedBy == Library.UserId) ,JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExist(string Email)
        {
//check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.
            return Json(!db.Users.Any(x => x.Email == Email && x.IsActive == true) ,JsonRequestBehavior.AllowGet);
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
