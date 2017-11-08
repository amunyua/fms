using System.Security.Policy;

namespace StationLinerMVC.Migrations
{
    using StationLinerMVC.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IMSDataEntities>
//    public class Configuration : System.Data.Entity.CreateDatabaseIfNotExists<IMSDataEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(IMSDataEntities context)
        {
            //create nozzles view
            //create crud actions
            context.CrudActions.AddOrUpdate(a => a.ActionCode,
                new CRUDAction { ActionCode = "ADD", Description = "Can Add an entry" });
            context.CrudActions.AddOrUpdate(a => a.ActionCode,
                new CRUDAction { ActionCode = "EDIT", Description = "Can Edit an entry" });
            context.CrudActions.AddOrUpdate(a => a.ActionCode,
                new CRUDAction { ActionCode = "DELETE", Description = "Can Delete an entry" });

            //create station/channel types
            context.StationCategories.AddOrUpdate(c => c.StationCategoryName, new StationCategory{ StationCategoryName = "Fuel Station",SatationCategoryDesc = "Fuel Station", DateCreated=DateTime.Now, IsActive=true });
            context.PumpModels.AddOrUpdate(x => x.Model, new PumpModel { Model = "Quantium" }, new PumpModel { Model = "Horizon" }, new PumpModel { Model = "Endura" });
            context.SaveChanges();

          /*  //create the admins user role
            var adminRole = new Roles { Name = "SystemAdmin", Description = "Super admin", ModifiedBy = 1 };
            context.Roles.AddOrUpdate(
                r => r.Name, adminRole
            );
            context.SaveChanges();
*/
            var admin = new User
            {
                FullName = "Tovuti Group",
                UserName = "admin@ims.com",
                Email = "admin@ims.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserAccount = true,
                IsActive = true,
                IsDeleted = false,
                PasswordHash = "6F7nyIgPwCUxYMUZVJZYbg==",
                DateCreated = DateTime.Now
            };
            context.Users.AddOrUpdate(
                u => u.UserName, admin
                );
            context.SaveChanges();
            //payment modes
            context.PaymentModes.AddOrUpdate(x=>x.PaymentModeName, new PaymentMode{ PaymentModeName = "Cash", IsActive = true, DateCreated = DateTime.Now });
            context.PaymentModes.AddOrUpdate(x => x.PaymentModeName, new PaymentMode { PaymentModeName = "Cheque", IsActive = true, DateCreated = DateTime.Now });
            context.PaymentModes.AddOrUpdate(x=>x.PaymentModeName, new PaymentMode{PaymentModeName = "Mpesa", IsActive = true, DateCreated=DateTime.Now });
            context.SaveChanges(); //Added

            context.PaymentTerms.AddOrUpdate(x=>x.PaymentTermName, new PaymentTerm{PaymentTermName = "Monthly",DayInFollowingMonth = 1,DaysBeforeDue = 30});

            //get the id of the system admin role
            var roleId = context.Roles.FirstOrDefault(r => r.Name == "SystemAdmin").RoleId;

            //get the default user and give them system admins role
            var admin2 = context.Users.FirstOrDefault();
            context.UserRoles.AddOrUpdate(a => a.UserId, new UserRoles { UserId = admin2.Id, RoleId = roleId, ModifiedBy = 1 });


            //seed a default user layout to admin
            context.UserLayout.AddOrUpdate(l => l.UserId, new UserLayout{ UserId = admin2.Id, Mode = Constants.AdminMode});

            //seed product types
            context.ProductCategories.AddOrUpdate(x=>x.ProductCategoryName,
                new ProductCategory{ProductCategoryName = Constants.Fuel,ModifiedBy = 1,IsActive = true, DateCreated=DateTime.Now},
                    new ProductCategory{ProductCategoryName = "Lubricant",ModifiedBy = 1,IsActive = true, DateCreated=DateTime.Now},
                    new ProductCategory{ProductCategoryName = "LPG",ModifiedBy = 1,IsActive = true, DateCreated=DateTime.Now}
            );

            //seed tax category type
            context.TaxCategories.AddOrUpdate(x => x.CategoryName,
                new TaxCategory { CategoryName = "VAT", CategoryDescription = "VAT", CreatedBy = 1, CreatedAt = DateTime.Now, IsActive = true }
            );
            
            //finance transactiontypes
            context.FinanceTransactionTypes.AddOrUpdate(x=>x.FinanceTransTypeName, new FinanceTransactionType{ FinanceTransTypeName = Constants.Purchase},new FinanceTransactionType{FinanceTransTypeName = Constants.Sale});

            //seed a default pricelist
            context.PriceList.AddOrUpdate(l => l.PriceListName, new PriceList { PriceListName = "Default PriceList", PriceListDesc = "Default PriceList", IsActive = true, IsDefault = true, DateCreated = DateTime.Now, ModifiedBy = 1 });
            context.SaveChanges();
            //seed country
          /*  context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Afghanistan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Albania" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Algeria" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "American Samoa" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Angola" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Anguilla" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Antartica" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Antigua and Barbuda" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Argentina" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Armenia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Aruba" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ashmore and Cartier Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Australia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Austria" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Azerbaijan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bahamas" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bahrain" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bangladesh" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Barbados" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Belarus" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Belgium" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Belize" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Benin" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bermuda" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bhutan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bolivia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bosnia and Herzegovina" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Botswana" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Brazil" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "British Virgin Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Brunei" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Bulgaria" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Burkina Faso" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Burma" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Burundi" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cambodia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cameroon" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Canada" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cape Verde" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cayman Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Central African Republic" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Chad" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Chile" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "China" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Christmas Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Clipperton Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cocos (Keeling) Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Colombia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Comoros" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Congo), Democratic Republic of the" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Congo), Republic of the), Cook Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Costa Rica" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cote d Ivoire" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Croatia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cuba" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Cyprus" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Czeck Republic" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Denmark" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Djibouti" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Dominica" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Dominican Republic" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ecuador" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Egypt" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "El Salvador" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Equatorial Guinea" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Eritrea" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Estonia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ethiopia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Europa Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Falkland Islands (Islas Malvinas)" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Faroe Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Fiji" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Finland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "France" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "French Guiana" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "French Polynesia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "French Southern and Antarctic Lands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Gabon" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Gambia), The Gaza Strip" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Georgia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Germany" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ghana" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Gibraltar" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Glorioso Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Greece" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Greenland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Grenada" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guadeloupe" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guam" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guatemala" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guernsey" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guinea" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guinea-Bissau" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Guyana" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Haiti" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Heard Island and McDonald Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Holy See (Vatican City)" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Honduras" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Hong Kong" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Howland Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Hungary" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Iceland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "India" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Indonesia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Iran" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Iraq" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ireland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ireland), Northern" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Israel" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Italy" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Jamaica" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Jan Mayen" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Japan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Jarvis Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Jersey" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Johnston Atoll" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Jordan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Juan de Nova Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Kazakhstan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Kenya" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Kiribati" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Korea), North" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Korea), South" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Kuwait" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Kyrgyzstan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Laos" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Latvia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Lebanon" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Lesotho" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Liberia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Libya" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Liechtenstein" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Lithuania" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Luxembourg" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Macau" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Macedonia), Former Yugoslav Republic of Madagascar" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Malawi" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Malaysia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Maldives" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mali" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Malta" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Man Isle of Marshall Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Martinique" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mauritania" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mauritius" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mayotte" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mexico" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Micronesia), Federated States of" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Midway Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Moldova" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Monaco" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mongolia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Montserrat" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Morocco" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Mozambique" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Namibia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Nauru" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Nepal" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Netherlands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Netherlands Antilles" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "New Caledonia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "New Zealand" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Nicaragua" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Niger" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Nigeria" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Niue" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Norfolk Island" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Northern Mariana Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Norway" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Oman" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Pakistan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Palau" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Panama" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Papua New Guinea" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Paraguay" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Peru" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Philippines" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Pitcaim Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Poland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Portugal" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Puerto Rico" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Qatar" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Reunion" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Romainia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Russia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Rwanda" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Saint Helena" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Saint Kitts and Nevis" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Saint Lucia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Saint Pierre and Miquelon" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Saint Vincent and the Grenadines" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Samoa" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "San Marino" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Sao Tome and Principe" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Saudi Arabia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Scotland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Senegal" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Seychelles" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Sierra Leone" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Singapore" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Slovakia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Slovenia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Solomon Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Somalia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "South Africa" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "South Georgia and South Sandwich Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Spain" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Spratly Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Sri Lanka" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Sudan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Suriname" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Svalbard" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Swaziland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Sweden" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Switzerland" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Syria" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Taiwan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tajikistan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tanzania" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Thailand" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tobago" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Toga" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tokelau" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tonga" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Trinidad" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tunisia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Turkey" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Turkmenistan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Tuvalu" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Uganda" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Ukraine" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "United Arab Emirates" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "United Kingdom" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Uruguay" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "USA" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Uzbekistan" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Vanuatu" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Venezuela" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Vietnam" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Virgin Islands" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Wales" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Wallis and Futuna" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "West Bank" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Western Sahara" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Yemen" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Yugoslavia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Zambia" });
            context.Countries.AddOrUpdate(c => c.CountryName, new Country { CountryName = "Zimbabwe" });
            context.SaveChanges();
            //seed currencies
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Afghanistan Afghani", CurrencySymbol = "AFN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Albanian Lek", CurrencySymbol = "ALL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Algerian Dinar", CurrencySymbol = "DZD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Angolan Kwanza", CurrencySymbol = "AOA", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Argentine Peso", CurrencySymbol = "ARS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Armenian Dram", CurrencySymbol = "AMD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Aruban Guilder", CurrencySymbol = "AWG", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Australian Dollar", CurrencySymbol = "AUD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Azerbaijan New Manat", CurrencySymbol = "AZN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Bahamian Dollar", CurrencySymbol = "BSD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Bahraini Dinar", CurrencySymbol = "BHD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Bangladeshi Taka", CurrencySymbol = "BDT", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Barbados Dollar", CurrencySymbol = "BBD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Belarussian Ruble", CurrencySymbol = "BYR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Belize Dollar", CurrencySymbol = "BZD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Bermudian Dollar", CurrencySymbol = "BMD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Bhutan Ngultrum", CurrencySymbol = "BTN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Boliviano", CurrencySymbol = "BOB", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Marka", CurrencySymbol = "BAM", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Botswana Pula", CurrencySymbol = "BWP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Brazilian Real", CurrencySymbol = "BRL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Brunei Dollar", CurrencySymbol = "BND", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Bulgarian Lev", CurrencySymbol = "BGN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "CFA Franc BCEAO", CurrencySymbol = "XOF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Burundi Franc", CurrencySymbol = "BIF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Kampuchean Riel", CurrencySymbol = "KHR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "CFA Franc BEAC", CurrencySymbol = "XAF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Canadian Dollar", CurrencySymbol = "CAD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Cape Verde Escudo", CurrencySymbol = "CVE", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Chilean Peso", CurrencySymbol = "CLP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Yuan Renminbi", CurrencySymbol = "CNY", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Colombian Peso", CurrencySymbol = "COP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Comoros Franc", CurrencySymbol = "KMF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Francs", CurrencySymbol = "CDF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Costa Rican Colon", CurrencySymbol = "CRC", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Croatian Kuna", CurrencySymbol = "HRK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Cuban Peso", CurrencySymbol = "CUP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Czech Koruna", CurrencySymbol = "CZK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Danish Krone", CurrencySymbol = "DKK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Djibouti Franc", CurrencySymbol = "DJF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Dominican Peso", CurrencySymbol = "DOP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Ecuador Sucre", CurrencySymbol = "ECS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Egyptian Pound", CurrencySymbol = "EGP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "El Salvador Colon", CurrencySymbol = "SVC", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Eritrean Nakfa", CurrencySymbol = "ERN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Ethiopian Birr", CurrencySymbol = "ETB", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Fiji Dollar", CurrencySymbol = "FJD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Gambian Dalasi", CurrencySymbol = "GMD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Georgian Lari", CurrencySymbol = "GEL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Ghanaian Cedi", CurrencySymbol = "GHS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Gibraltar Pound", CurrencySymbol = "GIP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Pound Sterling", CurrencySymbol = "GBP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Guatemalan Quetzal", CurrencySymbol = "QTQ", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Pound Sterling", CurrencySymbol = "GGP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Guinea Franc", CurrencySymbol = "GNF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Guinea-Bissau Peso", CurrencySymbol = "GWP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Guyana Dollar", CurrencySymbol = "GYD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Haitian Gourde", CurrencySymbol = "HTG", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Honduran Lempira", CurrencySymbol = "HNL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Hong Kong Dollar", CurrencySymbol = "HKD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Hungarian Forint", CurrencySymbol = "HUF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Iceland Krona", CurrencySymbol = "ISK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Indian Rupee", CurrencySymbol = "INR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Indonesian Rupiah", CurrencySymbol = "IDR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Iranian Rial", CurrencySymbol = "IRR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Iraqi Dinar", CurrencySymbol = "IQD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Israeli New Shekel", CurrencySymbol = "ILS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Jamaican Dollar", CurrencySymbol = "JMD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Japanese Yen", CurrencySymbol = "JPY", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Jordanian Dinar", CurrencySymbol = "JOD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Kazakhstan Tenge", CurrencySymbol = "KZT", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Kenyan Shilling", CurrencySymbol = "KES", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "North Korean Won", CurrencySymbol = "KPW", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Korean Won", CurrencySymbol = "KRW", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Kuwaiti Dinar", CurrencySymbol = "KWD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Som", CurrencySymbol = "KGS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Lao Kip", CurrencySymbol = "LAK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Latvian Lats", CurrencySymbol = "LVL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Lebanese Pound", CurrencySymbol = "LBP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Lesotho Loti", CurrencySymbol = "LSL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Liberian Dollar", CurrencySymbol = "LRD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Libyan Dinar", CurrencySymbol = "LYD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Swiss Franc", CurrencySymbol = "CHF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Lithuanian Litas", CurrencySymbol = "LTL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Macau Pataca", CurrencySymbol = "MOP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Denar", CurrencySymbol = "MKD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Malagasy Franc", CurrencySymbol = "MGF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Malawi Kwacha", CurrencySymbol = "MWK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Malaysian Ringgit", CurrencySymbol = "MYR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Maldive Rufiyaa", CurrencySymbol = "MVR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Mauritanian Ouguiya", CurrencySymbol = "MRO", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Mauritius Rupee", CurrencySymbol = "MUR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Mexican Nuevo Peso", CurrencySymbol = "MXN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Moldovan Leu", CurrencySymbol = "MDL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Mongolian Tugrik", CurrencySymbol = "MNT", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Moroccan Dirham", CurrencySymbol = "MAD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Mozambique Metical", CurrencySymbol = "MZN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Myanmar Kyat", CurrencySymbol = "MMK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Namibian Dollar", CurrencySymbol = "NAD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Nepalese Rupee", CurrencySymbol = "NPR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Nigerian Naira", CurrencySymbol = "NGN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "New Zealand Dollar", CurrencySymbol = "NZD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Norwegian Krone", CurrencySymbol = "NOK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Omani Rial", CurrencySymbol = "OMR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Pakistan Rupee", CurrencySymbol = "PKR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Panamanian Balboa", CurrencySymbol = "PAB", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Paraguay Guarani", CurrencySymbol = "PYG", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Peruvian Nuevo Sol", CurrencySymbol = "PEN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Philippine Peso", CurrencySymbol = "PHP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Polish Zloty", CurrencySymbol = "PLN", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Qatari Rial", CurrencySymbol = "QAR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Romanian New Leu", CurrencySymbol = "RON", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Russian Ruble", CurrencySymbol = "RUB", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Rwanda Franc", CurrencySymbol = "RWF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "St. Helena Pound", CurrencySymbol = "SHP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Samoan Tala", CurrencySymbol = "WST", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Dobra", CurrencySymbol = "STD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Saudi Riyal", CurrencySymbol = "SAR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Dinar", CurrencySymbol = "RSD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Seychelles Rupee", CurrencySymbol = "SCR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Sierra Leone Leone", CurrencySymbol = "SLL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Singapore Dollar", CurrencySymbol = "SGD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Somali Shilling", CurrencySymbol = "SOS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "South African Rand", CurrencySymbol = "ZAR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "South Sudan Pound", CurrencySymbol = "SSP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Sri Lanka Rupee", CurrencySymbol = "LKR", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Sudanese Pound", CurrencySymbol = "SDG", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Surinam Dollar", CurrencySymbol = "SRD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Swaziland Lilangeni", CurrencySymbol = "SZL", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Swedish Krona", CurrencySymbol = "SEK", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Syrian Pound", CurrencySymbol = "SYP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Taiwan Dollar", CurrencySymbol = "TWD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Tajik Somoni", CurrencySymbol = "TJS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Tanzanian Shilling", CurrencySymbol = "TZS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Thai Baht", CurrencySymbol = "THB", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Tongan Paanga", CurrencySymbol = "TOP", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Tunisian Dollar", CurrencySymbol = "TND", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Turkish Lira", CurrencySymbol = "TRY", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Manat", CurrencySymbol = "TMT", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "US Dollar", CurrencySymbol = "USD", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Uganda Shilling", CurrencySymbol = "UGX", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Ukraine Hryvnia", CurrencySymbol = "UAH", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Arab Emirates Dirham", CurrencySymbol = "AED", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Uruguayan Peso", CurrencySymbol = "UYU", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Uzbekistan Sum", CurrencySymbol = "UZS", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Vanuatu Vatu", CurrencySymbol = "VUV", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Venezuelan Bolivar", CurrencySymbol = "VEF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Vietnamese Dong", CurrencySymbol = "VND", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "CFP Franc", CurrencySymbol = "XPF", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Yemeni Rial", CurrencySymbol = "YER", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Zambian Kwacha", CurrencySymbol = "ZMW", DateCreated = DateTime.Now });
            context.Currencies.AddOrUpdate(c => c.CurrencyName, new Currency { CurrencyName = "Zimbabwe Dollar", CurrencySymbol = "ZWD", DateCreated = DateTime.Now });
            context.SaveChanges();*/
            //parent menu for dashboard
            var dashboard = new Menu
            {
                Status = true,
                MenuName = "Dashboard",
                Controller = "#",
                Action = "#",
                Icon = "fa fa-dashboard text-yellow",
                Sequence = 1,
                LayoutBase = Constants.AdminMode
            };

            context.Menus.AddOrUpdate(
                m => m.MenuName, dashboard
            );
            context.SaveChanges(); //Added
            var dashboardId = dashboard.MenuId;
            //analytical dashboard
            var analyticalDashboard = new Menu
            {
                Status = true,
                MenuName = "Analitical Dashboard",
                ParentMenu = dashboardId,
                Controller = "Dashboard",
                Action = "Index",
                Icon = "",
                Sequence = 1
            };
            context.Menus.AddOrUpdate(m => m.MenuName, analyticalDashboard);
            context.SaveChanges(); //Added

            //seed default products; same as fuel type ; Added
            context.Products.AddOrUpdate(a => a.ProductName,
                new Product { ProductName = "PETROL", ProductDesc = "PETROL", ProductCategoryId = 1, TaxCategoryId = 1, DateCreated = DateTime.Now, ModifiedBy = 1, IsActive = true });
            context.Products.AddOrUpdate(a => a.ProductName,
                new Product { ProductName = "DIESEL", ProductDesc = "DIESEL", ProductCategoryId = 1, TaxCategoryId = 1, DateCreated = DateTime.Now, ModifiedBy = 1, IsActive = true });
            context.Products.AddOrUpdate(a => a.ProductName,
                new Product { ProductName = "KEROSENE", ProductDesc = "KEROSENE", ProductCategoryId = 1, TaxCategoryId = 1, DateCreated = DateTime.Now, ModifiedBy = 1, IsActive = true });
            //////////////////////////////////

            AssignRole(roleId, analyticalDashboard.MenuId, dashboardId, "{1,2,3}");





            //############################################### User Settings #############################################################
            //parent menu for user settings
            var userSettings = new Menu
            {
                Status = true,
                MenuName = "User Settings",
                Controller = "#",
                Action = "#",
                Icon = "fa fa-user text-yellow",
                Sequence = 8,
                LayoutBase = Constants.AdminMode
            };
            context.Menus.AddOrUpdate(
                m => m.MenuName, userSettings
            );
            context.SaveChanges(); //Added
            var userSettingsId = userSettings.MenuId;
            //////////////// children menus
            var roles = new Menu
            {
                Status = true,
                MenuName = "Manage Roles",
                Controller = "Roles",
                Action = "Index",
                Icon = "#",
                Sequence = 1,
                ParentMenu = userSettingsId
            };

            context.Menus.AddOrUpdate(m => m.MenuName, roles);
            ChildMenu(context, "Manage Roles", "Roles", "Index", 1, userSettingsId, roleId, "{1,2,3}"); //What is this for??

            context.SaveChanges(); //Added
            AssignRole(roleId, roles.MenuId, userSettingsId,"{1,2,3}"); 
            //manage users
//            var manageUsers = new Menu
//            {
//                Status = true,
//                MenuName = "Manage Users",
//                Controller = "Manage",
//                Action = "Index",
//                Icon = "#",
//                Sequence = 2,
//                ParentMenu = userSettingsId
//            };
//            context.Menus.AddOrUpdate(m => m.MenuName, manageUsers);
//            context.SaveChanges();
//            AssignRole(roleId, manageUsers.MenuId, userSettingsId, "{1,2,3}");


            //############################################### System Settings #############################################################
            //parent menu for system settings
            var systemSettings = new Menu
            {
                Status = true,
                MenuName = "System Settings",
                Controller = "#",
                Action = "#",
                Icon = "fa fa-wrench text-yellow",
                Sequence = 9,
                LayoutBase = Constants.AdminMode
            };
            context.Menus.AddOrUpdate(
                m => m.MenuName, systemSettings
            );
            context.SaveChanges(); //Added
            var systemSettingsId = systemSettings.MenuId;
            //////////////////// children menus for system settings
            var manageMenu = new Menu
            {
                Status = true,
                MenuName = "Manage Menu",
                Controller = "Menus",
                Action = "Index",
                Icon = "#",
                Sequence = 1,
                ParentMenu = systemSettingsId
            };

            context.Menus.AddOrUpdate(m => m.MenuName, manageMenu);
            context.SaveChanges();

            AssignRole(roleId, manageMenu.MenuId, systemSettingsId, "{1,2,3}");
            ChildMenu(context, "General Settings", "GeneralSettings", "Index", 2, systemSettingsId, roleId, "{1,2,3}");



            //##################### Inventory Module###############
            //parent
            var suppliersParent = ParentMenu(context, "Suppliers", 2, Constants.AdminMode, "fa fa-truck text-yellow");

            //manage suppliers
            ChildMenu(context, "Manage Suppliers", "Suppliers", "Index", 2, suppliersParent, roleId, "{1,2,3}");

            //################## Proucts module

            var productsParent = ParentMenu(context, "Products", 3, Constants.AdminMode, "fa fa-gears text-yellow");
            //children
            ChildMenu(context, "Manage Uom", "UOM", "Index", 2, productsParent, roleId, "{1,2,3}");
            //manage products categories
            /*ChildMenu(context, "Manage Product Categories", "ProductCategories", "Index", 1, productsParent, roleId,
                "{1,2,3}");*/
            ChildMenu(context, "Manage Products", "Products", "Index", 1, productsParent, roleId, "{1,2,3}");

            //################## Station prices module

//            var stationPricesParent = ParentMenu(context, "Manage Station", 4, Constants.AdminMode);
//            //children
//            ChildMenu(context, "Station Prices", "ChannelPrices", "Index", 1, stationPricesParent, roleId, "{1,2,3}");

            //################## Warehouses
            /*var warehouseParent = ParentMenu(context, "Warehouses", 5, Constants.AdminMode);
            //warehouse types
            ChildMenu(context, "Manage Warehouse Types", "WarehouseTypes", "Index", 1, warehouseParent, roleId,
                "{1,2,3}");
            ChildMenu(context, "Manage Warehouses", "Warehouses", "Index", 2, warehouseParent, roleId, "{1,2,3}");
*/
           /* //parent menu for sock management
            var stockParent = ParentMenu(context, "Stock Management", 6, Constants.AdminMode);
            ChildMenu(context, "Manage Stock", "Stock", "Index", 1, stockParent, roleId, "{1,2,3}");
            ChildMenu(context, "Stock Transaction", "Stock", "Create", 2, stockParent, roleId, "{1,2,3}");
            ChildMenu(context, "Transactions", "Stock", "Transactions", 3, stockParent, roleId, "{1,2,3}");
*/
            //######### Station Management
            //parent
            var stationParent = ParentMenu(context, "Station Management", 7, Constants.AdminMode, "fa fa-gears text-yellow");
            //children
            ChildMenu(context, "Manage Stations", "Stations", "Index", 1, stationParent, roleId, "{1,2,3}");
            //price lists
            ChildMenu(context, "Price Lists", "ChannelPrices", "PriceLists", 2, stationParent, roleId, "{1,2,3}");
            ChildMenu(context, "Station Prices", "ChannelPrices", "Index", 3, stationParent, roleId, "{1,2,3}");

            //######### Tax Management
            //parent
            var taxManagementParent = ParentMenu(context, "Tax Management", 8, Constants.AdminMode, "fa fa-gears text-yellow");
            //children
            ChildMenu(context, "Tax Categories", "TaxCategories", "Index", 2, taxManagementParent, roleId, "{1,2,3}");
            ChildMenu(context, "Tax Rates", "TaxRates", "Index", 3, taxManagementParent, roleId, "{1,2,3}");
            ChildMenu(context, "Customer Tax Categories", "CustomerTaxCategories", "Index", 4, taxManagementParent,
                roleId, "{1,2,3}");
            ChildMenu(context, "Tax List", "Taxes", "Index", 1, taxManagementParent, roleId, "{1,2,3}");


            //######### staff Management
            //parent
            var staffManagement = ParentMenu(context, "Staff Management", 9, Constants.AdminMode, "fa fa-gears text-yellow");
            ChildMenu(context, "Staff", "Staffs", "Index", 1, staffManagement, roleId, "{1,2,3}");

            //######### Shift Management
            var shiftManagement = ParentMenu(context, "Shift Management", 10, Constants.AdminMode, "fa fa-gears text-yellow");
            ChildMenu(context, "Shifts", "Shifts", "Index", 1, shiftManagement, roleId, "{1,2,3}");

//            //######### Shift Management
//            var pumpManagement = ParentMenu(context, "Pump Management", 11, Constants.AdminMode);
//            ChildMenu(context, "Pumps", "Pumps", "Index", 1, pumpManagement, roleId, "{1,2,3}");
//            ChildMenu(context, "Nozzle Types", "NozzleTypes", "Index", 2, pumpManagement, roleId, "{1,2,3}");
//            ChildMenu(context, "Nozzles", "Nozzles", "Index", 3, pumpManagement, roleId, "{1,2,3}");

            StationMenu(context, "OPEN SHIFT",roleId);
            StationMenu(context, "DROP CASH",roleId);
            StationMenu(context, "BANK CASH",roleId);
            StationMenu(context, "CLOSE SHIFT",roleId);
            StationMenu(context, "RECEIVE FUEL",roleId);

        }

        private long ParentMenu(IMSDataEntities context, string menuName, int sequence,
            string mode,string icon)
        {
            //            var context = new IMSDataEntities();
            var parentMenu = new Menu
            {
                Status = true,
                MenuName = menuName,
                Controller = "#",
                Action = "#",
                Icon = icon,
                Sequence = sequence,
                LayoutBase = mode
            };
            context.Menus.AddOrUpdate(
                m => m.MenuName, parentMenu
            );
            context.SaveChanges(); //Added
            return parentMenu.MenuId;
        }
       
        private void ChildMenu(IMSDataEntities context, string childMenuName,
            string controller, string action, int sequence, long parentId, long roleId, string cruds)
        {
            //            var context = new IMSDataEntities();
            var childMenu = new Menu
            {
                Status = true,
                MenuName = childMenuName,
                Controller = controller,
                Action = action,
                Icon = "#",
                Sequence = sequence,
                ParentMenu = parentId
            };
            context.SaveChanges(); //Added
            context.Menus.AddOrUpdate(m => m.MenuName, childMenu);
            context.SaveChanges(); //Added
            context.CrudMenuActions.AddOrUpdate(m => m.MenuId,
                new Models.CrudMenuActions { MenuId = childMenu.MenuId, CrudActions = cruds });

                        AssignRole(roleId, childMenu.MenuId, parentId, cruds); //Uncommented by Adam
        }







        private void AssignRole(long Rid, long menuId, long parentId, string cruds)
        {
            var context = new IMSDataEntities();

            var count = context.UserRoleAllocations.Where(x => x.RoleId == Rid && x.MenuId == menuId);
            if (!count.Any())
            {
                context.UserRoleAllocations.Add(

                    new UserRoleAllocation
                    {
                        RoleId = Rid,
                        MenuId = menuId,
                        ParentId = parentId,
                        CrudActions = cruds
                    }
                );
                context.SaveChanges();
            }
        }


        public void StationMenu(IMSDataEntities context,string menuName,long roleId)
        {
            StationMenus menu = new StationMenus
            {
                MenuName = menuName
            };  
            context.StationMenus.AddOrUpdate(x=>x.MenuName,menu);
            context.StationRoles.AddOrUpdate(x=>new {x.UserId,x.StationMenuId}, new StationRoles{ UserId = roleId, StationMenuId = menu.Id});
            context.SaveChanges();
        }
       
    }
}
