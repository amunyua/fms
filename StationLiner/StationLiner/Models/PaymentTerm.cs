using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class PaymentTerm
    {
        public long Id { get; set; }
        public string PaymentTermName { get; set; }
        public int DayInFollowingMonth { get; set; }
        public int DaysBeforeDue { get; set; }
    }
}