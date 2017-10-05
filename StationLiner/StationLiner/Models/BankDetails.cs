using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StationLinerMVC.Models
{
    public class BankDetails
    {
        public long Id { get; set; }
        [Required, DisplayName("Bank Name")]
        public string Bank { get; set; }
        
        [Required,DisplayName("Account Number")]
        public string AccountNumber { get; set; }
        [Required,DisplayName("Account Name")]
        public string AccountName { get; set; }
        [Required]
        public string Branch { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


    }

    public class PaymentMode
    {
        [Key]
        public long Id { get; set; }

        public string PaymentModeName { get; set; }

        public Nullable<bool> IsPOSAccepted { get; set; }

        public Nullable<bool> IsActive { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}