using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using StationLinerMVC.Models;
using System.Text;
using System.Configuration;

namespace StationLinerMVC
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            ////////
            //string id = CryptorEngine.Encrypt(Convert.ToString(message.Staff_id));
            //string verificationLink = BaseSiteUrl + "Account/Login/?id=" + HttpUtility.UrlEncode(id);
            StringBuilder strBody = new StringBuilder();
            strBody.Append(message.Body);
            //strBody.Append("Link is " +"<a href='" + verificationLink + "'>Click here</a>");

            EmailDispacher objEmail = new EmailDispacher();
            objEmail.from = ConfigurationManager.AppSettings["AdminEmail"];
            objEmail.to = message.Destination; //objUser.Staff_Email;
            objEmail.subject = message.Subject; //objComTemplate.Template_Subject;
            objEmail.password = ConfigurationManager.AppSettings["AdminEmailPassword"];
            objEmail.strbody = strBody.ToString();
            objEmail.SendEmail();
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User, long>
    {
        public ApplicationUserManager(IUserStore<User, long> store)
            : base(store)
        {
        }

        public Task<User> FindByNamePassAsync(string username, String password)
        {
            UserStore userStore = new UserStore();
            return userStore.FindByNamePassAsync(username, password);
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new StaffStore(context.Get<IMSDataEntities>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Register two factor authentication providers. This application uses Phone
            // and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            return manager;
        }
    }


    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, long>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        /*public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }*/

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
