using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StationLinerMVC.Models
{

    public class ProductAttributeType
    {
        public long Id {get; set;}
        public string ProductAttributeTypeName {get; set;}
    }

    public class ProductAttribute
    {
        public long Id {get; set;}
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        [ForeignKey("ProductAttributeType")]
        public long ProductAttributeTypeId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductAttributeType ProductAttributeType { get; set; }
    }

    public class ProductAttributeValue
    {
        public long Id {get; set;}
        [ForeignKey("ProductAttributeType")]
        public long ProductAttributeTypeId { get; set; }
        public long value {get; set;}

        public virtual ProductAttributeType ProductAttributeType { get; set; }
    }
    
    
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [Remote("IsProductsExists","Products",ErrorMessage="Product Already exists")]
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        [Required]
        [DisplayName("Product Category")]
        [ForeignKey("ProductCategory")]
        public long ProductCategoryId { get; set; }
        public string ProductNumber { get; set; }
        [ForeignKey("UOM")]
        public long? UOMId { get; set; }
        public string BarCode { get; set; }
        public double? Cost { get; set; }
        public int? MaxShelfLife { get; set; }
        public string Notes { get; set; }
        [Required]
        [DisplayName("Tax Category")]
        [ForeignKey("TaxCategory")]
        public long TaxCategoryId { get; set; }
        public long ModifiedBy { get; set; }
        public bool? HasAttribute {get; set;}
        public bool IsActive { get; set; }
        public DateTime DateCreated {get; set;}
        public bool IsDeleted { get; set; }


        public virtual UOM UOM { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual TaxCategory TaxCategory { get; set; }
    }


    public class ProductCategory
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string ProductCategoryName { get; set; }
        public string ProductCategoryDesc { get; set; }
        public bool IsActive {get; set;}
        public long ModifiedBy {get; set;}
        public DateTime DateCreated {get; set;}
    }
}
