using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.Owin.Security;

namespace StationLinerMVC.Models
{
    public class Tax
    {
        [Key]
        public long Id { get; set; }
        [Required, DisplayName("Tax Name")]
        public string TaxName { get; set; }

        [ForeignKey("TaxCategory")]
        public long CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        [ForeignKey("TaxRate")]
        public long? TaxRateId { get; set; }
        [ForeignKey("CustomerTaxCategory")]
        public long? CustCatId { get; set; }
        public long CreatedBy { get; set; }
        public long CreatedAt { get; set; }

        public virtual TaxCategory TaxCategory { get; set; }
        public virtual TaxRate TaxRate { get; set; }
        public virtual CustomerTaxCategory CustomerTaxCategory { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }

    public class TaxCategory
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Category Name"),Required]
        public string CategoryName { get; set; }
        [DisplayName("Category Description")]
        public string CategoryDescription { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }
    }

    public class TaxRate
    {
        [Key]
        public long Id { get; set; }

        public string TaxRateName { get; set; }

        public double? TaxRateValue { get; set; }

        public string TaxRateDescription { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CustomerTaxCategory
    {
        [Key]
        public long Id { get; set; }

        public string CustCatName { get; set; }

        public string CustCatDescription { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }
    }
}
