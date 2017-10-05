using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace StationLinerMVC.Models
{
    public class DashboardModels
    {

    }

    public class PerformingStations
    {
        public string StationName { get; set; }
        public double TotalForSingleStation { get; set; }
        public string TotalForSingleStationFormated { get; set; }
        public string Contribution { get; set; }
        public double TotalForAllStations { get; set; }
    }

    public class SalesData
    {
        public DateTime DashBoardDate { get; set; }
        public long StationId { get; set; }
        public long ProductId { get; set; }
        public double TotalSold { get; set; }
        public string ProductName { get; set; }

        public double Summary { get; set; }
    }

   

    public class Record
    {
        public string ProductName { get; set; }
        public List<double> AmountForEachDate { get; set; }
    }

    public class PaymentModesViewModels
    {
        public string Mode { get; set; }

        public double Amount { get; set; }
    }

    public class AttendantCashDrops
    {
        public long Id { get; set; }
        public long StaffId { get; set; }
        public string FullName { get; set; }
        public long ShiftId { get; set; }
        public double Amount { get; set; }
        public DateTime DropDate { get; set; }
        public long PaymentModeId { get; set; }
        public long StationId { get; set; }

    }

    public class AttendantPerformance
    {
        public long StaffId { get; set; }
        public string FullName { get; set; }
        public string ProductName { get; set; }
        public DateTime DateCreated { get; set; }
        public long StationId { get; set; }
        public long ProductId { get; set; }
        public double Price { get; set; }
        public double Units { get; set; }
    }

    public class Performances
    {
        public string FullName { get; set; }
        public string ProductName { get; set; }
        public long StationId { get; set; }
        public double TotalSold { get; set; }
    }

    public class PumpSummary
    {
        public string PumpName { get; set; }
        public string ProductName { get; set; }
        public long ShiftId { get; set; }
        public double EndManualReading { get; set; }
        public double EndElectronicReading { get; set; }
        public DateTime DateCreated { get; set; }
        public long PumpId { get; set; }
        public double LitersSold { get; set; }
        public long StationId { get; set; }
        public double DifferenceR { get; set; }
    }

    public class StationTanksViewModel
    {
        public long Id { get; set; }
        public long TankId { get; set; }
        public double Volume { get; set; }
        public DateTime DateCreated { get; set; }
        public long StationId { get; set; }
        public string TankName { get; set; }

    }
   
    
}