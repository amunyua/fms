using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class PriceList
    {
        [Key]
        public long Id { get; set; }
        public string PriceListName { get; set; }
        public string PriceListDesc { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public DateTime DateCreated { get; set; }
        public long ModifiedBy { get; set; }
    }
}