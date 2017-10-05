using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth.Messages;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class ShiftsController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Shifts
        public ActionResult Index()
        {
            var shifts = db.ShiftCategories.Where(x=>x.IsDeleted != true);
            return View(shifts.ToList());
        }


      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShiftName,ShiftDescription")] ShiftCategory shift)
        {
            if (ModelState.IsValid)
            {
                if (db.ShiftCategories.Any(x => x.ShiftName == shift.ShiftName && x.IsDeleted != true))
                {
                    Session["warning"] = shift.ShiftName + " already exist";
                    return RedirectToAction("Index");
                }
                shift.IsActive = true;
                shift.StartTime = DateTime.Now;
                shift.EndTime = DateTime.Now;
                shift.IsDeleted = false;
                db.ShiftCategories.Add(shift);
                db.SaveChanges();
                Session["success"] = "The Shift has been created";
               
            }
            return RedirectToAction("Index");
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShiftName,ShiftDescription")] ShiftCategory shift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shift).State = EntityState.Modified;
                shift.EndTime = DateTime.Now;
                shift.StartTime = DateTime.Now;
                shift.IsActive = true;
                shift.IsDeleted = false;
                db.SaveChanges();
                Session["success"] = "Shift details have been updated";
                return RedirectToAction("Index");
            }
//            ViewBag.ChannelId = new SelectList(db.Channels, "Id", "ChannelName", shift.ChannelId);
            return View(shift);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ShiftCategory shift = db.ShiftCategories.Find(id);
            //shift.IsDeleted = true; //To move record to historical tables
//            db.ShiftCategories.Remove(shift);

            //remove records associated with this shift
            var stationshifts = db.StationShifts.Where(x => x.ShiftCategoryId == shift.Id);
            var stationShifts = db.StationShifts.RemoveRange(stationshifts);
            shift.IsActive = false;
            shift.IsDeleted = true;
            try
            {
                db.SaveChanges();
                Session["success"] = "Shift has been deleted";
            }
            catch (Exception e)
            {
               Library.Log(e.Message,e.StackTrace);
            }
           
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

        public JsonResult GetShiftDetails(FormCollection col)
        {
            var id = Int64.Parse(col["id"]);
            var data = db.ShiftCategories.Find(id);
            return Json(data);
        }
    }
}
