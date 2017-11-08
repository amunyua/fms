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
    public class WarehouseType
    {
        public long Id { get; set; }
        [Required]
        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
    public class Warehouse
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Warehouse Name"), Required]
        public string WarehouseName { get; set; }
        [DisplayName("Warehouse Code")]
        public string WarehouseCode { get; set; }
        [DisplayName("Warehohuse location")]
        public string WarehouseLocation { get; set; }
        [DisplayName("Warehohuse description")]
        public string WarehouseDesc { get; set; }
        [DisplayName("Warehouse Type")]
        [ForeignKey("WarehouseType")]
        public long WarehouseTypeId { get; set; }
       
        [DisplayName("Maximum Capacity")]
        public double? MaximumCapacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public long ModifiedBy { get; set; }

        public virtual WarehouseType WarehouseType { get; set; }
     }

    public class WarehouseProduct
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Item ReoderLevel")]
        public double ReorderLevel { get; set; }
        [DisplayName("Available Stock")]
        public double AvailableStock { get; set; }
        [DisplayName("Maximum Quantity")]
        public double MaximumQuantity { get; set; }
        [ForeignKey("Warehouse")]
        public long WarehouseId { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        [ForeignKey("UOM")]
        public long UOMId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual UOM UOM { get; set; }
    }
}
