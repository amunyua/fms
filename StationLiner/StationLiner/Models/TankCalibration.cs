using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StationLinerMVC.Models
{
    public class CalibrationChart
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Tank")]
        public long TankId { get; set; }
        public double Height { get; set; }
        public double Volume { get; set; }

        public virtual StationTanks Tank { get; set; }
    }
}