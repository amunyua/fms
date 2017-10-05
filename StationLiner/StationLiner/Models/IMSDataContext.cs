using Microsoft.AspNet.Identity.EntityFramework;
using StationLinerMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StationLinerMVC.Models
{
    public class IMSDataEntities : DbContext
    {

        public IMSDataEntities()

            : base("IMSDataEntities") //This is the name of the connection string which will be added to the Web.config later
        {

            //ConnectionTools db = new ConnectionTools();
            //db.ChangeDatabase(null, "FCSMW2", "", "", "", false);
            //DbConnection db = new DbConnection();
//            Database.SetInitializer(new StationLinerMVC.Migrations.Configuration()); //Added by Adam
        }
        public static IMSDataEntities Create()
        {
            return new IMSDataEntities();
        }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserRoleAllocation> UserRoleAllocations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CRUDAction> CrudActions { get; set; }
        public DbSet<UOM> Uom { get; set; }
        public DbSet<Supplier> Supppliers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductAttributeType> ProductAttributeTypes { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStationPrice> ProductStationPrices { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<WarehouseType> WarehouseTypes { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        public DbSet<SupplierProduct> SupplierProducts { get; set; }
        public DbSet<SupplierProductPrice> SupplierProductPrices { get; set; }
        public DbSet<SupplierDriver> Drivers { get; set; }
        public DbSet<SupplierVehicle> Vehicles { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionDetails> TransctionDetails { get; set; }
        public DbSet<TransactionDocuments> TransactionDocuments { get; set; }
        public DbSet<FuelTransactionDetails> FuelTransactionDetails { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<StationCategory> StationCategories { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyDenomination> CurrencyDenominations { get; set; }
        public DbSet<TaxCategory> TaxCategories { get; set; }
        public DbSet<TaxRate> TaxRates { get; set; }
        public DbSet<CustomerTaxCategory> CustomerTaxCategories { get; set; }
        public DbSet<Tax> Taxes { get; set; }
//        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftCategory> ShiftCategories { get; set; }
        public DbSet<CrudMenuActions> CrudMenuActions { get; set; }
        public DbSet<UserLayout> UserLayout { get; set; }
        public DbSet<UserAllocatedChannels> UserAllocatedChannels { get; set; }
        public DbSet<StationTanks> StationTanks { get; set; }
        public DbSet<PumpModel> PumpModels { get; set; }
        public DbSet<StationPump> StationPumps { get; set; }
        public DbSet<PumpNozzle> PumpNozzles { get; set; }
        public DbSet<StationShift> StationShifts { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<BankDetails> BankDetails { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CashDrop> CashDrops { get; set; }
        public DbSet<BankDeposit> BankDeposit { get; set; }
        public DbSet<FinanceTransactionType> FinanceTransactionTypes { get; set; }
        public DbSet<InvPurchase> InventoryPurchase { get; set; }
        public DbSet<FuelReceipt> FuelReceipt { get; set; }
        public DbSet<NozzleReading> NozzleReadings { get; set; }
        public DbSet<NozzleTest> NozzleTests { get; set; }
        public DbSet<FinanceTransaction> FinanceTransactions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketLine> TicketLines { get; set; }
        public DbSet<TaxLine> TaxLine { get; set; }
        public DbSet<TankReading> TankReadings { get; set; }
        public DbSet<InvPurchaseLine> InvPurchaseLine { get; set; }
        public DbSet<StationMenus> StationMenus { get; set; }
        public DbSet<StationRoles> StationRoles { get; set; }


    

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

//        public System.Data.Entity.DbSet<StationLinerMVC.Models.Product> Products { get; set; }
    }

    public class CustomUserRole : IdentityUserRole<long>
    {
        [Key]
        public long Id { get; set; }
    }
    public class CustomUserClaim : IdentityUserClaim<long> { }
    public class CustomUserLogin : IdentityUserLogin<long> { }

    public class CustomRole : IdentityRole<long, CustomUserRole>
    {
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public int? UserLevel { get; set; }

        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, long,
       CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(IMSDataEntities context)
            : base(context)
        {
        }
    }

}
