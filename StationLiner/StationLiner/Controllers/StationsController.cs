using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Xml.Schema;
using Microsoft.Ajax.Utilities;
using MoreLinq;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    public class StationsController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Stations
        public ActionResult Index()
        {
            var stations = db.Stations.Where(x=>x.IsDeleted == false).OrderByDescending(x=>x.Id);
            ViewBag.Countries = db.Countries.ToList();
            return View(stations.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.Country = new SelectList(db.Countries.Where(x => x.Id != null), "Id", "CountryName");
            return View();
        }

        public JsonResult GetStationDetails(FormCollection coll)
        {
            var id = Int64.Parse(coll["id"]);
            return Json(db.Stations.Find(id));
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormCollection values) //Changed, Country to be added later
        {
            if (ModelState.IsValid)
            {
                Station stations = db.Stations.Find(Int64.Parse(values["Id"]));
                stations.StationName = values["StationName"];
                stations.StationDesc = values["StationDesc"];
                stations.Address = values["Address"];
                stations.City = values["City"];
                stations.ModifiedBy = Library.UserId;
                stations.IsDeleted = false;
                stations.IsActive = true;
                stations.DateCreated = DateTime.Now;

                db.Entry(stations).State = EntityState.Modified;
                await db.SaveChangesAsync();
               
            }
            return RedirectToAction("Index");
        }

     

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            try
            {
            Station stations = await db.Stations.FindAsync(id);
            //here we get all data associated to stations step by step

            //starting with pumps
            var pumps = db.StationPumps.Where(x => x.StationId == stations.Id);
            if (pumps.Any())
            {
                foreach (StationPump pump in pumps)
                {
                    var pumpNozzoles = db.PumpNozzles.Where(x => x.PumpId == pump.Id && x.IsDeleted == false);
                    if (pumpNozzoles.Any())
                    {
                        foreach (PumpNozzle nozzle in pumpNozzoles)
                        {
                            nozzle.IsDeleted = true;
                        }
                    }
                    pump.IsDeleted = true;
                }
            }
            // delete station tanks
                var tanks = db.StationTanks.Where(x => x.StationId == id && x.IsDeleted == false);
                if (tanks.Any())
                {
                    foreach (StationTanks tank in tanks)
                    {
                        tank.IsDeleted = true;
                    }
                }

                //product station pricelists
                var stationplist = db.ProductStationPrices.Where(x => x.StationId == id && x.IsDeleted != true);
                if (stationplist.Any())
                {
                    foreach (ProductStationPrice list in stationplist)
                    {
                        list.IsDeleted = true;
                    }
                }


            stations.IsDeleted = true;
            stations.IsActive = false;
//            db.Stations.Remove(stations);
           
                await db.SaveChangesAsync();
                Session["success"]= stations.StationName+" has been deleted";
            }
            catch(Exception e)
            {
               SetWarning(e);
            }
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<JsonResult> StationBasicDetails(FormCollection collection)
        {
//            var  acol.AllKeys[];
            List<Int64> status = new List<Int64>();

            StationCategory stationCategory = db.StationCategories.Where(x => x.StationCategoryName == "Fuel Station").FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("StationCatId....." + stationCategory.Id);

            Station station = new Station();
            station.StationName = collection["StationName"];
            station.StationDesc = collection["StationDescription"];
            station.Address = collection["Address"];
            station.City = collection["City"];
            station.CountryId = Int64.Parse(collection["Country"]);
           // station.PumpsCount = 1; //Added
            station.StationCategoryId = stationCategory.Id; //Added to take fuel station category
           // station.TanksCount = 1; //Added
            station.IsActive = true;
            station.IsDeleted = false;
            station.ModifiedBy = Library.UserId;
            station.DateCreated = DateTime.Now;
            string name = collection["StationName"];
            if (await db.Stations.Where(x => x.StationName == name).AnyAsync())
            {
                db.Stations.AddOrUpdate(x => x.StationName, station);
            }
            else
            {
                db.Stations.Add(station);
            }
            System.Diagnostics.Debug.WriteLine("I am here 2.2.....");
            if (collection["Shifts[]"] != null)
            {
                var shiftsArray = collection["Shifts[]"].ToString();
                var shifts = shiftsArray.TrimStart(new char[] { ' ', ',' }).TrimEnd(new char[] { ',', ' ' }).Split(',');

                foreach (var shift in shifts)
                {
                    System.Diagnostics.Debug.WriteLine("I am here 3.....");
                    var sId = Int64.Parse(shift);
                    if (!db.StationShifts.Where(x => x.Id == sId && x.StationId == station.Id).Any())
                    {
                        var shft = new StationShift();
                        shft.ShiftCategoryId = Int64.Parse(shift);
                        shft.StationId = station.Id;
                        shft.ModifiedBy = Library.UserId;
                        db.StationShifts.Add(shft);
                    }

                }
                System.Diagnostics.Debug.WriteLine("I am here 2.3.....");
            } 
            System.Diagnostics.Debug.WriteLine("I am here 2.4.....");

            // here save the selected shifts against the station id
//            var allKeys = collection["Shifts"].Cast<Array>();

            try
            {
                System.Diagnostics.Debug.WriteLine("I am here 4.....");
                db.UserAllocatedChannels.AddOrUpdate(x=>x.StationId,new UserAllocatedChannels{ StationId = station.Id, UserId = Library.UserId, IsDeleted = false});
                db.SaveChanges();
                status.Add(station.Id);
                return Json(status);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("I am here 5.....");
                return Json(new{e.Message});
            }

        }


        
        public async Task<JsonResult> TankDetails(JQueryDataTableParamModel param, String id)
        {
            System.Diagnostics.Debug.WriteLine("Station Id value is "+id);
            using (var db1 = new IMSDataEntities())
            {
                long id2 = Convert.ToInt64(id);
                db.Configuration.LazyLoadingEnabled = false;
                var tanks = db.StationTanks.Where(x => x.StationId == id2 && x.IsDeleted == false);
                System.Diagnostics.Debug.WriteLine("N.o of tanks is " + tanks.Count());
            
            

            return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = tanks.Count(),
                    iTotalDisplayRecords = tanks.Count(),
                    aaData = tanks.ToList()
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> PumpDetails(AjaxModels mod)
        {
            JQueryDataTableParamModel param = new JQueryDataTableParamModel();
//            System.Diagnostics.Debug.WriteLine("Station Id value is " + coll["id"]);
//            using (var db1 = new IMSDataEntities())
//            {
//                long id2 = Int64.Parse(coll["id"]);
                db.Configuration.LazyLoadingEnabled = false;
                var pumps = db.StationPumps.Where(x => x.StationId == mod.Id && x.IsDeleted == false);
                System.Diagnostics.Debug.WriteLine("N.o of tanks is " + pumps.Count());



                return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = pumps.Count(),
                        iTotalDisplayRecords = pumps.Count(),
                        aaData = pumps.ToList()
                    },
                    JsonRequestBehavior.AllowGet);
//            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddTank(FormCollection collection)
        {
            List<string> status = new List<string>();
            StationTanks tank = new StationTanks();
            System.Diagnostics.Debug.WriteLine("Tankname is " + collection["TankName"]);
            System.Diagnostics.Debug.WriteLine("Description is " + collection["Description"]);
            System.Diagnostics.Debug.WriteLine("FuelType is " + Int64.Parse(collection["FuelType"]));
            System.Diagnostics.Debug.WriteLine("Length is " + double.Parse(collection["length"]));
            System.Diagnostics.Debug.WriteLine("Volume is " + double.Parse(collection["Volume"]));
            System.Diagnostics.Debug.WriteLine("Diameter is " + double.Parse(collection["Diameter"]));

            tank.TankName = collection["TankName"];
            tank.TankDesc = collection["Description"];
            tank.ProductId = Int64.Parse(collection["FuelType"]);
            tank.IsActive = true;
            tank.StationId = Int64.Parse(collection["StationId"]);
            tank.Length = double.Parse(collection["length"]);
            tank.Volume = double.Parse(collection["Volume"]);
            tank.Diameter = double.Parse(collection["Diameter"]);
            tank.DateCreated = DateTime.Now;
            tank.IsDeleted = false;

            System.Diagnostics.Debug.WriteLine("I am here 1...........");

            db.StationTanks.Add(tank);
            System.Diagnostics.Debug.WriteLine("I am here 2...........");

            try
            {
                System.Diagnostics.Debug.WriteLine("I am here 3...........");
                await db.SaveChangesAsync();
                status.Add("success");
                System.Diagnostics.Debug.WriteLine("I am here 4...........");
            }
            catch
            {
                status.Add("failed");
            }
            return Json(status);
        }
        [HttpPost]
//        [ValidateAntiForgeryToken]
        public JsonResult GetTanks(FormCollection collection)
        {
            var station = Int64.Parse(collection["Station"]);
            var fuelTypes = collection["FuelType"].Split('/');
            var fuelType = Int64.Parse(fuelTypes[0]);

            var tanks =  db.StationTanks.Where(x => x.StationId == station && x.ProductId == fuelType && x.IsDeleted == false).ToList();

            return Json(tanks);
        }
        //I am here//////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPumpDetails(FormCollection collection)
        {
            List<int> state = new List<int>();
            var pump = new StationPump();
            pump.StationId = Int64.Parse(collection["StationId"]);
            pump.Name = collection["Name"];
            pump.DateCreated = DateTime.Now;
            pump.IsDeleted = false;
            pump.Description = collection["Description"];
            pump.IsDoubleSided = Int64.Parse(collection["Sides"]) == 2 ? true : false; //changed to boolean
            pump.ModelId = Int64.Parse(collection["Model"]); //changed to ModelId
            db.StationPumps.Add(pump);
            var fuelTypes = collection["FuelType[]"].ToString();
            var tankIds = collection["TankId[]"].ToString();

            if (fuelTypes.Any())
            {
                var fuelTyesArray = fuelTypes.TrimStart(new char[]{' ',','}).TrimEnd(new char[]{',',' '}).Split(',');
                var tankIdsArray = tankIds.TrimStart(new char[]{' ',','}).TrimEnd(new char[]{',',' '}).Split(',');
                for(var i=0; i<fuelTyesArray.Length; i++)
                {
                    Debug.WriteLine(tankIdsArray[i]);
                    var sides = fuelTyesArray[i].Split('/');
                    var prodId = sides[0];
                    var side = sides[1];

                    var tankId = Int64.Parse(tankIdsArray[i]);
//                    if (!db.PumpNozzles.Any(x => x.ProductId == productId && x.TankId == tankId && x.PumpId == pump.Id))
//                    {
                        var nozzle = new PumpNozzle();
                        nozzle.ProductId = Int64.Parse(prodId); //Changed to productId
                        nozzle.TankId = Int64.Parse(tankIdsArray[i]);
                        nozzle.PumpId = pump.Id;
                        nozzle.DateCreated = DateTime.Now;
                        nozzle.IsActive = true;
                    nozzle.IsDeleted = false;
                    nozzle.Side = side;
                        db.PumpNozzles.Add(nozzle);
//                    } uncomment to validate unique sides

                    
                }
                try
                {
                    db.SaveChanges();
                    return Json("success");
                }
                catch(Exception e)
                {
                    Library.Log(e.Message,e.StackTrace);
                    return Json("Failed");
                }

            }


            return Json(state);
        }

        public async Task<ActionResult> ViewTanks(long id)
        {
            var tanks = db.StationTanks.Where(x=>x.StationId == id && x.IsDeleted == false).OrderByDescending(x=>x.Id);
//            ViewBag.station = tanks
            ViewBag.station = db.Stations.Find(id).StationName;
            ViewBag.StationId = id;
            return View( await tanks.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTankHttp(FormCollection collection)
        {
//            List<string> status = new List<string>();
            StationTanks tank = new StationTanks();
            tank.TankName = collection["TankName"];
            tank.TankDesc = collection["Description"];
            tank.ProductId = Int64.Parse(collection["FuelType"]); //changed to productId
            tank.IsActive = true;
            tank.StationId = Int64.Parse(collection["StationId"]);
            tank.Length = double.Parse(collection["length"]);
            tank.Volume = double.Parse(collection["Volume"]);
            tank.Diameter = double.Parse(collection["Diameter"]);
            tank.DateCreated = DateTime.Now;
            tank.IsDeleted = false;

            db.StationTanks.Add(tank);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
               SetWarning(e);
            }
            

            return RedirectToAction("ViewTanks",new{id = collection["StationId"]});
        }

        //function to delete a tank
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteTank(long id, FormCollection collection)
        {
            StationTanks tank = await db.StationTanks.FindAsync(id);
//            db.StationTanks.Remove(tank);
            try
            {
                tank.IsDeleted = true;
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
               SetWarning(e);
            }
            return RedirectToAction("ViewTanks",new {id = collection["StationId"]});
        }

        //view pumps
        public ActionResult ViewPumps(long id)
        {
            var pumps = db.StationPumps.Where(x => x.StationId == id  && x.IsDeleted == false).OrderByDescending(x=>x.Id);
            ViewBag.StationId = id;
            try
            {
                ViewBag.station = db.Stations.Find(id).StationName;
            }
            catch (Exception e)
            {
               SetWarning(e);
            }
            
            return View(pumps.ToList());
        }
        //add pumps http
        [HttpPost]
//        [ValidateAntiForgeryToken]
        public ActionResult AddPumpHttp(FormCollection collection)
        {
            var pump = new StationPump();
            pump.StationId = Int64.Parse(collection["StationId"]);
            pump.Name = collection["Name"];
            pump.Description = collection["Description"];
            pump.IsDoubleSided = Int64.Parse(collection["Sides"]) == 2 ? true : false; //changed to boolean
            pump.ModelId = Int64.Parse(collection["Model"]); //changed to ModelId
            pump.DateCreated = DateTime.Now;
            pump.IsDeleted = false;
            pump.IsActive = true;
            db.StationPumps.Add(pump);
//            db.SaveChanges();
            var fuelTypes = collection["FuelType[]"].ToString();
            var tankIds = collection["TankId[]"].ToString();

            if (fuelTypes.Any())
            {
                var fuelTyesArray = fuelTypes.TrimStart(new char[]{' ',','}).TrimEnd(new char[]{',',' '}).Split(',');
                var tankIdsArray = tankIds.TrimStart(new char[]{' ',','}).TrimEnd(new char[]{',',' '}).Split(',');
                for(var i=0; i<fuelTyesArray.Length; i++)
                {
                    Debug.WriteLine(tankIdsArray[i]);
                    var sides = fuelTyesArray[i].Split('/');
                    var prodId = sides[0];
                    var side = sides[1];
//                    if (!db.PumpNozzles.Any(x => x.ProductId == prodId && x.TankId == tId && x.PumpId == pump.Id))
//                    {
                        var nozzle = new PumpNozzle();
                        nozzle.ProductId = Int64.Parse(prodId); 
                        nozzle.TankId = Int64.Parse(tankIdsArray[i]);
                        nozzle.PumpId = pump.Id;
                        nozzle.DateCreated = DateTime.Now;
                        nozzle.IsActive = true;
                        nozzle.Side = side;
                    nozzle.IsDeleted = false;
                        db.PumpNozzles.Add(nozzle);
//                    }
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    SetWarning(e);
                }
                
            }
            return RedirectToAction("ViewPumps",new {id = collection["StationId"]});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePumps(long id,FormCollection collection)
        {
            try
            {
                StationPump pump = db.StationPumps.Find(id);
                var nozzles = db.PumpNozzles.Where(x => x.PumpId == pump.Id);
//            db.PumpNozzles.RemoveRange(zozzles);
//            db.StationPumps.Remove(pump);
                if (nozzles.Any())
                {
                    foreach (PumpNozzle nozzle in nozzles)
                    {
                        nozzle.IsDeleted = true;
                    }
                }
                pump.IsDeleted = true;
                db.SaveChanges();
                Session["success"] = "Pump has been deleted";
            }
            catch (Exception e)
            {
               SetWarning(e);
            }
           
            return RedirectToAction("ViewPumps",new {id = collection["StationId"]});
        }
        [HttpPost]
        public JsonResult AssignStation(FormCollection collection)
        {
            var stationId = Int64.Parse(collection["stationId"]);
            var shiftId = Int64.Parse(collection["shiftId"]);
            string status = "";

            if (collection["action"] == "attach")
            {
                if (!db.StationShifts.Where(x => x.ShiftCategoryId == shiftId && x.StationId == stationId).Any())
                {
                    StationShift shift = new StationShift();
                    shift.ShiftCategoryId = shiftId;
                    shift.StationId = stationId;
                    shift.ModifiedBy = Library.UserId;
                    db.StationShifts.Add(shift);
                    db.SaveChanges();
                    status = "attached";
                }

            }else if(collection["action"] == "detach")
            {
                StationShift shift = db.StationShifts.Where(x => x.ShiftCategoryId == shiftId && x.StationId == stationId)
                    .FirstOrDefault();
                if (shift != null)
                {
                    db.StationShifts.Remove(shift);
                    db.SaveChanges();
                    status = "dettached";
                }

            }else if (collection["action"] == "check-state")
            {
                if (db.StationShifts.Where(x => x.ShiftCategoryId == shiftId && x.StationId == stationId).Any())
                {
                    status = "shift-attached";
                }
            }
            return Json(status);
        }
        [HttpPost]
        public JsonResult CheckStatus(FormCollection collection)
        {
            return Json("suceess");
        }

        public JsonResult GetEditTank(FormCollection col)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var id = Int64.Parse(col["id"]);   
            StationTanks tank = db.StationTanks.Find(id);
            return Json(tank);
        }

        public ActionResult EditTank(FormCollection collection)
        {
            long id =Int64.Parse(collection["TankId"]);
            long stationId = Int64.Parse(collection["StationId"]);
            StationTanks tank = db.StationTanks.Find(id);
            tank.TankName = collection["TankName"];
            tank.TankDesc = collection["Description"];
            tank.Diameter = double.Parse(collection["Diameter"]);
            tank.Length = double.Parse(collection["Length"]);
            tank.Volume = double.Parse(collection["Volume"]);
            tank.IsDeleted = false;
//            db.StationTanks.Add(tank);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                SetWarning(e);
            }
            

            return RedirectToAction("ViewTanks", new {id = stationId});

        }
        public JsonResult GetEditPumpDetails(FormCollection col)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var id = Int64.Parse(col["id"]);   
            StationPump pump = db.StationPumps.Find(id);
            var nozzles =  db.PumpNozzles.Where(x => x.PumpId == pump.Id).ToList();
            return Json(new { pump = pump, nozzoles= nozzles});
        }
        public ActionResult EditPump(FormCollection collection)
        {
            long stationId = Int64.Parse(collection["StationId"]);
            try
            {
                long id = Int64.Parse(collection["PumpId"]);
                
                StationPump pump = db.StationPumps.Find(id);
                pump.Name = collection["Name"];
                pump.IsDeleted = false;
//                pump.IsActive = true;
                pump.ModelId = Int64.Parse(collection["Model"]); //Changed to ModelId
                pump.Description = collection["Description"];
                //            db.StationTanks.Add(tank);
                db.SaveChanges();
            }
            catch (Exception e)
            {
               SetWarning(e);
            }
            

            return RedirectToAction("ViewPumps", new { id = stationId });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetWarning(Exception e)
        {
            Library.Log(e.Message, e.StackTrace);
            Session["warning"] = "Something went wrong, please contact admin";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private class TankModels
        {
            private long TankId { get; set; }
            private string TankName { get; set; }
            private long StationId { get; set; }
        }
        public ActionResult CalibrationChart(long id)
        {
            var allocatedStations = db.UserAllocatedChannels.Where(x => x.UserId == Library.UserId)
                .Select(x => x.StationId).ToList();
//            ViewBag.tanks = db.StationTanks.Where(x => x.IsDeleted !=true).ToList();
            ViewBag.tanks = db.StationTanks.Where(x => x.IsDeleted != true && x.StationId == id).ToList();
            return View();
        }

        public class GetId
        {
            public long Id { get; set; }
        }
        
        public JsonResult CalibrationDetails(JQueryDataTableParamModel param, string id)
        {
//            return Json(new{id},JsonRequestBehavior.AllowGet);
            using (var db1 = new IMSDataEntities())
            {
                long id2 = Convert.ToInt64(id);
                db.Configuration.LazyLoadingEnabled = false;
//                var tanks = db.StationTanks.Where(x => x.StationId == id2 && x.IsDeleted == false);
                var calibrations = db.CalibrationChart.Where(x=>x.TankId == id2);

                return Json(new 
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = calibrations.Count(),
                        iTotalDisplayRecords = calibrations.Count(),
                        aaData = calibrations.ToList()
                    },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public class CalibrationModels
        {
            public long TankId { get; set; }
            public List<CalbModels> Calibration { get; set; }
        }

        public class CalbModels
        {
            public double Height { get; set; }
            public double Volume { get; set; }
        }
        /// <summary>
        /// Update calibration details.
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateCalibtationDetails(CalibrationModels calibration)
        {
            var status = "Failed";
            var message = "";
            
            
            try
            {
                var maxVol = calibration.Calibration.MaxBy(x => x.Volume);
                var tankMaxVolume = db.StationTanks.FirstOrDefault(x => x.Id == calibration.TankId);
                if (maxVol.Volume > tankMaxVolume.Volume)
                {
                    message = maxVol.Volume + " exceed the maximum volume defined for this tank";
                }
                else
                {
                    List<CalibrationChart> calibrations =
                        db.CalibrationChart.Where(x => x.TankId == calibration.TankId).ToList();
                    db.CalibrationChart.RemoveRange(calibrations);
                    db.SaveChanges();
                    if (calibration.Calibration.Any())
                    {
                        foreach (var tankCalibration in calibration.Calibration)
                        {
                            db.CalibrationChart.Add(new CalibrationChart
                            {
                                TankId = calibration.TankId,
                                Height = tankCalibration.Height,
                                Volume = tankCalibration.Volume
                            });
                        }

                    }
                    db.SaveChanges();
                    status = "Success";
                    message = "Calibration details have been recorded";
                }
            }
            catch (Exception e)
            {
                Library.Log2(e);
                message = "Something went wrong, please contact admin";
            }
            return Json(new { status = status, message = message});
        }

        public JsonResult getTankVolume(GetId id)
        {
            try
            {
                StationTanks tank = db.StationTanks.Find(id.Id);
                return Json(tank);
            }
            catch (Exception e)
            {
                Library.Log2(e);
            }
            
            return Json("");
        }
    }
}
