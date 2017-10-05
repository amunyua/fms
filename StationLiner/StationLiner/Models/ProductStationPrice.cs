using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class ProductStationPrice
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        [ForeignKey("Station")]
        public long StationId { get; set; }
        [ForeignKey("PriceList")]
        public long ProductPriceListId { get; set; }
        public double ProductPrice { get; set; }
//        [ForeignKey("Currency")]
//        public long ProductCurrencyId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public long ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Product Product { get; set; }
        public virtual Station Station { get; set; }
        public virtual PriceList PriceList { get; set; }
//        public virtual Currency Currency { get; set; }
    }
}