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
    public class Supplier
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Postal Address"),Required]
        public string PostalAddress { get; set; }
        [DisplayName("Postal Code"),Required]
        public string PostalCode { get; set; }

        public string City { get; set; }
        [ForeignKey("Country")]
        public long? CountryId { get; set; }
        [DisplayName("Contact One")]
        [Required]
        public string Contact1 { get; set; }

        [DisplayName("Contact Two")]
        public string Contact2 { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ContactOneName { get; set; }
        public string ContactOneTelephone { get; set; }
        public string ContactOneDesignation { get; set; }
        public string ContactOneEmail { get; set; }
        public string ContactTwoName { get; set; }
        public string ContactTwoDesignation { get; set; }
        public string ContactTwoTelephone { get; set; }
        public string ContactTwoEmail { get; set; }
        public string MOQ { get; set; }
        [DisplayName("Pin Number")]
        public string PinNumber { get; set; }
        [DisplayName("Contact person")]
        public string ContactPersonName { get; set; }
        [DisplayName("Credit Limit")]
        public double CreditLimit { get; set; }
        [ForeignKey("PaymentTerm")]
        public long? PaymentTerms { get; set; }
        public string Notes { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Country Country { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
    }

    public class SupplierProduct
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Supplier")]
        public long SupplierId { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        public bool IsActive {get; set;}
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Product Product { get; set; }
        public virtual Supplier Supplier{ get; set; }
    }

    public class SupplierProductPrice
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("SupplierProduct")]
        public long SupplierProductId { get; set; }
        public double MinQuantity { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate {get; set;}
        public double Price { get; set; }
        [ForeignKey("UOM")]
        public long UOMId { get; set; }
        public bool IsActive {get; set;}
        public DateTime DateCreated { get; set; }

        public virtual UOM UOM { get; set; }
        public virtual SupplierProduct SupplierProduct{ get; set; }
    }

    public class SupplierDriver
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Name")]
        public string DriverName { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [ForeignKey("Suppliers")]
        public long SupplierId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Supplier Suppliers { get; set; }
    }

    public class SupplierVehicle
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Registration Number")]
        public string RegNumber { get; set; }
        [DisplayName("Maximum Capacity")]
        public string MaximumCapacity { get; set; }
        [ForeignKey("Suppliers")]
        public long SupplierId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Supplier Suppliers { get; set; }
    }
}
