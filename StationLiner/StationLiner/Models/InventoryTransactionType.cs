﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class InventoryTransactionType
    {
        public long Id { get; set; }
        [Required]
        public string InventroryTransactionType { get; set; }

        public string InventoryTransactionTypeDesc { get; set; }
    }
}