using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web.UI;

namespace StationLinerMVC.Models
{
    public class Shift
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public DateTime ShiftStartTime { get;set; }
        public DateTime? ShiftEndTime { get; set; }
        public long? StationId { get; set; }
        [Required]
        [ForeignKey("ShiftCategory")]
        public long ShiftCategoryId { get; set; }

        public bool IsActive { get; set; }

        public virtual Station Station { get; set; }
        public virtual ShiftCategory ShiftCategory { get; set; }

    }

   
    public class ShiftCategory
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string ShiftName { get; set; }

        public string ShiftDescription { get; set; }

        public DateTime StartTime { get ; set; }

        public DateTime EndTime { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }


    }
    public class CashDrop
    {
        public long Id { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public long StaffId { get; set; }
        [ForeignKey("Shifts")]
        public long ShiftId { get; set; }
        [ForeignKey("PaymentMode")]
        public long? PaymentModeId { get; set; }
        [Required]
        public double Amount { get; set; } 
        public DateTime DateCreated { get; set; }
        public DateTime? ActualDateCreated { get; set; }
//        [ForeignKey("Station")]
        public long? StationId { get; set; }

        public virtual Shift Shifts { get; set; }
        public virtual User UserId { get; set; }
        public virtual PaymentMode PaymentMode { get; set; }
//        public virtual Station Station { get; set; }
    }

   
    public class BankCheque
    {
        public string FileName;
    }
    public class BankDeposit
    {
        public long Id { get; set; }

        public long BankAccountId { get; set; }
        public double Amount { get; set; }
        public string Reference { get; set; }
        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        [ForeignKey("Staff")]
        public long StaffId { get; set; }
        public string DepositedBy { get; set; }
        public string Notes { get; set; }

        public DateTime BankedOn { get; set; }
        public long? StationId { get; set; }
        public string SlipPath { get; set; }

        public virtual Shift Shift { get; set; }
        public virtual User Staff { get; set; }
    }

    public class InvPurchase
    {
        [Key]
        public long Id { get; set; }
        public long StaffId { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("FinanceTransType")]
        public long FinaceTransId { get; set; }
        public double? Discount { get; set; }
        [ForeignKey("Station")]
        public long? StationId { get; set; }
        public long ModifiedBy { get; set; }
        public double? TotalAmount { get; set; }
        public long? SupplierId { get; set; }
        public long ShiftId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCanceled { get; set; }

        public virtual FinanceTransactionType FinanceTransType { get; set; }
        public virtual Station Station { get; set; }
    }

    public class InvPurchaseLine
    {
        public long Id { get; set; }
        [ForeignKey("InvPurchase")]
        public long InvPurchaseId { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public long ModifiedBy { get; set; }

        public virtual InvPurchase InvPurchase { get; set; }
        public virtual Product Product { get; set; }
    }

    public class FuelReceipt
    {
        public long Id { get; set; }
        [ForeignKey("Tank")]
        public long TankId { get; set; }
        [ForeignKey("InvPurchase")]
        public long InvPurchaseId { get; set; }
        [ForeignKey("Driver")]
        public long? DriverId { get; set; }
        [ForeignKey("Vehicle")]
        public long? VehicleId { get; set; }
        public double? SampleTemperature { get; set; }
        public double?SampleDensity { get; set; }
        public double? HeightBefore { get; set; }
        public double? HeightAfter { get; set; }
        public double? DesityBefore { get; set; }
        public double? DensityAfter { get; set; }
        public double? TemperatureBefore { get; set; }
        public double? TemperatureAfter { set; get; }
        public double? VolumeBefore { get; set; }
        public double? VolumeAfter { get; set; }
        public string Notes { get; set; }
        public double? VolDispensedDuring { get; set; }
        public string DocumentNumber { get; set; }
        public double? VolumeOnWeyBill { get; set; }
        public double? PricePerLiter { get; set; }
//        [ForeignKey("Shift")]
//        public long ShiftId { get; set; }
        public virtual StationTanks Tank { get; set; }
        public virtual InvPurchase InvPurchase { get; set; }
        public virtual SupplierDriver Driver { get; set; }
        public virtual SupplierVehicle Vehicle { get; set; }
//        public virtual Shift Shift { get; set; }

    }
    public class FinanceTransactionType
    {
        [Key]
        public long Id { get; set; }
        public string FinanceTransTypeName { get; set; }
    }

    public class NozzleReading
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Nozzle")]
        public long NozzleId { get;set; }
        public double NozzlePricePerLiter { get; set; }
        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        [ForeignKey("Users")]
        public long StaffId { get; set; }

        public long PumpId { get; set; }
        public double StartManualReading { get; set; }

        public double EndManualReading { get; set; }

        public double StartElectronicReading { get; set; }
        public double EndElectronicReading { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual PumpNozzle Nozzle { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual User Users { get; set; }
    }

    public class NozzleTest
    {
        public long Id { get; set; }
        [ForeignKey("Nozzle")]
        public long NozzleId { get; set; }
        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        public double Liters { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ActuelDateCreated { get; set; }

        public virtual PumpNozzle Nozzle { get; set; }
        public virtual Shift Shift { get; set; }

    }

    public class FinanceTransaction
    {
        public long Id { get; set; }

        public string FinanceTransDesc { get; set; }
        public string FinanceTransCode { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("Station")]
        public long? StationId { get; set; }
        [ForeignKey("TransType")]
        public long FinanceTransactionTypeId { get; set; }
        public long? FinanceTransParentId { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime TransactionActualDate { get; set; }
        public bool? IsPosted { get; set; }
        public long? DoctType { get; set; }
        [ForeignKey("Shift")]
        public long ShiftId { get; set; }
        [ForeignKey("Users")]
        public long StaffId { get; set; }

        public virtual Station Station { get; set; }
        public virtual FinanceTransactionType TransType { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual User Users { get; set; }
    }

    public class Ticket
    {
        public long Id { get; set; }

        public long? CustomerAccountId { get; set; }
        public long StaffId { get; set; }
        public DateTime DashBoardDate { get; set; }

        public DateTime DateCreated { get; set; }
        [ForeignKey("FinanceTransaction")]
        public long FinanceTransactionId { get; set; }
        public double Discount { get; set; }
        [ForeignKey("Station")]
        public long? StationId { get; set; }
        public double TotalAmount { get; set; }
        public long ModifiedBy { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerWEbUrl { get; set; }

        public virtual FinanceTransaction FinanceTransaction { get; set; }
        public virtual Station Station { get; set; }
    }

    public class TicketLine
    {
        public long Id { get; set; }
        [ForeignKey("Ticket")]
        public long TicketId { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        public double Price { get; set; }
        public double Units { get; set; }
        public double Discount { get; set; }
        public string DiscountReason { get; set; }
        public long ModifiedBy { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual Product Product { get; set; }

    }

    public class TaxLine
    {
        public long Id { get; set; }
        [ForeignKey("TicketLine")]
        public long TicketLineId { get; set; }
        [ForeignKey("Tax")]
        public long TaxId { get; set; }
        public double Base { get; set; }
        public double Amount { get; set; }

        public virtual TicketLine TicketLine { get; set; }
        public virtual Tax Tax { get; set; }
    }

    public class TankReading
    {
        public long Id { get; set; }
        [ForeignKey("Tank")]
        public long TankId { get; set; }
        public double Height { get; set; }
        public double Volume { get; set; }
        public double Temperature { get; set; }
        public double Density { get; set; }
        public long ReferenceId { get; set; }
        public string ReferenceDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ActualDateCreated { get; set; }

        public StationTanks Tank { get; set; }
    }

    public class RecieveFuelWizardModels
    {
        public long TankId { get; set; }
        public long InvPurchaseId { get; set; }
        public long FuelReceiptId { get; set; }
        public long? SupplierId { get; set; }
        public long? DriverId { get; set; }
        public long? VehicleId { get; set; }
        public double? SampleTemperature { get; set; }
        public double? SampleDensity { get; set; }
        public double? HeightBefore { get; set; }
        public double HeightAfter { get; set; }
        public double? DesityBefore { get; set; }
        public double DensityAfter { get; set; }
        public double? TemperatureBefore { get; set; }
        public double TemperatureAfter { set; get; }
        public double? VolumeBefore { get; set; }
        public double VolumeAfter { get; set; }
        public string Notes { get; set; }
        public double? VolDispensedDuring { get; set; }
        public string DocumentNumber { get; set; }
        public double? VolumeOnWeyBill { get; set; }
        public double PricePerLiter { get; set; }
        public double? TotalAmount { get; set; }
    }

    public class ClosePumpDetailsModel
    {
        public long NozzleId { get; set; }
        public double EndManualReading { get; set; }
        public double EndElectronicReading { get; set; }
        public double TestData { get; set; }
        public long ReadingId { get; set; }
    }

    public class CashDropModels
    {
        public long StaffId { get; set; }
        public long PaymentMode { get; set; }
        public double Amount { get; set; }
    }

    public class AllCloseDetailsModels
    {
        public List<ClosePumpDetailsModel> CloseDetails { get; set; }
        public List<CashDropModels> CashDrop { get; set; }
    }

    public class ExpectedCashModels
    {
        public List<ClosePumpDetailsModel> ClosingPumpDetails { get; set; }
    }
}