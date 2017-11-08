using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StationLinerMVC.Models
{
    [Table("Staff")]
    public class User : IUser<long>
    {

        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string NationalIdentification { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool UserAccount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        public long ModifiedBy { get; set; }
//        
    }

    public partial class StaffViewModels
    {
        public long Id { get; set; }
        [Required]
        [DisplayName("User Name")]
        [Remote("IsUserExists","Staffs",ErrorMessage="User Name already taken")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [DisplayName("National Id")]
        public string NationalIdentification { get; set; }
        [Required]
        public string Address { get; set; }
        [EmailAddress]
        [Required]
        [Remote("IsEmailExist","Staffs",ErrorMessage="Email already taken")]

        public string Email { get; set; }
        [DisplayName("Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
        [DisplayName("Create user account?")]
        public bool UserAccount { get; set; }
        public bool IsActive { get; set; }
        public long Role { get; set; }
    }

    public class UserCheck
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
    }



}
