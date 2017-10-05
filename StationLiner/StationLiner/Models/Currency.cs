using System;
using System.ComponentModel.DataAnnotations;

namespace StationLinerMVC.Models
{
    public class Currency
    {
        [Key]
        public long Id { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencySymbol { get; set; }

        public double? RatioToBase { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}