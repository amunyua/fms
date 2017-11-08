using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationLinerMVC.Models
{
   public class Menu
    {
        [Key]
        public long MenuId { get; set; }
       
        [Display(Name = "Menu Name"), Required]
        public String MenuName { get; set; }
        [Display(Name = "Parent Menu")]
//        [ForeignKey("Menus")]
        public long? ParentMenu { get; set; }
        [Required]
        public String Controller { get; set; }
        [Required]
        public String Action { get; set; }
        //public string IdAttribute { get; set; }
        public string Icon { get; set; }
        public int? Sequence { get; set; }
        [DefaultValue(true)]
        public bool Status { get; set; }

        public string LayoutBase { get; set; }

        public bool AdminBased { get; set; }
//        [Required]
//        public virtual Menu Menus { get; set; }
    }
}
