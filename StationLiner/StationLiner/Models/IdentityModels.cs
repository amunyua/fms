using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StationLinerMVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<long, CustomUserLogin, CustomUserRole,
    CustomUserClaim> 
    {
        public long CreatedBy { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }
    }

    /*public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
    long, CustomUserLogin, CustomUserRole, CustomUserClaim>
    { 
 
        public ApplicationDbContext()
            : base("IMSDataEntities")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<ApplicationUser>().ToTable("Staff");
           // modelBuilder.Entity<CustomRole>().ToTable("Roles");
           // modelBuilder.Entity<CustomUserRole>().ToTable("UserRoles");
           // modelBuilder.Entity<CustomUserClaim>().ToTable("UserClaims");
           // modelBuilder.Entity<CustomUserLogin>().ToTable("UserLogins");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }*/

    /*public class CustomUserRole : IdentityUserRole<long>
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
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }*/
    
    /*public class CustomRoleStore : RoleStore<CustomRole, long, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }*/

}