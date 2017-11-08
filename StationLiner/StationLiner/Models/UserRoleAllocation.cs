using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationLinerMVC.Models
{
    public class UserRoleAllocation
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Roles")]
        public long RoleId { get; set; }
        [ForeignKey("Menus")]
        public long MenuId { get; set; }
        public long? ParentId { get; set; }
        public string CrudActions { get; set; }
        
        public virtual Menu Menus { get; set; }
        
        public virtual Roles Roles { get; set; }
    }
}
