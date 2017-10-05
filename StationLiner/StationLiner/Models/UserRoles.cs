using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationLinerMVC.Models
{
   public class UserRoles
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Roles")]
        public long RoleId { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public long ModifiedBy { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual User User { get; set; }
        
    }
}
