using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class StationRoles
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("StationMenus")]
        public long StationMenuId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        
        public virtual User User { get; set; }

        public virtual StationMenus StationMenus { get; set; }


    }

    public class StationMenus
    {
        [Key]
        public long Id { get; set; }

        public string MenuName { get; set; }
    }
}