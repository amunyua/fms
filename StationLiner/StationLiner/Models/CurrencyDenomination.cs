using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class CurrencyDenomination
    {
        [Key]
        public long Id { get; set; }

        public string CurrencyDenominationName { get; set; }

        [ForeignKey("Currency")]
        public long CurrencyId { get; set; }

        public string CurrencyDenominationValue { get; set; }

        public bool IsActive { get; set; }

        public long ModifiedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Currency Currency { get; set; }
    }
}