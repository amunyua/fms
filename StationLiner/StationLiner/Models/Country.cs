using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class Country
    {
        [Key]
        public long Id { get; set; }

        public string CountryName { get; set; }
    }
}