using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.WebPages;
using StationLinerMVC.Models;


namespace StationLinerMVC
{
    public static class Flag
    {
        private static string Controller { get; set; }
        private static string Action { get; set; }
        private static bool status { get; set; }
        public static bool Status(string check = null)
        {
//            bool status = false;
            if (check != null)
            {
                // create a new instance of the db context
                var db = new IMSDataEntities();

                //first get the logged in user
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();

                //with the user id get the role which the user belongs to
                var userRoleId = db.UserRoles.FirstOrDefault(x => x.UserId == userId);
                if (userRoleId == null)
                {
                    return status = false;
                }
                   
                else
                {
                    //now store the role id to use it below
                    var roleId = userRoleId.RoleId;

                    //below we get the current request so that we get the menu id and check whether it has the crud permissions that we need
                    var currentUrl = Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);

                    //format the url to obtain both the controller name and the name of the menu
                    var formattedUrl = currentUrl.TrimStart(new[] {' ', '/'}).TrimEnd(new[] {' ', '/'});
                    var urls = formattedUrl.Split(new char[] {'/'});
                    if (urls.Length > 1)
                    {
                        Controller = urls[0];
                        Action = urls[1];
                    }
                    else
                    {
                        Controller = formattedUrl;
                        Action = "Index";
                    }


                    //now we can get the id of the menu using the contoller and the action name as well
                    var menuId = db.Menus.FirstOrDefault(x => x.Controller == Controller && x.Action == Action);
                    if (menuId == null)
                    {
                         status = false;
                    }
                    else
                    {
                        //store the id of the menu
                        var currentMenuId = menuId.MenuId;

                        //by the look of things, its now clear that we can be able to get the crud actions that are assigned to this menu item since we have
                        // the menu id, and the role id... perfect,right?
                        var allocatedCrudActions =
                            db.UserRoleAllocations.FirstOrDefault(x => x.MenuId == currentMenuId && x.RoleId == roleId);
                        if (allocatedCrudActions == null || allocatedCrudActions.CrudActions == null)
                        {
                             status = false;
                        }
                        else
                        {
                            //first we get the id of the action the user is trying to access from the view, can either be add, edit or delete.
                            //the action should be passed as a parameter when the function is called
                            var currentActionId = db.CrudActions.FirstOrDefault(x => x.ActionCode == check);
                            if (currentActionId == null)
                            {
                                 status = false;
                            }
                            else
                            {
                                var crudId = currentActionId.Id;
                                //now we format the crud actions and and strip them into an array
                                var allocatedActions = allocatedCrudActions.CrudActions;
                                var formattedCrud = allocatedActions.Trim(new[] {'{', '}'}).TrimStart().TrimEnd(new char[]{',',' '});
                                
                                //now get the allocated crud actions as an array
                                var finalCrudActions = formattedCrud.Split(new char[] {','});
                                
                                //check whether the requested permission exists in the final array
                                if (finalCrudActions.Contains(crudId.ToString()))
                                {
                                     status = true;
                                }
                                else
                                {
                                     status = false;
                                }
                            }
                            
                        }
                    }

                }
            }
            Debug.WriteLine("status " + status);
            return status;
        }
    }

    public static class Validation
    {
        public static bool Status;
        public static bool CheckExisting(IMSDataEntities model,string column,string value)
        {
            Status = false;


            return Status;
        }
    }

    
}