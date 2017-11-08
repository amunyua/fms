using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationLinerMVC.Models
{
    public class UOM
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Name")]
        [Required]
        public string UOMName { get; set; }
        [DisplayName("Description")]
        public string UOMDesc { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
