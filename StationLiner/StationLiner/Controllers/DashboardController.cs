using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.DataHandler.Serializer;
using MoreLinq;
using Newtonsoft.Json.Schema;
using StationLinerMVC.Models;
using WebGrease.Css.Extensions;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeMode(string mode, long? channelId)
        {
            var userId = Library.UserId;
            var users = db.UserLayout.Where(x => x.UserId == userId);
            if (users.Any())
            {
                Library.ChannelId = channelId;
                users.First().Mode = mode;
                users.First().ChannelId = channelId;
                db.SaveChanges();
            }
            else
            {
                Library.ChannelId = channelId;
                db.UserLayout.Add(new UserLayout() {UserId = userId, Mode = mode, ChannelId = channelId});
                db.SaveChanges();
            }
            if (mode == Constants.AdminMode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("StationDashboard");
            }

//            return Redirect(Request.UrlReferrer.ToString());

        }





        public JsonResult LoadSalesData(FormCollection collection)
        {
            //get the first day of the current month
            var today = DateTime.Now;
            var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
            var tickets = db.Database.SqlQuery<SalesData>(
                "select tkl.ProductId,tkt.StationId,DashBoardDate,p.ProductName, sum(tkl.Price*tkl.Units) as totalsold from TicketLines as tkl Left join Tickets as tkt on tkl.TicketId=tkt.Id Left join Products as p on tkl.ProductId = p.Id group by tkl.ProductId,StationID,DashBoardDate,p.ProductName");
            var stations = collection["Station"];
            var period = collection["period"];

            List<int> labels = new List<int>();
            List<object> dataSets = new List<object>();
            string title = "";
            if (stations == "all")
            {

                switch (period)
                {
                    case "month-to-date":
                        title = "Month to date";
                        MonthToDate(labels, tickets.ToList(), dataSets);
                        break;

                    case "quater-to-date":
                        title = "Quater to date";
                        QuaterToDate(labels, tickets.ToList(), dataSets);
                        break;
                    case "year-to-date":
                        title = "Year to date";
                        YearToDate(labels, tickets.ToList(), dataSets);
                        break;
                }

            }
            else
            {
                var stationId = Int64.Parse(collection["Station"]);
                switch (period)
                {
                    case "month-to-date":
                        title = "Month to date";
                        MonthToDate(labels, tickets.Where(x => x.StationId == stationId).ToList(), dataSets);
                        break;
                    case "year-to-date":
                        title = "Year to date";
                        YearToDate(labels, tickets.Where(x => x.StationId == stationId).ToList(), dataSets);
                        break;
                    case "quater-to-date":
                        title = "Quater to date";
                        QuaterToDate(labels, tickets.Where(x => x.StationId == stationId).ToList(), dataSets);
                        break;
                }
            }





            return Json(new {labels = labels, datasets = dataSets, chartTitle = title});
//            return Json(dataSets);
        }

        public object MonthToDate(List<int> labels, List<SalesData> tickets, List<object> dataSets)
        {
            var today = DateTime.Now;
            var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
            var numberOfDays = (today - startOfthisMonth).Days;
            for (int i = numberOfDays; i >= 0; i--)
            {
                var actualDate = today.AddDays(-i);
                labels.Add(actualDate.Day);

            }
            var products = tickets.Select(x => new {x.ProductId, x.ProductName}).DistinctBy(x => x.ProductId);

            List<double> totalForEachDay = new List<double>();

            foreach (var product in products)
            {
                var pd = new Record();
                double total = 0;

                List<double> amountForThisDay = new List<double>();
                for (int i = numberOfDays; i >= 0; i--)
                {
                    var actualDate = today.AddDays(-i);
                    var startofday = StartOfDay(actualDate);
                    var sum = tickets.Where(x => x.ProductId == product.ProductId && x.DashBoardDate == startofday)
                        .Select(x => x.TotalSold)
                        .Sum();
                    amountForThisDay.Add(sum);
                    total = total + sum;
                    totalForEachDay.Add(total);
                    //                    dataSets.Add(sum); 

                }
                pd.ProductName = product.ProductName;
                pd.AmountForEachDate = amountForThisDay;
                dataSets.Add(new
                {
                    name = product.ProductName,

                   
                    data = amountForThisDay
                });

            }
//            dataSets.Add(new
//            {
//                label = "Total Sales",
//                data = totalForEachDay
//            });

            return new {labels, dataSets};
        }

        public object QuaterToDate(List<int> labels, List<SalesData> tickets, List<object> dataSets)
        {
            int currentQuarter = GetQuarter();
            for (var i = 0; i <= currentQuarter; i++)
            {
                labels.Add(i);
            }

            var products = tickets.Select(x => new {x.ProductId, x.ProductName}).DistinctBy(x => x.ProductId);

            List<double> totalForEachDay = new List<double>();

            foreach (var product in products)
            {
                var pd = new Record();
                double total = 0;

                List<double> amountForThisDay = new List<double>();
                int a = 1;
                int b = 3;
                amountForThisDay.Add(0);
                for (var i = 0; i <= currentQuarter; i++)
                {

                    var sum = tickets.Where(x => x.ProductId == product.ProductId && x.DashBoardDate.Month >= a &&
                                                 x.DashBoardDate.Month <= b)
                        .Select(x => x.TotalSold)
                        .Sum();
                    amountForThisDay.Add(sum);
                    total = total + sum;
                    totalForEachDay.Add(total);
                    a = a + 3;
                    b = b + 3;
                }
                pd.ProductName = product.ProductName;
                pd.AmountForEachDate = amountForThisDay;
                dataSets.Add(new
                {
                    name = product.ProductName,
                    data = amountForThisDay
                });

            }
            /* dataSets.Add(new
             {
                 label = "Total Sales",
                 data = totalForEachDay
             });*/
            return new {labels, dataSets};

        }

        public object YearToDate(List<int> labels, List<SalesData> tickets, List<object> dataSets)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            var numberOfMonths = (DateTime.Now.Month - firstDay.Month);
            var a = numberOfMonths + 1;
            for (var i = 0; i < a; i++)
            {
                labels.Add(i);
            }

            var products = tickets.Select(x => new {x.ProductId, x.ProductName}).DistinctBy(x => x.ProductId);

            List<double> totalForEachDay = new List<double>();

            foreach (var product in products)
            {
                var pd = new Record();
                double total = 0;

                List<double> amountForThisDay = new List<double>();
                for (int i = numberOfMonths; i >= 0; i--)
                {
                    var actualDate = DateTime.Today.AddMonths(-i);
//                    dataSets.Add(new {month = actualDate.Month});
//                    var startofday = StartOfDay(actualDate);
                    var sum = tickets.Where(x => x.ProductId == product.ProductId &&
                                                 x.DashBoardDate.Month == actualDate.Month)
                        .Select(x => x.TotalSold)
                        .Sum();
                    amountForThisDay.Add(sum);
                    total = total + sum;
                    totalForEachDay.Add(total);
                    //                    dataSets.Add(sum); 

                }
                pd.ProductName = product.ProductName;
                pd.AmountForEachDate = amountForThisDay;
                dataSets.Add(new
                {
                    name = product.ProductName,
                    data = amountForThisDay
                });

            }
            /* dataSets.Add(new
             {
                 label = "Total Sales",
                 data = totalForEachDay
             });*/

            return new {labels, dataSets};

        }


        public int GetQuarter()
        {
            return (DateTime.Now.Month + 2) / 3;
        }




        [HttpPost]
        public JsonResult GetTankLevels(FormCollection collection)
        {
            List<object> tankReadings = new List<object>();
            var tanks = db.TankReadings.Select(x => x.TankId).Distinct();
            if (tanks.Any())
            {

                foreach (var tankId in tanks)
                {
                    var detail = db.TankReadings.Include(x => x.Tank)
                        .Where(x => x.TankId == tankId)
                        .MaxBy(x => x.DateCreated);
                    tankReadings.Add(detail);
                }
            }

            return Json(tankReadings);
        }

        public JsonResult LeastPerforming(FormCollection collection)
        {
            int take = int.Parse(collection["filterBy"]);
            var today = DateTime.Now;
            var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
            List<PerformingStations> ps = new List<PerformingStations>();
            switch (collection["range"])
            {
                case "month-to-date":
                    ps = AnalysePerformance(db.Tickets.Where(x => x.DateCreated >= startOfthisMonth).ToList());
                    break;
                case "quater-to-date":
                    int currentQuarter = GetQuarter();
                    int a;
                    int b;
                    if (currentQuarter == 1)
                    {
                        a = 1;
                    }
                    else if (currentQuarter == 2)
                    {
                        a = 4;
                    }
                    else if (currentQuarter == 3)
                    {
                        a = 7;
                    }
                    else
                    {
                        a = 10;
                    }
                    ps = AnalysePerformance(db.Tickets.Where(x => x.DateCreated.Month >= a).ToList());
                    break;
                case "year-to-date":
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);
                    ps = AnalysePerformance(db.Tickets.Where(x => x.DateCreated >= firstDay).ToList());
                    break;
            }

            return Json(ps.OrderBy(x => x.TotalForSingleStation).Take(take));
        }

        public JsonResult BestPerfomingStation(FormCollection collection)
        {
            int take = int.Parse(collection["filterBy"]);
            var today = DateTime.Now;
            var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
            List<PerformingStations> ps = new List<PerformingStations>();
            switch (collection["range"])
            {
                case "month-to-date":
                    ps = AnalysePerformance(db.Tickets.Where(x => x.DateCreated >= startOfthisMonth).ToList());
                    break;
                case "quater-to-date":
                    int currentQuarter = GetQuarter();
                    int a;
                    int b;
                    if (currentQuarter == 1)
                    {
                        a = 1;
                    }
                    else if (currentQuarter == 2)
                    {
                        a = 4;
                    }
                    else if (currentQuarter == 3)
                    {
                        a = 7;
                    }
                    else
                    {
                        a = 10;
                    }
                    ps = AnalysePerformance(db.Tickets.Where(x => x.DateCreated.Month >= a).ToList());
                    break;
                case "year-to-date":
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);
                    ps = AnalysePerformance(db.Tickets.Where(x => x.DateCreated >= firstDay).ToList());
                    break;
            }

            return Json(ps.OrderByDescending(x => x.TotalForSingleStation).Take(take));
        }

        public List<PerformingStations> AnalysePerformance(List<Ticket> totalTickets)
        {
            double total = 0;
            //            List<object> dataSet = new List<object>();
            List<PerformingStations> stationsArray = new List<PerformingStations>();
            
            

            if (totalTickets.Any())
            {
                //               //get the total amount for all tickets fro this month
                foreach (var ticket in totalTickets)
                {
                    var ticketLines = db.TicketLines.Where(x => x.TicketId == ticket.Id);
                    if (ticketLines.Any())
                    {
                        foreach (var line in ticketLines)
                        {
                            var amount = line.Price * line.Units;
                            total = total + amount;
                        }

                    }
                }

                //here we  start classifying them station by station

                var stations = totalTickets.Select(x => x.StationId).Distinct();
                if (stations.Any())
                {

                    foreach (var station in stations)
                    {
                        double singleStationTotal = 0;

                        var singleRecords = db.Tickets.Where(x => x.StationId == station);

                        foreach (var record in singleRecords)
                        {
                            var ticketLineDetails = db.TicketLines.Where(x => x.TicketId == record.Id);
                            if (ticketLineDetails.Any())
                            {
                                foreach (var detail in ticketLineDetails)
                                {
                                    singleStationTotal = singleStationTotal + (detail.Price * detail.Units);
                                }
                            }
                        }
                        var contribution = (singleStationTotal / total) * 100;
                        var stationName = db.Stations.Find(station).StationName;
                        //                        stationsArray.Add(new { StationName = stationName, totalForSingleStation = string.Format("{0:n}", singleStationTotal), totalForAllStations = total, contribution = string.Format("{0:n}", contribution) });
                        PerformingStations st = new PerformingStations
                        {
                            StationName = stationName,
                            TotalForSingleStation = singleStationTotal,
                            TotalForSingleStationFormated = string.Format("{0:n}", singleStationTotal),
                            Contribution = string.Format("{0:n}", contribution),
                            TotalForAllStations = total
                        };
                        stationsArray.Add(st);
                    }

                }

            }
            return stationsArray;
        }


        public DateTime EndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public DateTime StartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public ActionResult StationDashboard()
        {
            ViewBag.stationId = Library.ChannelId;
            return View();
        }

        public JsonResult LoadStationSalesData(FormCollection collection)
        {
            //get the first day of the current month

            var tickets = db.Database.SqlQuery<SalesData>(
                    "select tkl.ProductId,tkt.StationId,DashBoardDate,p.ProductName, sum(tkl.Price*tkl.Units) as totalsold from TicketLines as tkl Left join Tickets as tkt on tkl.TicketId=tkt.Id Left join Products as p on tkl.ProductId = p.Id group by tkl.ProductId,StationID,DashBoardDate,p.ProductName")
                .ToList();
            var today = DateTime.Now;
            var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
            List<int> labels = new List<int>();
            List<object> dataSets = new List<object>();

            var numberOfDays = (today - startOfthisMonth).Days;
            for (int i = numberOfDays; i >= 0; i--)
            {
                var actualDate = today.AddDays(-i);
                labels.Add(actualDate.Day);

            }
            var products = tickets.Select(x => new {x.ProductId, x.ProductName}).DistinctBy(x => x.ProductId);

            List<double> totalForEachDay = new List<double>();

            foreach (var product in products)
            {
                var pd = new Record();
                double total = 0;

                List<double> amountForThisDay = new List<double>();
                for (int i = numberOfDays; i >= 0; i--)
                {

                    var actualDate = today.AddDays(-i);
                    var startofday = StartOfDay(actualDate);
                    var sum = tickets.Where(x => x.ProductId == product.ProductId && x.DashBoardDate == startofday &&
                                                 x.StationId == Library.ChannelId)
                        .Select(x => x.TotalSold)
                        .Sum();
                    amountForThisDay.Add(sum);
                    total = total + sum;
                    totalForEachDay.Add(total);
                    //                    dataSets.Add(sum); 

                }
                pd.ProductName = product.ProductName;
                pd.AmountForEachDate = amountForThisDay;
                dataSets.Add(new
                {
                    label = product.ProductName,
                    data = amountForThisDay
                });

            }
            dataSets.Add(new
            {
                label = "Total Sales",
                data = totalForEachDay
            });

            return Json(new {labels = labels, datasets = dataSets});
        }

        public JsonResult StationPaymentModes()
        {
            double allmodestotal = 0;
            var paymentModes = db.PaymentModes;
            var cashDrops = db.CashDrops;
            List<PaymentModesViewModels> totalAmountPerMode = new List<PaymentModesViewModels>();
            if (paymentModes.Any())
            {

                foreach (var mode in paymentModes)
                {

                    double totalForMode = 0;
                    if (Library.ChannelId != null)
                    {

                        totalForMode = cashDrops
                            .Where(x => x.PaymentModeId == mode.Id && x.StationId == Library.ChannelId)
                            .Select(x => x.Amount)
                            .Sum();
                        allmodestotal = allmodestotal + totalForMode;
                    }
                    totalAmountPerMode.Add(new PaymentModesViewModels
                    {
                        Mode = mode.PaymentModeName,
                        Amount = totalForMode
                    });

                }
            }
            return Json(new {modes = totalAmountPerMode, total = allmodestotal});
        }


        public JsonResult LoadAttendantCashDrop(FormCollection collection)
        {
            var period = collection["period"];
            var performance2 = db.Database
                .SqlQuery<AttendantCashDrops>(
                    "select c.Id,c.StaffId,c.ShiftId,c.Amount,CONVERT(date,c.DateCreated) as DropDate,PaymentModeId,StationId, s.FullName from CashDrops c Left join Staff s on c.staffId = s.Id")
                .Where(x => x.StationId == Library.ChannelId);
            List<AttendantCashDrops> performance = new List<AttendantCashDrops>();
            switch (period){
                case "month-to-date":
                    var today = DateTime.Now;
                    var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
                    performance = performance2.Where(x => x.DropDate > startOfthisMonth).ToList();
                    break;

                case "quater-to-date":
                    int currentQuarter = GetQuarter();
                    int a;
                    int b;
                    if (currentQuarter == 1)
                    {
                        a = 1;
                    }
                    else if (currentQuarter == 2)
                    {
                        a = 4;
                    }
                    else if (currentQuarter == 3)
                    {
                        a = 7;
                    }
                    else
                    {
                        a = 10;
                    }
                    performance = performance2.Where(x => x.DropDate.Month > a).ToList();
                    break;
                case "year-to-date":
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);
                    performance = performance2.Where(x => x.DropDate > firstDay).ToList();
                        
                    break;
            }
            
            List<string> labels = new List<string>();
            List<object> dataSets = new List<object>();

            if (performance.Any())
            {
                var staffNames = performance.Select(x => x.FullName).Distinct();

                if (staffNames.Any())
                {
                    List<double> amount = new List<double>();
                    foreach (var name in staffNames)
                    {

                        labels.Add(name);
                        double totalAmount = performance.Where(x => x.FullName == name).Sum(x => x.Amount);
                        amount.Add(totalAmount);
                    }
                    dataSets.Add(new {label = "Attendant Cash Drops", data = amount});

                }
            }
            return Json(new {labels = labels, datasets = dataSets});
        }



        public JsonResult LoadAttendantPerformance()
        {
            var today = DateTime.Now;
            var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
            List<string> labels = new List<string>();
            List<object> dataset = new List<object>();
            List<string> productName = new List<string>();

            var performances = db.Database.SqlQuery<Performances>(
                    "select s.FullName,p.ProductName, t.StationId, sum(tl.Units* Price) as totalsold from Tickets t left join TicketLines tl on tl.TicketId = t.Id Left join Staff s on s.Id = t.StaffId Left join Products p on p.Id = tl.ProductId where t.DateCreated>='" +
                    startOfthisMonth + "' group by fullname,productname,stationID")
                .Where(x => x.StationId == Library.ChannelId);

            if (performances.Any())
            {
                var attendants = performances.Select(x => x.FullName).Distinct();
                foreach (var attendant in attendants)
                {
                    labels.Add(attendant);
                    var products = performances.Where(x => x.FullName == attendant);

                }

            }
            return Json(labels);
        }

        public JsonResult PumpSummary()
        {
            List<object> details = new List<object>();
            var results = db.Database.SqlQuery<PumpSummary>(
                    "select sp.Name as PumpName, p.ProductName,nr.ShiftId,nr.EndManualReading,nr.EndElectronicReading,nr.DateCreated,nr.PumpId, (nr.EndElectronicReading-nr.StartElectronicReading) as LitersSold,sp.StationId,(nr.EndElectronicReading - nr.EndManualReading) as DifferenceR from NozzleReadings nr left join StationPumps sp on sp.id = nr.PumpId left join PumpNozzles pn on pn.Id = nr.NozzleId left join Products p on p.id = pn.ProductId")
                .Where(x => x.StationId == Library.ChannelId);
            if (results.Any())
            {
                var id = results.Select(x => x.ShiftId).Max();
                var resultset = results.Where(x => x.ShiftId == id);
                var distintPumps = resultset.Select(x => x.PumpName).Distinct();
                foreach (var pump in distintPumps)
                {
                    var pumpdetail = resultset.Where(x => x.PumpName == pump);
                    details.Add(new {PumpName = pump, nozzles = pumpdetail});
                }
            }
            return Json(details);
        }

        public JsonResult StationFuelLevels(FormCollection collection)
        {
            List<object> obj = new List<object>();
            List<string> labels = new List<string>();
            List<double> data = new List<double>();
            long? stationId = Library.ChannelId;
            if (stationId == null)
            {
                stationId = Int64.Parse(collection["Station"]);
            }
           
            var results =
                db.Database.SqlQuery<StationTanksViewModel>(
                        "select tr.* , st.StationId, st.TankName from TankReadings tr left join StationTanks st on st.id = tr.TankId")
                    .Where(x => x.StationId == stationId);
            if (results.Any())
            {
                var distinctTankId = results.Select(x => x.TankId).Distinct();
                if (distinctTankId.Any())
                {
                    foreach (var id in distinctTankId)
                    {
                        var recentRecord = results.Where(x => x.TankId == id).MaxBy(x => x.Id);
                        labels.Add(recentRecord.TankName);
                        data.Add(recentRecord.Volume);
                    }
                }
            }
            return Json(new {labels = labels, data = data});
        }
        public JsonResult AdminStationPaymentModes(FormCollection collection)
        {
            double allmodestotal = 0;
            var stations = collection["Station"];
            var period = collection["period"];
            
            List<PaymentModesViewModels> totalAmountPerMode = new List<PaymentModesViewModels>();
            var cashDrops = db.CashDrops;
            if (stations == "all")
            {

                switch (period)
                {
                    case "month-to-date":
                        var today = DateTime.Now;
                        var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
                    TimeToDatePaymenyModes(allmodestotal, totalAmountPerMode, cashDrops.Where(x => x.DateCreated >= startOfthisMonth).ToList());
                        break;

                    case "quater-to-date":
                        int currentQuarter = GetQuarter();
                        int a;
                        int b;
                        if (currentQuarter == 1)
                        {
                            a = 1;
                        }
                        else if (currentQuarter == 2)
                        {
                            a = 4;
                        }
                        else if (currentQuarter == 3)
                        {
                            a = 7;
                        }
                        else
                        {
                            a = 10;
                        }
                        TimeToDatePaymenyModes(allmodestotal, totalAmountPerMode, cashDrops.Where(x => x.DateCreated.Month >= a).ToList());
                        
                        break;
                    case "year-to-date":
                        int year = DateTime.Now.Year;
                        DateTime firstDay = new DateTime(year, 1, 1);
                        TimeToDatePaymenyModes(allmodestotal, totalAmountPerMode, cashDrops.Where(x => x.DateCreated >= firstDay).ToList());
                        break;
                }

            }
            else
            {
                var stationId = Int64.Parse(collection["Station"]);
                switch (period)
                {
                    case "month-to-date":
                        var today = DateTime.Now;
                        var startOfthisMonth = new DateTime(today.Year, today.Month, 1);
                        TimeToDatePaymenyModes(allmodestotal, totalAmountPerMode, cashDrops.Where(x => x.DateCreated >= startOfthisMonth && x.StationId == stationId).ToList());
                        break;
                    case "year-to-date":
                        int year = DateTime.Now.Year;
                        DateTime firstDay = new DateTime(year, 1, 1);
                        TimeToDatePaymenyModes(allmodestotal, totalAmountPerMode, cashDrops.Where(x => x.DateCreated >= firstDay && x.StationId == stationId).ToList());
                        break;
                    case "quater-to-date":
                        int currentQuarter = GetQuarter();
                        int a;
                        int b;
                        if (currentQuarter == 1)
                        {
                            a = 1;
                        }
                        else if (currentQuarter == 2)
                        {
                            a = 4;
                        }
                        else if (currentQuarter == 3)
                        {
                            a = 7;
                        }
                        else
                        {
                            a = 10;
                        }
                        TimeToDatePaymenyModes(allmodestotal, totalAmountPerMode, cashDrops.Where(x => x.DateCreated.Month >= a && x.StationId == stationId).ToList());
                        break;
                }
            }

            
            return Json(new { modes = totalAmountPerMode, total = totalAmountPerMode.Sum(x=>x.Amount) });
        }

        public object TimeToDatePaymenyModes(double allmodestotal, List<PaymentModesViewModels> totalAmountPerMode, List<CashDrop> cashDrops)
        {
           
            var paymentModes = db.PaymentModes;
            if (paymentModes.Any())
            {
                double totalForMode = 0;
                foreach (var mode in paymentModes)
                {   
                        var drops = cashDrops
                            .Where(x => x.PaymentModeId == mode.Id)
                            .Select(x => x.Amount);
                    if (drops.Any())
                    {
                        totalForMode = drops.Sum();
                        allmodestotal = allmodestotal + totalForMode;
                    }

                   
                    totalAmountPerMode.Add(new PaymentModesViewModels
                    {
                        Mode = mode.PaymentModeName,
                        Amount = totalForMode
                    });

                }
            }
            return new { totalAmountPerMode, allmodestotal };
        }

        public string DynamicColors(){
            
            Random random = new Random();
            var r = Math.Floor(random.NextDouble() * 255);
            var g = Math.Floor(random.NextDouble() * 255);
            var b = Math.Floor(random.NextDouble() * 255);
            return "rgb(" + r + "," + g + "," + b + ")";
            
        }

        public JsonResult FuelLevel(FormCollection collection)
        {
            long? stationId = Library.ChannelId;
            if (stationId == null)
            {
                stationId = Int64.Parse(collection["Station"]);
            }

            var results = db.Database.SqlQuery<FuelLevels>(
                    "select s.StationName,st.TankName,p.ProductName,t.Volume as CurrentVolume,st.Volume AS MaxVolume,st.StationId from TankReadings as t LEFT JOIN TankReadings as t2 on t.TankId = t2.TankId AND t.DateCreated < t2.DateCreated LEFT JOIN StationTanks st ON t.TankId = st.Id LEFT JOIN Stations s ON st.StationId = s.Id left join Products" +
                    " p on st.ProductId = p.Id where t2.TankId is NULL order by t.DateCreated desc").Where(x=>x.StationId == stationId)
                ;


            return Json(results);
        }
        
    }
}