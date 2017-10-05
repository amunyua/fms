using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationLinerMVC.Models
{
    public class Station
    {
        [Key]
        public long Id { get; set; }
        [DisplayName("Station Name"), Required]

        public string StationName { get; set; }
        [DisplayName("Station Description")]
        public string StationDesc { get; set; }
        [DisplayName("Station Category")]
        [ForeignKey("StationCategory")]
        public long StationCategoryId { get; set; }
        [ForeignKey("Country")]
        public long CountryId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int TanksCount { get; set; }
        public int PumpsCount { get; set; }


        public long ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Country Country { get; set; }
        public virtual StationCategory StationCategory { get; set; }

    }

    public class StationTanks
    {
        public long Id { get; set; }
        [ForeignKey("Station")]
        public long StationId { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public string TankName { get; set; }
        public string TankDesc { get; set; }
        public string CalibrationChart { get; set; }
        public double Length { get; set; }
        public double Diameter { get; set; }
        public double Volume { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }
        public virtual Station Station { get; set; }
    }

    public class PumpModel
    {
        public long Id { get; set; }
        public string Model { get; set; }
    }

    public class StationPump
    {
        public long Id { get; set; }
        [ForeignKey("Station")]
        public long StationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDoubleSided { get; set; }
        [ForeignKey("PumpModel")]
        public long ModelId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Station Station { get; set; }
        public virtual PumpModel PumpModel { get; set; }

    }

    public class PumpNozzle
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [ForeignKey("StationPump")]
        public long PumpId { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        [ForeignKey("StationTank")]
        public long TankId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Side { get; set; }

        public virtual StationPump StationPump { get; set; }
        public virtual StationTanks StationTank { get; set; }
        public virtual Product Product { get; set; }
        
    }

    public class StationShift
    {
        public long Id { get; set; }
        [ForeignKey("Station")]
        public long StationId { get; set; }
        [ForeignKey("ShiftCategory")]
        public long ShiftCategoryId { get; set; }

        public long ModifiedBy { get; set; }

        public virtual Station Station { get; set; }
        public virtual ShiftCategory ShiftCategory { get; set; }


    }
}