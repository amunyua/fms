using StationLinerMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Facebook;

namespace StationLinerMVC.Controllers
{
    public class ChannelPricesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();
        // GET: ChannelPrices
        public ActionResult Index()
        {
            var channelprices = from lnk_station in db.ProductStationPrices
                                join station in db.Stations.Where(x=>x.IsDeleted != true) on lnk_station.StationId equals station.Id
                                select new ChannelStationName()
                                {
                                    ProductStationPrice = lnk_station,
                                    StationName = station.StationName
                                };
            //var channelPrices = db.Lnk_Products_Channel.Include(p => p).Include(p => p.UnitOfMeasure);
            // ViewBag.ChannelPrices = new SelectList(db.Lnk_Products_Channel, "Id", "ChannelName");
            return View(channelprices.ToList());
        }

        public class ChannelStationName //Custom class created to be accessible in view; Test
        {
            public ProductStationPrice ProductStationPrice { get; set; }
            public string StationName { get; set; }
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.PriceList = new SelectList(db.PriceList.Where(x => x.Id != null), "Id", "PriceListName");
            ViewBag.Products = new SelectList(db.Products.Where(x => x.Id != null && x.IsDeleted != true), "Id", "ProductName");
            ViewBag.Currency = new SelectList(db.Currencies.Where(x => x.Id != null), "Id", "CurrencyName");
            List<ProductStationPrice> lstProdStation = db.ProductStationPrices.Where(x => x.IsActive == true && x.IsDeleted != true ).ToList();
            List<Station> lstStations = db.Stations.Where(x => x.IsActive == true && x.IsDeleted != true).ToList();
            var tplChannelProduct = new Tuple<List<Station>, List<ProductStationPrice>>(lstStations, lstProdStation) { };
            return View(tplChannelProduct);
        }
        [HttpPost]
        public ActionResult Create(FormCollection values)
        {
            String buttonClicked = values["saveexit"];
            List<long> Channel_ids = string.IsNullOrEmpty(values["channelIds"]) ? new List<long>() : values["channelIds"].Split(',').Select(long.Parse).ToList();
            if (Channel_ids != null && Channel_ids.Count() > 0)
            {
                ProductStationPrice prodChannel = new ProductStationPrice();
                for (int x = 0; x < Channel_ids.Count(); x++)
                {
                    var PriceListId = Convert.ToInt64(values["PriceList"]);
                    var ProductId = Convert.ToInt64(values["Products"]);
                    var ProductCurrencyId = Convert.ToInt64(values["Currency"]);
                    var StationId = Channel_ids[x];
                    List<ProductStationPrice> lst = db.ProductStationPrices.Where(y => y.ProductPriceListId == PriceListId && y.ProductId == ProductId && y.StationId == StationId && y.IsDeleted != true ).ToList();
                    if (lst != null && lst.Count > 0)
                    {
                        foreach (ProductStationPrice itm in lst)
                        {
                            itm.ProductPrice = double.Parse(values["Price"]);
                            itm.IsDeleted = false;
                            itm.ModifiedBy = Library.UserId;
                            db.Entry(itm).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        prodChannel.ProductPriceListId = Convert.ToInt64(values["PriceList"]);
                        prodChannel.ProductId = Convert.ToInt64(values["Products"]);
                        prodChannel.ProductPrice = double.Parse(values["Price"]);
                        prodChannel.StationId = Channel_ids[x];
                        prodChannel.IsActive = true;
                        prodChannel.IsDeleted = false;
//                        prodChannel.ProductCurrencyId = ProductCurrencyId;
                        prodChannel.ModifiedBy = Library.UserId;
                        prodChannel.DateCreated = DateTime.Now;
                        
                        db.ProductStationPrices.Add(prodChannel);
                        db.SaveChanges();
                    }
                }
                TempData["msg"] = "Success";
            }
            ViewBag.PriceList = new SelectList(db.PriceList.Where(x => x.Id != null), "Id", "PriceListName");
            ViewBag.Products = new SelectList(db.Products.Where(x => x.Id != null && x.IsDeleted !=true), "Id", "ProductName");
            ViewBag.Currency = new SelectList(db.Currencies.Where(x => x.Id != null), "Id", "CurrencyName");
            List<ProductStationPrice> lstProdStation = db.ProductStationPrices.Where(x => x.IsActive == true && x.IsDeleted != true).ToList();
            List<Station> lstStations = db.Stations.Where(x => x.IsActive == true && x.IsDeleted != true).ToList();
            var tplChannelProduct = new Tuple<List<Station>, List<ProductStationPrice>>(lstStations, lstProdStation) { };
            if (buttonClicked == "saveandexit")
            {
                return RedirectToAction("Index", "ChannelPrices");
            }
            else
            {
                return View(tplChannelProduct);
            }
        }

        public ActionResult PriceLists()
        {
            var priceLists = db.PriceList.OrderByDescending(x => x.Id).ToList();
            return View(priceLists);
        }
        [HttpPost]
        public ActionResult CreatePriceList(PriceList plist)
        {
            if (ModelState.IsValid)
            {
               PriceList priceList = new PriceList
               {
                   PriceListName = plist.PriceListName,
                   PriceListDesc = plist.PriceListDesc,
                   IsActive = true,
                   IsDefault = false,
                   ModifiedBy = Library.UserId,
                   DateCreated = DateTime.Now
                   
               };
                db.PriceList.AddOrUpdate(x=>x.PriceListName, priceList);
                db.SaveChanges();
                Session["success"] = "Pricelist has been added";
            }
            return RedirectToAction("PriceLists");
        }

        public JsonResult GetPriceListDetails(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);
            PriceList list = db.PriceList.Find(id);
            return Json(list);
        }

        public ActionResult EditPriceList(PriceList list)
        {
            if (ModelState.IsValid)
            {
                PriceList p = db.PriceList.Find(list.Id);

                    p.PriceListName = list.PriceListName;
                    p.PriceListDesc = list.PriceListDesc;
                    p.IsActive = list.IsActive;
                db.Entry(p).State = EntityState.Modified;
                Session["success"] = "Pricelist details updated";
                db.SaveChanges();
            }
            return RedirectToAction("PriceLists");
        }

       /* public bool CheckEsisting(dynamic value,)
        {
            IMSDataEntities context = new IMSDataEntities();
            var ex = context.
        }*/

        // POST: Taxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PriceList priceList = db.PriceList.Find(id);
            //            tax.IsDeleted = true;
            /*
            tax.IsActive = false;*/
            db.PriceList.Remove(priceList);
            db.SaveChanges();
            Session["success"] = "The pricelist has been deleted";
            return RedirectToAction("PriceLists");
        }
    }
}