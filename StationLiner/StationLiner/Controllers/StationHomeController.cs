using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class StationHomeController : Controller
    {
        // GET: StationHome
        private IMSDataEntities db = new IMSDataEntities();

        public ActionResult StationHome()
        {
            
            // the line below if for making sure there station id is always active when available
            var ActiveStation = db.UserLayout.FirstOrDefault(x=>x.UserId == Library.UserId);
            if (ActiveStation != null)
            {
                Library.ChannelId = ActiveStation.ChannelId;
            }

            var shift = db.Shifts.FirstOrDefault(x => x.IsActive == true && x.StationId == Library.ChannelId);
            ViewBag.shift = shift;
            if (shift != null)
            {
                var shiftDetails = db.ShiftCategories.Find(shift.ShiftCategoryId);
                ViewBag.shiftDetails = shiftDetails;
            }
            ViewBag.openShift = IsMenuAllocated("OPEN SHIFT");
            ViewBag.CashDrop = IsMenuAllocated("DROP CASH");
            ViewBag.BankCash = IsMenuAllocated("BANK CASH");
            ViewBag.CloseShift = IsMenuAllocated("CLOSE SHIFT");
            ViewBag.ReceiveFuel = IsMenuAllocated("RECEIVE FUEL");
            return View();
        }

        public bool IsMenuAllocated(string menuName)
        {
            bool status = false;
            StationMenus menu = db.StationMenus.FirstOrDefault(x => x.MenuName == menuName);

            if (menu != null)
            {
                if (db.StationRoles.Any(x => x.UserId == Library.UserId && x.StationMenuId == menu.Id))
                {
                    status = true;
                }
            }
            return status;
        }

        public ActionResult MakeCashDrop()
        {
            //get active shift
            var activeShift = db.Shifts.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsActive);
            long shift = 0;
            if (activeShift != null)
            {
                shift = activeShift.Id;
            }
            ViewBag.shift = shift;
            ViewBag.allDrops = db.CashDrops.Where(x => x.StationId == Library.ChannelId).ToList();
            return View(db.CashDrops.Where(x=>x.StationId == Library.ChannelId && x.ShiftId == shift).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeCashDrop(CashDrop drop)
        {
            //get payment modes
            var mode = db.PaymentModes.FirstOrDefault(x => x.PaymentModeName == "Cash");
            //at this point check if there is an active shift
            Shift shift = db.Shifts.FirstOrDefault(x => x.IsActive);
            if (shift != null)
            {
                try
                {
                CashDrop cDrop = new CashDrop();
                cDrop.StaffId = drop.StaffId;
                cDrop.Amount = drop.Amount;
                cDrop.ShiftId = shift.Id;
                cDrop.DateCreated = DateTime.Now;
                cDrop.PaymentModeId = mode.Id;
                cDrop.StationId = Library.ChannelId;
                db.CashDrops.Add(cDrop);
                

                    db.SaveChanges();
                    Session["success"] = "Cashdrop created";
                }
                catch (Exception e)
                {
                   SetWarning(e);
                }
            }
            else
            {
                Session["warning"] = "You cannot make a cash drop without opening a shift";
            }
            return RedirectToAction("StationHome");
        }

      
        [HttpGet]
        public ActionResult BankCash()
        {
            var deposits = db.BankDeposit.Where(x => x.StationId == Library.ChannelId).ToList();
            return View(deposits);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BankCashDetails(BankDeposit depo, HttpPostedFileBase BankSlip)
        {
            //check whether there is an active shift
            Shift shift = db.Shifts.FirstOrDefault(x => x.IsActive == true);
            if (shift != null)
            {
                string filePath = "";
                if (BankSlip.ContentLength > 0)
                {
                    string extension = Path.GetExtension(BankSlip.FileName);
                    string fileName = DateTime.Now.ToString("ddMMyyHHmmss").ToString() + extension;
                    string _FileName = Path.GetFileName(BankSlip.FileName);
                    filePath = Path.Combine(Server.MapPath("~/UploadedDocuments"), fileName );
                    BankSlip.SaveAs(filePath);
                }  


                BankDeposit dep = new BankDeposit();
                dep.Amount = depo.Amount;
                dep.BankAccountId = depo.BankAccountId;
                dep.DepositedBy = depo.DepositedBy;
                dep.Reference = depo.Reference;
                dep.Notes = depo.Notes;
                dep.ShiftId = shift.Id;
                dep.StaffId = Library.UserId;
                dep.BankedOn = DateTime.Now;
                dep.StationId = Library.ChannelId;
                dep.SlipPath = filePath;
                db.BankDeposit.Add(dep);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    SetWarning(e);
                }
                

                Session["success"] = "Bank deposit processed created";
            }
            else
            {
                Session["warning"] = "You cannot make cash deposit without opening a shift";
            }
            return RedirectToAction("BankCash");
        }


       
        //function to begin fuel receipt
        [HttpGet]
        public ActionResult BeginFuelReceipt()
        {
            var transactions =
                db.InventoryPurchase.Where(x => x.StationId == Library.ChannelId && x.IsCompleted == false &&
                                                x.IsCanceled == false);
            if (transactions.Any())
            {
                Session["warning"] =
                    "You must complete the ongoing offload or mark it as complete to start a new one";
                return RedirectToAction("StationHome");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BeginFuelReceiptStore(FormCollection collection)
        {
            //here we check whether there is an active shift
            var shift = db.Shifts.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsActive == true);
            if (shift != null)
            {
                //at this point we check whether there is an active fueltransaction going on if yes we tell the user to close or cancel it
                var transactions =
                    db.InventoryPurchase.Where(x => x.StationId == Library.ChannelId && x.IsCompleted == false &&
                                                    x.IsCanceled == false);
                if (!transactions.Any())
                {
                    //get financetransaction type where name is equal to purchase
                    var transactionType =
                        db.FinanceTransactionTypes.FirstOrDefault(x => x.FinanceTransTypeName == Constants.Purchase);
                    //if every thing goes well, save the details
                    InvPurchase purchase = new InvPurchase();
                    purchase.StationId = Library.ChannelId;
                    purchase.StaffId = Library.UserId;
                    purchase.DateCreated = DateTime.Now;
                    purchase.FinaceTransId = transactionType.Id;
                    purchase.ModifiedBy = Library.UserId;
                    purchase.ShiftId = shift.Id;
                    purchase.IsCompleted = false;
                    purchase.IsCanceled = false;

                    db.InventoryPurchase.Add(purchase);

                    //now we store some details in the fuel receipt table

                    FuelReceipt receipt = new FuelReceipt();
                    receipt.TankId = Int64.Parse(collection["TankId"]);
                    receipt.InvPurchaseId = purchase.Id;
                    receipt.HeightBefore = double.Parse(collection["HeightBefore"]);
                    receipt.DesityBefore = double.Parse(collection["DesityBefore"]);
                    receipt.TemperatureBefore = double.Parse(collection["TemperatureBefore"]);
                    receipt.VolumeBefore = double.Parse(collection["VolumeBefore"]);

                    db.FuelReceipt.Add(receipt);
                    db.SaveChanges();
                    Session["success"] = "Fuel Receipt started";


                }
                else
                {
                    Session["warning"] =
                        "You must complete the ongoing offload or mark it as complete to start a new one";
                }
            }
            else
            {
                Session["warning"] = "You must open a shift before starting a fuel offload";
            }

            return RedirectToAction("StationHome");
        }

        public ActionResult CompleteFuelReceipt()
        {
            // get the id of the active transaction
            var activeOffload = db.InventoryPurchase.Where(
                x => x.StationId == Library.ChannelId && x.IsCompleted == false && x.IsCanceled == false);
            if (!activeOffload.Any())
            {
                Session["warning"] = "There must be an active fuel receipt to close";
                return RedirectToAction("StationHome");
            }

            var invPurchase = activeOffload.First();
            //using the active purchase id, we get the the fuel receipt details
            var fuelReceipt = db.FuelReceipt.FirstOrDefault(x => x.InvPurchaseId == invPurchase.Id);
            ViewBag.FuelReceipt = fuelReceipt;
            return View();
        }

        [HttpPost]
        public JsonResult CompleteFuelReceiptStore(RecieveFuelWizardModels wizard)
        {
            if (wizard.FuelReceiptId != default(long))
            {
                try
                {
                //
                var receiptId = wizard.FuelReceiptId;
                //first we get  details of the ongoing offload
                var receiptDetails = db.FuelReceipt.Find(receiptId);
                //we also get details of the inventory purchase using the inv id 
                var invPurchase = db.InventoryPurchase.Find(wizard.InvPurchaseId);
                invPurchase.SupplierId = wizard.SupplierId;
                invPurchase.TotalAmount = wizard.TotalAmount;
//                db.InventoryPurchase.Add(invPurchase);
//                db.SaveChanges();

                //we then fill fuel receipt details
                receiptDetails.DriverId = wizard.DriverId;
                receiptDetails.VehicleId = wizard.VehicleId;
                receiptDetails.DocumentNumber = wizard.DocumentNumber;
                receiptDetails.VolumeOnWeyBill = wizard.VolumeOnWeyBill;
                receiptDetails.TemperatureAfter = wizard.TemperatureAfter;
                receiptDetails.DensityAfter = wizard.DensityAfter;
                receiptDetails.SampleTemperature = wizard.SampleTemperature;
                receiptDetails.SampleDensity = wizard.SampleDensity;
                receiptDetails.VolDispensedDuring = wizard.VolDispensedDuring;
                receiptDetails.VolumeAfter = wizard.VolumeAfter;
                receiptDetails.HeightAfter = wizard.HeightAfter;
                receiptDetails.PricePerLiter = wizard.PricePerLiter;
//                receiptDetails.ShiftId = invPurchase.ShiftId;

                //record tank reading
                TankReading tankReading = new TankReading
                {
                    TankId = receiptDetails.TankId,
                    Volume = wizard.VolumeAfter,
                    Height = wizard.HeightAfter,
                    Temperature = wizard.TemperatureAfter,
                    Density = wizard.DensityAfter,
                    DateCreated = DateTime.Now,
                    ActualDateCreated = DateTime.Now
                };
                db.TankReadings.AddOrUpdate(tankReading);

//                db.FuelReceipt.Add(receiptDetails);
                //get finace transaction type
                var transTypeId =
                    db.FinanceTransactionTypes.FirstOrDefault(x => x.FinanceTransTypeName == Constants.Purchase);
                //create a finance transaction
               /* FinanceTransaction finance = new FinanceTransaction
                {
                    FinanceTransDesc = "Fuel Purchase",
                    FinanceTransCode = Constants.Purchase,
                    DateCreated = DateTime.Now,
                    StationId = Library.ChannelId,
                    FinanceTransactionTypeId = transTypeId.Id,
                    ReceiptNumber = collection["DocumentNumber"],
                    TransactionActualDate = DateTime.Now,
                    ShiftId = invPurchase.ShiftId,
                    StaffId = Library.UserId
                };
                db.FinanceTransactions.Add(finance);

                //create a ticket
                Ticket ticket = new Ticket
                {
                    StaffId = Library.UserId,
                    DateCreated = DateTime.Now,
                    FinanceTransactionId = finance.Id,
                    Discount = 0,
                    StationId = Library.ChannelId,
                    ModifiedBy = Library.UserId
                };
                db.Tickets.Add(ticket);

                //get the product id
                var product = db.StationTanks.Find(receiptDetails.TankId);
                //create a ticket line
                TicketLine line= new TicketLine
                {
                    TicketId =  ticket.Id,
                    ProductId = product.ProductId,
                    Price = double.Parse(collection["PricePerLiter"]),
                    ModifiedBy = Library.UserId
                };
                db.TicketLines.Add(line);*/

                //create inventory purchase line
                var product = db.StationTanks.Find(receiptDetails.TankId);
                InvPurchaseLine pLine = new InvPurchaseLine
                {
                    InvPurchaseId = invPurchase.Id,
                    ProductId = product.Id,
                    Price = wizard.PricePerLiter,
                    Discount = 0,
                    ModifiedBy = Library.UserId
                };
                db.InvPurchaseLine.AddOrUpdate(pLine);
                
                    
                    invPurchase.IsCompleted = true;
                    db.Entry(invPurchase).State = EntityState.Modified;
//                    db.Entry(pLine).State = EntityState.Modified;
                    db.Entry(receiptDetails).State = EntityState.Modified;

                    db.SaveChanges();

                }
                catch(Exception e)
                {
                    Library.Log(e.Message, e.StackTrace);
                    return Json(e);
                }
                //commit changes

//                Session["success"] = "Fuel Receipt completed";

            }
            return Json("success");
        }

        public JsonResult GetFuelDetails(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);
            var drivers = db.Drivers.Where(x => x.SupplierId == id && x.IsDeleted == false);
            var vehicles = db.Vehicles.Where(x => x.SupplierId == id && x.IsDeleted == false);

            return Json(new {drivers, vehicles});
        }

        public ActionResult CreateSummary(long id)
        {
            //using the receipt id get the reception detasils
            FuelReceipt receipt = db.FuelReceipt.Find(id);

            //then using the receip details get the inventory details
            InvPurchase purchase = db.InventoryPurchase.Find(receipt.InvPurchaseId);

            ViewBag.ReceipDetails = receipt;
            ViewBag.InventoryDetails = purchase;

            return View();
        }

        public ActionResult CloseShiftView()
        {
            // check whether there is an active shift to close
            var shift = db.Shifts.FirstOrDefault(x => x.IsActive && x.StationId == Library.ChannelId);
            if (shift != null)
            {
                ViewBag.ShiftId = shift.Id;
                return View();
            }
            Session["warning"] = "There is no active shift to close";
            return RedirectToAction("StationHome");
        }
      

        public JsonResult CloseShitDetails(FormCollection collection)
        {
            object status = new { }; 
            //here check to make sure a shift has been started
            var shift = db.Shifts.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsActive == true);
            if (shift != null)
            {
                //Store the record as a finance transaction
                FinanceTransaction transaction = new FinanceTransaction();
                transaction.FinanceTransDesc = "Fuel Sale Close Shift";
                transaction.FinanceTransCode = Constants.Sale;
                transaction.DateCreated = DateTime.Now;
                transaction.StationId = Library.ChannelId;
                transaction.FinanceTransactionTypeId = db.FinanceTransactionTypes
                    .FirstOrDefault(x => x.FinanceTransTypeName == Constants.Sale)
                    .Id;
                transaction.TransactionActualDate = DateTime.Now;
                transaction.ShiftId = shift.Id;
                transaction.StaffId = Library.UserId;
                db.FinanceTransactions.Add(transaction);

                //at this point save the ticket
                Ticket ticket = new Ticket();
                ticket.StaffId = Library.UserId;
                ticket.DateCreated = DateTime.Now;
                ticket.DashBoardDate = StartOfDay(DateTime.Now);
                ticket.StationId = Library.ChannelId;
                ticket.ModifiedBy = Library.UserId;
                ticket.FinanceTransactionId = transaction.Id;
                db.Tickets.Add(ticket);


                var count = (collection.AllKeys.Length / 5);

                for (int i = 0; i < count; i++)
                {
                    var nozId = Int64.Parse(collection["CloseDetails[" + i + "][NozzleId]"]);
                    

                    //here we first get the id of the readings we want to update
                    var readingId = Int64.Parse(collection["CloseDetails[" + i + "][ReadingId]"]);
                    var nozzleReading = db.NozzleReadings.Find(readingId);
                    nozzleReading.EndManualReading = Int64.Parse(collection["CloseDetails[" + i + "][EndManualReading]"]);
                    nozzleReading.EndElectronicReading = Int64.Parse(collection["CloseDetails[" + i + "][EndElectronicReading]"]);
                   

                    //add item ticket line
                    TicketLine line = new TicketLine();
                    line.Discount = 0;
                    line.ModifiedBy = Library.UserId;
                    line.ProductId = db.PumpNozzles.Find(nozId).ProductId;
                    line.TicketId = ticket.Id;
                    line.Price = nozzleReading.NozzlePricePerLiter;
                    line.Units = Int64.Parse(collection["CloseDetails[" + i + "][EndElectronicReading]"]) -
                                 nozzleReading.StartElectronicReading;
                    db.TicketLines.Add(line);


                    //at this point we store nozzle tests if any test was done in between the shift hours
                    if (collection["CloseDetails[" + i + "][TestData]"] != "")
                    {
                        NozzleTest test = new NozzleTest();
                        test.ShiftId = shift.Id;
                        test.NozzleId = nozId;
                        test.Liters = double.Parse(collection["CloseDetails[" + i + "][TestData]"]);
                        test.DateCreated = DateTime.Now;
                        test.ActuelDateCreated = DateTime.Now;
                        db.NozzleTests.Add(test);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Library.Log(e.Message,e.StackTrace);
                    }
                   
                }
                return Json("success");
//                db.SaveChanges();
//                Session["success"] = "Opening details recorded";
                
            }
            return Json("failed");
        }
        [HttpPost]
        public JsonResult CloseShift(AllCloseDetailsModels shiftCloseDetails)
        {
            object status = new { status = "failed" }; 
            //here check to make sure a shift has been started
            var shift = db.Shifts.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsActive == true);
            if (shift != null)
            {
                //Store the record as a finance transaction
                FinanceTransaction transaction = new FinanceTransaction
                {
                    FinanceTransDesc = "Fuel Sale Close Shift",
                    FinanceTransCode = Constants.Sale,
                    DateCreated = DateTime.Now,
                    StationId = Library.ChannelId,
                    FinanceTransactionTypeId = db.FinanceTransactionTypes
                        .FirstOrDefault(x => x.FinanceTransTypeName == Constants.Sale)
                        .Id,
                    TransactionActualDate = DateTime.Now,
                    ShiftId = shift.Id,
                    StaffId = Library.UserId

                };
                //to make sure we do not record a transaction two times, we do as below
                db.FinanceTransactions.Add(transaction);

                //at this point save the ticket
                Ticket ticket = new Ticket
                {
                    StaffId = Library.UserId,
                    DateCreated = DateTime.Now,
                    DashBoardDate = StartOfDay(DateTime.Now),
                    StationId = Library.ChannelId,
                    ModifiedBy = Library.UserId,
                    FinanceTransactionId = transaction.Id
                };
                db.Tickets.AddOrUpdate(x=>x.FinanceTransactionId,ticket);

                if (shiftCloseDetails.CloseDetails.Any())
                {
                    foreach (var detail in shiftCloseDetails.CloseDetails)
                    {
                        var nozzleReading = db.NozzleReadings.Find(detail.ReadingId);
                        nozzleReading.EndElectronicReading = detail.EndElectronicReading;
                        nozzleReading.EndManualReading = detail.EndManualReading;

                        //add item ticket line
                        TicketLine line = new TicketLine();
                        line.Discount = 0;
                        line.ModifiedBy = Library.UserId;
                        line.ProductId = db.PumpNozzles.Find(detail.NozzleId).ProductId;
                        line.TicketId = ticket.Id;
                        line.Price = nozzleReading.NozzlePricePerLiter;
                        line.Units = detail.EndElectronicReading -
                                     nozzleReading.StartElectronicReading;
                        db.TicketLines.Add(line);

                        //at this point we store nozzle tests if any test was done in between the shift hours
                        if (detail.TestData != default(double))
                        {
                            NozzleTest test = new NozzleTest();
                            test.ShiftId = shift.Id;
                            test.NozzleId = detail.NozzleId;
                            test.Liters = detail.TestData;
                            test.DateCreated = DateTime.Now;
                            test.ActuelDateCreated = DateTime.Now;
                            db.NozzleTests.Add(test);
                        }

                    }

                    if (shiftCloseDetails.CashDrop.Any())
                    {
                        foreach (var drop in shiftCloseDetails.CashDrop)
                        {
                            CashDrop cDrop = new CashDrop
                            {
                                StaffId = drop.StaffId,
                                Amount = drop.Amount,
                                PaymentModeId = drop.PaymentMode,
                                ShiftId = shift.Id,
                                StationId = Library.ChannelId,
                                DateCreated = DateTime.Now
                            };
                            db.CashDrops.Add(cDrop);
                        }
                    }
                    try
                    {
                        var closeShift = db.Shifts.Find(shift.Id);
                        closeShift.ShiftEndTime = DateTime.Now;
                        closeShift.IsActive = false;
                        db.SaveChanges();
                        status = new {status = "success"};
                        Session["success"] = "Shift details have been recorded";
                    }
                    catch (Exception e)
                    {
                        Library.Log(e.Message, e.StackTrace);
                        status = new {status = "failed", message = e.Message};
                    }
                }
                else
                {
                    status = new {status = "failed", message = "No open shift"};
                }
            }
            return Json(status);
        }

        public JsonResult RecordTankReadings(FormCollection collection)
        {
            var count = (collection.AllKeys.Length / 5);

            for (int i = 0; i < count; i++)
            {
                var tankId = Int64.Parse(collection["Readings[" + i + "][TankId]"]);
                var height = double.Parse(collection["Readings[" + i + "][Height]"]);
                TankReading reading = new TankReading();
                reading.TankId = tankId;
                reading.Height = height;
                reading.DateCreated = DateTime.Now;
                reading.ActualDateCreated = DateTime.Now;
                db.TankReadings.Add(reading);

            }
            try
            {
                db.SaveChanges();
                return Json("success");
            }
            catch (Exception e)
            {
                Library.Log(e.Message, e.StackTrace);
                return Json(e);

            }

        }

        public JsonResult CloseShiftCashDrops(FormCollection collection)
        {
            var count = (collection.AllKeys.Length / 3);
            try
            {
            var shift = db.Shifts.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsActive == true);
            for (int i = 0; i < count; i++)
            {
//                if (collection["Cashdrop[" + i + "][Amount]"] != "")
//                {
                    var staffId = Int64.Parse(collection["Cashdrop[" + i + "][StaffId]"]);
                    var paymentMode = Int64.Parse(collection["Cashdrop[" + i + "][paymentMode]"]);
                    var amount = double.Parse(collection["Cashdrop[" + i + "][Amount]"]);
                CashDrop cDrop = new CashDrop
                {
                    StaffId = staffId,
                    Amount = amount,
                    PaymentModeId = paymentMode,
                    ShiftId = shift.Id,
                    StationId = Library.ChannelId,
                    DateCreated = DateTime.Now
                };
                db.CashDrops.Add(cDrop);
            }
            
                var closeShift = db.Shifts.Find(shift.Id);
                closeShift.ShiftEndTime = DateTime.Now;
                closeShift.IsActive = false;
                db.SaveChanges();
                return Json("success");
            }
            catch (Exception e)
            {
                Library.Log(e.Message, e.StackTrace);
                return Json(e);

            }
        }
        public DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public JsonResult GetProduct(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);

            StationTanks tank = db.StationTanks.Find(id);

            return Json(tank);

        }

        [HttpGet]
        public ActionResult StartShift()
        {
            //check whether there is an active shift
            var shift = db.Shifts.Where(x => x.StationId == Library.ChannelId && x.IsActive == true);
            if (shift.Any())
            {
                return RedirectToAction("StationHome");
            }
            return View();
        }

        [HttpPost]
        public JsonResult OpenShiftDetails(FormCollection collection)
        {
            //here we begin the shift
            List<object> status = new List<object>();
            List<string> failed = new List<string>();

            //here check to make sure a shift has been started
            var activeShift = db.Shifts.Where(x => x.StationId == Library.ChannelId && x.IsActive == true);
            if (!activeShift.Any())
            {
                Shift newShift = new Shift
                {
                    ShiftStartTime = DateTime.Parse(collection["shiftDetails[0][StartTime]"]),
                    ShiftCategoryId = Int64.Parse(collection["shiftDetails[0][ShiftId]"]),
                    StationId = Library.ChannelId,
                    IsActive = true
                };
                db.Shifts.Add(newShift);
                //                var results = .Length;
                var count = (collection.AllKeys.Length / 7);

                for (int i = 0; i < count; i++)
                {
                    var nozId = Int64.Parse(collection["shiftDetails[" + i + "][NozzleId]"]);
                    //                    if (!db.NozzleReadings.Any(x => x.ShiftId == shift.Id && x.NozzleId == nozId))
                    //                    {
                    //here get the product price
                    try
                    {
                        var nozzeldetails = db.PumpNozzles.Find(nozId);
                        //get default price list
                        var dp = db.PriceList.FirstOrDefault(x => x.IsDefault);
                        var productPrice =
                            db.ProductStationPrices.FirstOrDefault(
                                x => x.StationId == Library.ChannelId && x.ProductId == nozzeldetails.ProductId &&
                                     x.ProductPriceListId == dp.Id);
                        NozzleReading reading = new NozzleReading();
                        reading.ShiftId = newShift.Id;
                        reading.DateCreated = DateTime.Now;
                        reading.NozzleId = Int64.Parse(collection["shiftDetails[" + i + "][NozzleId]"]);
                        reading.StartManualReading = Int64.Parse(collection["shiftDetails[" + i + "][ManualReading]"]);
                        reading.PumpId = Int64.Parse(collection["shiftDetails[" + i + "][PumpId]"]);
                        reading.StartElectronicReading =
                            Int64.Parse(collection["shiftDetails[" + i + "][ElectronicReading]"]);
                        reading.StaffId = Int64.Parse(collection["shiftDetails[" + i + "][StaffId]"]);
                        reading.NozzlePricePerLiter = productPrice.ProductPrice;
                        db.NozzleReadings.Add(reading);
                    }
                    catch (Exception e)
                    {
                        Library.Log(e.Message, e.StackTrace);
                        return Json(new { status = "failed", message = "You must set station prices for each product before opening a shift, please contact admmin" });
                    }

                    //                    }
                }

                try
                {
                    db.SaveChanges();
                    Session["success"] = "Opening details recorded";
                    return Json(new { status = "success", message = "openning details have been recorded" });
                }
                catch (Exception e)
                {
                    Library.Log(e.Message, e.StackTrace);
                    return Json(new { status = "failed", message = e.Message });
                }

            }
            return Json(new { status = "failed", message = "something went wrong" });
        }
        [HttpGet]
        public ActionResult RecieveFuelWizard(long? id)
        {
            if (id != null)
            {
                ViewBag.fId = id;
                return View();
            }
            //check whether there is an active shift
            var shift = db.Shifts.FirstOrDefault(x => x.IsActive == true && x.StationId == Library.ChannelId);
            if (shift == null)
            {
                Session["warning"] = "You must open a shift first before recieving fuel";
                return RedirectToAction("GetFuelReciepts");
            }
            return View();
        }
        [HttpPost]
        public JsonResult RecieveFuelWizardStore(RecieveFuelWizardModels wizard)
        {
            object status = new object();
            var shift = db.Shifts.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsActive == true);
            if (shift != null)
            {
                //at this point we check whether there is an active fueltransaction going on if yes we tell the user to close or cancel it
                var transactions =
                    db.InventoryPurchase.Where(x => x.StationId == Library.ChannelId && x.IsCompleted == false &&
                                                    x.IsCanceled == false);
                if (!transactions.Any())
                {
                    try
                    {
                    //get financetransaction type where name is equal to purchase
                    var transactionType =
                        db.FinanceTransactionTypes.FirstOrDefault(x => x.FinanceTransTypeName == Constants.Purchase);
                    //if every thing goes well, save the details
                    InvPurchase purchase = new InvPurchase();
                    purchase.StationId = Library.ChannelId;
                    purchase.StaffId = Library.UserId;
                    purchase.DateCreated = DateTime.Now;
                    purchase.FinaceTransId = transactionType.Id;
                    purchase.ModifiedBy = Library.UserId;
                    purchase.ShiftId = shift.Id;
                    purchase.IsCompleted = false;
                    purchase.IsCanceled = false;

                    db.InventoryPurchase.Add(purchase);

                    //now we store some details in the fuel receipt table

                    FuelReceipt receipt = new FuelReceipt();
                    receipt.TankId = wizard.TankId;
                    receipt.InvPurchaseId = purchase.Id;
                    receipt.HeightBefore = wizard.HeightBefore;
                    receipt.DesityBefore = wizard.DesityBefore;
                    receipt.TemperatureBefore = wizard.TemperatureBefore;
                    receipt.VolumeBefore = wizard.VolumeBefore;

                    db.FuelReceipt.Add(receipt);
                    
                        db.SaveChanges();
                        status = new { status = "success", message = "Fuel details recorded" };
                    }
                    catch (Exception e)
                    {
                        Library.Log(e.Message,e.StackTrace);
                        status = new { status = "failed", message = e.Message };
                    }
                }
                
            }
            else
            {
                Session["warning"] = "You must be in a shift to proceed with fuel offload";
                status = new { status = "no-shift", message = "You must be in a shift to proceed" };
            }

            return Json(status);
        }

        public JsonResult PopulateWizardData(FormCollection collection)
        {
            long collectionId = default(long);
            if (collection["id"] == "")
            {
                var transactions =
                    db.InventoryPurchase.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsCompleted == false && x.IsCanceled == false);
                if (transactions != null)
                {
                    collectionId = transactions.Id;
                }
            }
            else
            {
                collectionId = Int64.Parse(collection["id"]);
            }
          //check whether there is an active transaction
//            var transactions =
//                db.InventoryPurchase.FirstOrDefault(x => x.StationId == Library.ChannelId && x.IsCompleted == false && x.IsCanceled == false);


            if (collectionId != default(long))
            {
                try
                {
                    var fuelReceiptDetails = db.FuelReceipt.FirstOrDefault(x => x.InvPurchaseId == collectionId);

                    var wizardDetails = new RecieveFuelWizardModels()
                    {
                        TankId = fuelReceiptDetails.TankId,
                        InvPurchaseId = collectionId,
                        FuelReceiptId = fuelReceiptDetails.Id,
                        HeightBefore = fuelReceiptDetails.HeightBefore,
                        HeightAfter = fuelReceiptDetails.HeightAfter ?? default(double),
                        VolumeBefore = fuelReceiptDetails.VolumeBefore,
                        VolumeAfter = fuelReceiptDetails.VolumeAfter ?? default(double),
                        TemperatureBefore = fuelReceiptDetails.TemperatureBefore,
                        TemperatureAfter = fuelReceiptDetails.TemperatureAfter ?? default(double),
                        DesityBefore = fuelReceiptDetails.DesityBefore,
                        DensityAfter = fuelReceiptDetails.DensityAfter ?? default(double),
                        DriverId = fuelReceiptDetails.DriverId,
                        VehicleId = fuelReceiptDetails.VehicleId,
                        SupplierId = fuelReceiptDetails.InvPurchase.SupplierId,
                        SampleTemperature = fuelReceiptDetails.SampleTemperature,
                        DocumentNumber = fuelReceiptDetails.DocumentNumber,
                        VolumeOnWeyBill = fuelReceiptDetails.VolumeOnWeyBill,
                        SampleDensity = fuelReceiptDetails.SampleDensity,
                        VolDispensedDuring = fuelReceiptDetails.VolDispensedDuring,
                        TotalAmount = fuelReceiptDetails.InvPurchase.TotalAmount
                    };

                    return Json(new {details = wizardDetails});
                }
                catch (Exception e)
                {
                    Library.Log(e.Message,e.StackTrace);
                }
               
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult CompleteFuelReceiptStore2(RecieveFuelWizardModels wizard)
        {
            //inventory purchase
            //fuel reciept
            //record inventory purchase lines
            //record tank readings
            //mark transaction as complete

            if (wizard.FuelReceiptId != default(long))
            {
                try
                {
                //get the reciept details
                FuelReceipt reciept = db.FuelReceipt.Find(wizard.FuelReceiptId);
                reciept.DensityAfter = wizard.DensityAfter;
                reciept.DriverId = wizard.DriverId;
                reciept.SampleTemperature = wizard.SampleTemperature;
                reciept.SampleDensity = wizard.SampleDensity;
                reciept.HeightAfter = wizard.HeightAfter;
                reciept.TemperatureAfter = wizard.TemperatureAfter;
                reciept.VolumeAfter = wizard.VolumeAfter;
                reciept.VolDispensedDuring = wizard.VolDispensedDuring;
                reciept.VehicleId = wizard.VehicleId;
                reciept.DocumentNumber = wizard.DocumentNumber;
                reciept.VolumeOnWeyBill = wizard.VolumeOnWeyBill;
                reciept.PricePerLiter = wizard.PricePerLiter;
                db.Entry(reciept).State = EntityState.Modified;
                
                //record inventory purchase line
                var product = db.StationTanks.Find(wizard.TankId);
                InvPurchaseLine pLine = new InvPurchaseLine
                {
                    InvPurchaseId = wizard.InvPurchaseId,
                    ProductId = product.ProductId,
                    Price = wizard.PricePerLiter,
                    Discount = 0,
                    ModifiedBy = Library.UserId
                };
                db.InvPurchaseLine.AddOrUpdate(x=>x.InvPurchaseId,pLine);

                //record tank readings
                TankReading tankReading = new TankReading
                {
                    TankId = wizard.TankId,
                    Volume = wizard.VolumeAfter,
                    Height = wizard.HeightAfter,
                    Temperature = wizard.TemperatureAfter,
                    Density = wizard.DensityAfter,
                    DateCreated = DateTime.Now,
                    ActualDateCreated = DateTime.Now,
                    ReferenceId = wizard.InvPurchaseId,
                    ReferenceDescription = Constants.FuelRecieption
                };
                db.TankReadings.AddOrUpdate(x=>x.ReferenceId,tankReading);

                //mark transaction as complete
                InvPurchase invPurchase = db.InventoryPurchase.Find(wizard.InvPurchaseId);
                invPurchase.SupplierId = wizard.SupplierId;
                invPurchase.TotalAmount = wizard.TotalAmount;
                invPurchase.IsCompleted = true;
               
                    db.SaveChanges();
                    return Json(new{ status = "success", message = "Fuel details recorded"});
                }
                catch (Exception e)
                {
                    Library.Log(e.Message, e.StackTrace);
                    return Json(new {status = "failed",message = e.Message});
                }
               
            }
            return Json(new { status= "failed", message = "something went wrong"});
        }

        public ActionResult GetFuelReciepts()
        {
            List<RecieveFuelWizardModels> recieps = new List<RecieveFuelWizardModels>();
            // check the active
            return View(db.FuelReceipt.Where(x=>x.InvPurchase.StationId == Library.ChannelId).OrderByDescending(x=>x.Id).Take(10));
        }

        public JsonResult GetTotalExpectedForEachStaff(ExpectedCashModels closeDetails)
        {
            List<object> expected = new List<object>();
            if (closeDetails.ClosingPumpDetails.Any())
            {
                
                var shift = db.Shifts.FirstOrDefault(x => x.IsActive == true && x.StationId == Library.ChannelId);
                if (shift != default(Shift))
                {
                    var nozzleReadings = db.NozzleReadings.Where(x => x.ShiftId == shift.Id);
                    var staffs = nozzleReadings.Select(x => x.StaffId).Distinct();
                    if (staffs.Any())
                    {
                        foreach (var staff in staffs)
                        {
                            double totalExpected = 0;
                            var readings = nozzleReadings.Where(x => x.StaffId == staff);
                            foreach (var reading in readings)
                            {
                                //get the record from close shift filled values
                                var rec = closeDetails.ClosingPumpDetails.FirstOrDefault(x => x.ReadingId == reading.Id);
                                //now subtract the closing electronic reading from the database opening reading in the database to get the amount sold
                                //and multiply the resulting value by the cash per liter
                                var amountSold = (rec.EndElectronicReading - reading.StartElectronicReading) * reading.NozzlePricePerLiter;

                                //add the resulting value to the total sum expected
                                totalExpected += amountSold;
                            }
                            expected.Add(new { staffId = staff, amount = totalExpected});
                        }
                    }
                }
                
            }
            return Json(expected);
        }

        public ActionResult GenerateCloseShiftReport(long? id)
        {
            ViewBag.readingId = id;

            return View();
        }

        private void SetWarning(Exception e)
        {
            Library.Log(e.Message, e.StackTrace);
            Session["warning"] = "Something went wrong, please contact admin";
        }
        //model class for shift summary
        public class  ShiftSummaryModels
        {
            public long ShiftId { get; set; }
            public string ShiftName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }
        public ActionResult ShiftSummary()
        {
            List<ShiftSummaryModels> summary = new List<ShiftSummaryModels>();
            var readings = db.NozzleReadings;
            var shifts = readings.Select(x => x.ShiftId).Distinct();
            if (shifts.Any())
            {
                foreach (var shift in shifts)
                {
                    var reading = readings.FirstOrDefault(x => x.ShiftId == shift);
                    ShiftSummaryModels sum = new ShiftSummaryModels
                    {
                        ShiftId = reading.ShiftId,
                        ShiftName = reading.Shift.ShiftCategory.ShiftName,
                        StartTime = String.Format("{0:g}", reading.Shift.ShiftStartTime), // "Sunday, March 09, 2008 4:05 PM",
                        EndTime = String.Format("{0:g}", reading.Shift.ShiftEndTime)
                    };
                    summary.Add(sum);
                }
            }
            return View(summary.OrderByDescending(x=>x.ShiftId).Take(10));
        }
    }
}