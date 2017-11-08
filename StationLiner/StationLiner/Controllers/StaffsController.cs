using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Antlr.Runtime.Misc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Schema;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class StaffsController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public StaffsController()
        {

        }

        public StaffsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        // GET: Staffs
        public ActionResult Index()
        {
            var staffs = db.Users.Where(x=>x.IsDeleted != true).OrderByDescending(x=>x.Id);
            ViewBag.roles = db.Roles.Where(x=>x.IsDeleted != true).ToList();

            return View(staffs.ToList());
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StaffViewModels values)
        {
            if (ModelState.IsValid)
            {
                var staff = new User();
                staff.FullName = values.FullName;
                staff.UserName = values.UserName;
                staff.Email = values.Email;
                staff.EmailConfirmed = false;
                staff.PhoneNumber = values.PhoneNumber;
                staff.AccessFailedCount = 0;
                staff.LockoutEnabled = false;
                staff.Address = values.Address;
                staff.NationalIdentification = values.NationalIdentification;
                staff.UserAccount = values.UserAccount;
                staff.IsActive = true;
                staff.IsDeleted = false;
                staff.DateCreated = DateTime.Now;
                staff.ModifiedBy = Library.UserId; // User.Identity.GetUserId<int>(); 
                Debug.WriteLine("reached here");
                //                var userMan = new UserManager<ApplicationUserManager,string>();
                var result = await UserManager.CreateAsync(staff);
                //Error handling when email is the same?? and others??
                if (result.Succeeded)
                {

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //if the account has been created assign the user to the role assigned
                    if (values.UserAccount)
                    {
                        try
                        {
                        var userRole = new UserRoles();
                        userRole.RoleId = values.Role;

                        
                        User staff2 = await UserManager.FindByEmailAsync(values.Email);
                        //create a user layout
                        var userLayout = new UserLayout();
                        userLayout.UserId = staff2.Id;
                        userLayout.Mode = Constants.AdminMode;
                        db.UserLayout.Add(userLayout);
                        //create a user role
                        userRole.UserId = staff2.Id;
                        userRole.ModifiedBy = Library.UserId;
                        db.UserRoles.Add(userRole);
                        db.SaveChanges();
                        //await SignInManager.SignInAsync(staff, isPersistent: false, rememberBrowser: false);  //SignInAsync(staff, staff.Id);
                        /*string code = await UserManager.GenerateEmailConfirmationTokenAsync(staff2.Id);*/

                        
                            var callbackUrl = Url.Action("SetPassword", "Staffs",
                                new { userId = CryptorEngine.Encrypt(Convert.ToString(staff2.Id)) }, protocol: Request.Url.Scheme);
                            await UserManager.SendEmailAsync(staff2.Id,
                                "Confirm your account", "Hi " + staff2.FullName + ",<br/>" + "Please confirm your account by clicking <a href=\""
                                                        + callbackUrl + "\">here</a>");
                        }
                        catch (Exception e)
                        {
                            Library.Log(e.Message,e.StackTrace);
                        }
                           
                    }
                    return RedirectToAction("Index", "Staffs");


                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
//            ViewBag.ChannelId = new SelectList(db.Channels, "Id", "ChannelName", values["ChannelId"]);
            ViewBag.Role = new SelectList(db.Roles, "RoleId", "Name", values.Role);
            return RedirectToAction("Index", "Staffs");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public JsonResult GetStaffDetails(FormCollection coll)
        {
            var id = Int64.Parse(coll["id"]);
            
            return Json(db.Users.Find(id));
        }

        public JsonResult GetRole(FormCollection col)
        {
            var id = Int64.Parse(col["id"]);
            var role = db.UserRoles.FirstOrDefault(x => x.UserId == id).RoleId;
            return Json(role);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User staff)
        {
            if (ModelState.IsValid)
            {
                staff.DateCreated = DateTime.Now;
                staff.IsDeleted = false;
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                Session["success"] = "Staff details updated";
                return RedirectToAction("Index");
            }
            ViewBag.Role = new SelectList(db.Roles.Where(x=>x.IsDeleted != true), "RoleId", "RoleName");
            return RedirectToAction("Index");
        }
        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User staff = db.Users.Find(id);
            staff.IsActive = false;
            staff.UserAccount = false;
            staff.IsDeleted = true;
//            db.Users.Remove(staff);
            try
            {
                Session["success"] = "Staff deleted";
                db.SaveChanges();
            }
            catch (Exception e)
            {
               Library.Log(e.Message,e.StackTrace);
            }
            
            return RedirectToAction("Index");
        }


//
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                //check whether the user has been blocked
                if (user.LockoutEnabled)
                {
                    return View("ForgotPasswordConfirmation");
                }

                user.IsActive = false;
//                user.LockoutEnabled = true;
                user.AccessFailedCount = 3;
                db.SaveChanges();
                var callbackUrl = Url.Action("SetPassword", "Staffs",
                    new { userId = CryptorEngine.Encrypt(Convert.ToString(user.Id)) }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id,
                    "Reset Password", "Hi " + user.FullName + ",<br/>" + "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                 return RedirectToAction("ForgotPasswordConfirmation", "Staffs");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }



       //Get: set password
        [AllowAnonymous]
        public ActionResult SetPassword(string userId)
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetnewPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            System.Diagnostics.Debug.WriteLine("UserId............" + model.userId);
            var user = await UserManager.FindByIdAsync(Convert.ToInt64(CryptorEngine.Decrypt(model.userId)));
            if (user == null)
            {
//                // Don't reveal that the user does not exist
               return RedirectToAction("SetPassword", "Staffs");
            }
            else //Add check and else if state for link expired e.t.c
            {
                user.PasswordHash = EncryptDecrypt.Encrypt(model.ConfirmPassword, "@#Df4190^");
                user.EmailConfirmed = true;
                user.IsActive = true;
                user.IsDeleted = false;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.ModifiedBy = Convert.ToInt64(CryptorEngine.Decrypt(model.userId));
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
//
        } 


        public JsonResult IsUserExists(string UserName)
        {
//check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.
            return Json(!db.Users.Any(x => x.UserName == UserName && x.IsDeleted == false) ,JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExist(string Email)
        {
//check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.
            return Json(!db.Users.Any(x => x.Email == Email && x.IsDeleted == false) ,JsonRequestBehavior.AllowGet);
        }


        public ActionResult BlockUnblockUser(long userId, bool status)
        {
            var user = db.Users.Find(userId);
            string message;
            user.LockoutEnabled = status;
            user.IsActive = !status;
            db.SaveChanges();
            if (status)
            {
                message = "User has been blocked";
            }
            else
            {
                message = "User has been unblocked";
            }
            Session["success"] = message;
            return RedirectToAction("Index");
        }
        
        
        
        
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult CheckUnique(List<UserCheck> checks)
        {
            
            return Json(checks);
        }
    }
}
