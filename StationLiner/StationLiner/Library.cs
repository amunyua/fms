using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StationLinerMVC.Models;

namespace StationLinerMVC
{
    [Authorize]
    public static class Library
    {
        private static IMSDataEntities db = new IMSDataEntities();

        public static long UserRole { get; set; }
        public static long UserId { get;  set; }
        public static string Layout { get; set; }
        public static long? ChannelId { get; set; }

        static Library()
        {
            UserRole = GetUserRole();
            if (IdentityExtensions.GetUserId(HttpContext.Current.User.Identity) != null)
            {
                UserId = Int64.Parse(IdentityExtensions.GetUserId(HttpContext.Current.User.Identity));
            }
            Layout = ViewLayout();
            db.Configuration.LazyLoadingEnabled = false;
        }
        public static long GetUserRole()
        {
            var userRole = default(long);


            if (UserId != default(int))
            {
//                using (db)
//                {
                    userRole = db.UserRoles.FirstOrDefault(x => x.UserId == UserId).RoleId;
//                }


            }

            return userRole;
        }

        public static string GetRoleName()
        {
            return db.Roles.Find(GetUserRole()).Name;
        }

        /* This function returns the logged in person
            if the logged in is the admin, the method returns true
        */
        public static bool IsAdmin()
        {
            if (GetRoleName() == Constants.AdminRole)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string  ViewLayout()
        {
            var lay = db.UserLayout.Where(x => x.UserId == UserId);
            if (lay.Any())
            {
                return lay.First().Mode;
            }
            else
            {
                return "";
            }
        }

        public static void Log(string message,string trace)
        {
            
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Logs"), "logs.text");
            try
            {
                var controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"]
                    .ToString();
                var actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"]
                    .ToString();
                string line = DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss") + " " + message+" "+ trace;/*
                string line = DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss") + " " + controllerName + " " + actionName +
                              " " + message+" "+ trace;*/
                if (!File.Exists(path))
                {
                    File.Create(path);
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine(line);
                        tw.Close();
                    }
                }
                else if (File.Exists(path))
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine(line);
                        tw.Close();
                    }
                }
            }
            catch
            {
                
            }
           
        }

        public static void Log2(Exception e)
        {
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Logs"), "logs.text");
            try
            {
                /*var controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"]
                    .ToString();
                var actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"]
                    .ToString();*/
                string line = DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss") + " " + e.Message + " " + e.StackTrace;/*
                string line = DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss") + " " + controllerName + " " + actionName +
                              " " + message+" "+ trace;*/
                if (!File.Exists(path))
                {
                    File.Create(path);
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine(line);
                        tw.Close();
                    }
                }
                else if (File.Exists(path))
                {
                    using (var tw = new StreamWriter(path, true))
                    {
                        tw.WriteLine(line);
                        tw.Close();
                    }
                }
            }
            catch
            {

            }
        }
    }
}