using System;
using System.ComponentModel.DataAnnotations;

namespace StationLinerMVC.Models
{
    public class StationCategory
    {
        [Key]
        public long Id { get; set; }
        public string StationCategoryName { get; set; }
        public string SatationCategoryDesc { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }
}