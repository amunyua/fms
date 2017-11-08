using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StationLinerMVC.Models
{
    public interface IUserCustomStore<TUser, Tkey> : IUserStore<User, long>
    {
        Task<User> FindByNamePassAsync(string username, String password);
    }
    public class StaffStore : IUserStore<User, long>, IUserEmailStore<User, long>
    {
        //private IMSDataEntities database;
        private IMSDataEntities iMSDataEntities;

        public StaffStore()
        {
            this.iMSDataEntities = new IMSDataEntities();
        }

        public StaffStore(IMSDataEntities iMSDataEntities)
        {
            // TODO: Complete member initialization
            this.iMSDataEntities = iMSDataEntities;
        }

        public void Dispose()
        {
            this.iMSDataEntities.Dispose();
        }

        public Task CreateAsync(User user)
        {
            // TODO
            this.iMSDataEntities.Users.Add(user);
            this.iMSDataEntities.SaveChanges();
            return Task.FromResult(0);
        }

        public Task UpdateAsync(User user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(long userId)
        {
            //To do
            //Check for null Id
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.Id == userId);
            return Task.FromResult<User>(staff);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            //To do
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.UserName == userName);
            return Task.FromResult<User>(staff);
            //throw new NotImplementedException();
        }

        public Task<User> FindByNamePassAsync(string username, String password)
        {
            //To do
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.UserName == username && x.PasswordHash == EncryptDecrypt.Encrypt(password, "@#Df4190^") && x.IsActive == true && x.IsDeleted == false);
            return Task.FromResult<User>(staff);
            //throw new NotImplementedException();
        }

        /* public Task<string> GetPasswordHashAsync(User user)
         {
             return Task.FromResult<string>(string.Empty);
             //throw new NotImplementedException();
         }

         public Task<bool> HasPasswordAsync(User user)
         {
             throw new NotImplementedException();
         }

         public Task SetPasswordHashAsync(User user, string passwordHash)
         {
             throw new NotImplementedException();
             //user.PasswordHash = passwordHash; // EncryptDecrypt.Encrypt(passwordHash, "@#Df4190^");
             //return Task.FromResult(0);
         }*/


        /*public Task<User> FindByIdAsync(long userId)
        {
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.Id == userId);
            return Task.FromResult<User>(staff);
        }*/

        public Task<User> FindByEmailAsync(string email)
        {
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.Email == email);
            return Task.FromResult<User>(staff);
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult<bool>(user.EmailConfirmed);
        }



        public Task SetEmailAsync(User user, string email)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            throw new NotImplementedException();
        }
    }

    public class UserStore : IUserCustomStore<User, long>
    {
        private IMSDataEntities iMSDataEntities;
        public UserStore()
        {
            this.iMSDataEntities = new IMSDataEntities();
        }

        public UserStore(IMSDataEntities iMSDataEntities)
        {
            // TODO: Complete member initialization
            this.iMSDataEntities = iMSDataEntities;
        }
        public void Dispose()
        {
            this.iMSDataEntities.Dispose();
        }

        public Task CreateAsync(User user)
        {
            // TODO
            this.iMSDataEntities.Users.Add(user);
            this.iMSDataEntities.SaveChanges();
            return Task.FromResult(0);
        }

        public Task UpdateAsync(User user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(long userId)
        {
            //To do
            //Check for null Id
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.Id == userId);
            return Task.FromResult<User>(staff);
        }

        public Task<User> FindByNameAsync(string email)
        {
            //To do
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.Email == email);
            return Task.FromResult<User>(staff);
            //throw new NotImplementedException();
        }

        public Task<User> FindByNamePassAsync(string username, String password)
        {
            //To do
//            if(FindByNameAsync(username))
            String passwordEncrypt = EncryptDecrypt.Encrypt(password, "@#Df4190^");
            User staff = this.iMSDataEntities.Users.FirstOrDefault(x => x.UserName == username && x.PasswordHash == passwordEncrypt && x.UserAccount == true && x.IsDeleted == false);
            return Task.FromResult<User>(staff);
            //throw new NotImplementedException();
        }
    }
}