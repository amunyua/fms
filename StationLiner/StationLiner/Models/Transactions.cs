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
 

    public class Transactions
    {
        [Key]
        public long Id { get; set; }

        public string TransactionType { get; set; }
        public string TransactionCategory { get; set; }
        public DateTime? TransactionDate { get; set; }

        public bool Status { get; set; }
        public long ReceivedBy { get; set; }

        public string DeliveryNote { get; set; }
        public string InvoiceRefNumber { get; set; }

        [DisplayName("Supplier"), Required]
        [ForeignKey("Supplier")]
        public long SupplierId { get; set; }
        [ DisplayName("Driver")]
//        [ForeignKey("SupplierDriver")]
        public long DriverId { get; set; }
        [DisplayName("Vehicle")]
        public long VehicleId { get; set; }


        public virtual Supplier Supplier { get; set; }
    }

    public class TransactionDetails
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Transaction")]
        public long TransactionId { get; set; }

        [DisplayName("Warehouse"), Required]
        [ForeignKey("Warehouse")]
        public long WarehouseId { get; set; }

        [Required, DisplayName("Product")]
        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public double? Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Transactions Transaction { get; set; }

    }

    public class TransactionDocuments
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Transaction")]
        public long TransactionId { get; set; }

        public string DocumentType { get; set; }

        public string DocumentPath { get; set; }

        public virtual Transactions Transaction { get; set; }
    }

    public class FuelTransactionDetails
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Transaction")]
        public long TransactionId { get; set; }
        public double ExpectedQuantity { get; set; }
        public double DipBeforeOffload { get; set; }
        public double DipAfterOffload { get; set; }
        public double AmountSoldDuringOffload { get; set; }
        public double ActualQuantityAvailable { get; set; }
        public double PricePerLiter { get; set; }
        public virtual Transactions Transaction { get; set; }

    }
    
}
