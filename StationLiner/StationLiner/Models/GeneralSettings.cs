using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationLinerMVC.Models
{
    public class GeneralSettings
    {
        [Key]
        public long Id { get; set; }

        public string CompanyName { get; set; } 
        public string VAT { get; set; }
        public string Location { get; set; }
        public string PinNumber { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public bool IsDeleted { get; set; }
    }
}
