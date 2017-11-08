using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationLinerMVC.Models
{
    public class ShitfDetails //What is this for??
    {

    }

    public class ActiveShift
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Shift")]
        public long ShiftId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime ShiftDate { get; set; }
        [ForeignKey("Station")]
        public long Channel { get; set; }

        public long CreatedBy { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public virtual Station Station { get; set; }
        public virtual Shift Shift { get; set; }


    }

    public class ShiftNozzleReadings
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("ActiveShift")]
        public long ActiveShiftId { get; set; }

        public long NozzleId { get; set; }

        public double NozzleReadings { get; set; }

        public virtual ActiveShift ActiveShift { get; set; }
    }

    public class ShiftNozzleAttendant
    {
        [Key]
        public long Id { get; set; }

    }
}