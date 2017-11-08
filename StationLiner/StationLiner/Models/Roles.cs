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
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public long RoleId { get; set; }
        [Required]
        [DisplayName("Role Name")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }


        public string Description { get; set; }
        public long ModifiedBy { get; set; }
    }
}
