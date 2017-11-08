using StationLinerMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();

        // GET: Roles
        public ActionResult Index()
        {
            if (Library.IsAdmin())
            {
                return View(db.Roles.Where(x=>x.IsDeleted == false).ToList());
            }
            else
            {
                return View(db.Roles.Where(x => x.ModifiedBy == Library.UserId && x.IsDeleted == false));
            }
        }

        // GET: Roles/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles = db.Roles.Find(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Roles roles)
        {
            if (ModelState.IsValid)
            {
                if (db.Roles.Any(x => x.Name == roles.Name && x.IsDeleted == false))
                {
                    Session["warning"] = "A role with that name already exist";
                    return RedirectToAction("Index");
                }
                roles.IsDeleted = false;
                roles.ModifiedBy = Library.UserId;
//                roles.UserLevel = (Library.IsAdmin()) ? 1 : 2;
                
                db.Roles.Add(roles);
                db.SaveChanges();
//                db.Message("success", "The Role has been created");
                Session["success"] = "The Role has been created";
                return RedirectToAction("Index");
            }

            return View(roles);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleId,Name,Description")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                var oldData = db.Roles.Find(roles.RoleId);
                if (oldData != default(Roles))
                {
                    if (oldData.Name != roles.Name)
                    {
                        var checkExisting = db.Roles.Where(x => x.Name == roles.Name);
                        if (checkExisting.Any())
                        {
                            Session["warning"] = roles.Name + " already exist";
                            return RedirectToAction("Index");
                        }
                    }
                    oldData.IsDeleted = false;
                    oldData.ModifiedBy = Library.UserId;
                    oldData.Name = roles.Name;
                    oldData.Description = roles.Description;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
                
//                db.Entry(roles).State = EntityState.Modified;
               
            }
            return RedirectToAction("Index");
        }

        //function to get edit data
        public JsonResult GetEditData(FormCollection collection)
        {
            var id = Int64.Parse(collection["id"]);
            var details = db.Roles.Find(id);
            return Json(details);

        }

     

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Roles roles = db.Roles.Find(id);
            roles.IsDeleted = true;
//            db.Roles.Remove(roles); uncomment to delete permanently
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult CheckAttachedMenus()
        {
            var roleId = Int32.Parse(Request.Form["roleId"]);
            var childId = Int32.Parse(Request.Form["child_id"]);
            List<string> success = new List<string>();
            var result = db.UserRoleAllocations.Where(x => x.MenuId == childId && x.RoleId == roleId).FirstOrDefault();
            if (result == null)
            {
                success.Add("false");
            }
            else
            {
                success.Add("attached");
            }
            return Json(success);
        }

        [HttpPost]
        public JsonResult AttachDettachMenu()
        {
            int parentId = Int32.Parse(Request.Form["ParentID"]);
            var childId = Int32.Parse(Request.Form["ChildId"]);
            var roleId = Int32.Parse(Request.Form["RoleId"]);
            List<string> success = new List<string> { };
            if (Request.Form["Action"] == "allocate")
            {
                var res = db.UserRoleAllocations.Where(x => x.ParentId == parentId && x.MenuId == childId && x.RoleId == roleId);
                if (res.Any())
                {
                    success.Add("already-attached");
                }
                else
                {
                    
                    try
                    {
                        db.UserRoleAllocations.Add(new UserRoleAllocation() { ParentId = parentId, MenuId = childId, RoleId = roleId });
                        db.SaveChanges();
                        success.Add("allocate-success");
                    }
                    catch
                    {
                        success.Add("failed");
                    }
                }
            }
            else if (Request.Form["Action"] == "disallocate")
            {
                var delete =db.UserRoleAllocations.Where(x => x.ParentId == parentId && x.MenuId == childId && x.RoleId == roleId).FirstOrDefault();
                if (delete != null)
                {
                    try
                    {
                        db.UserRoleAllocations.Remove(delete);
                        db.SaveChanges();
                        success.Add("dassalocate-success");
                    }
                    catch
                    {
                        success.Add("failed");
                    }
                }
            }
            return Json(success);
        }

        [HttpPost]
        public JsonResult CheckAttachedCrudActionToMenu()
        {
            var menuId = Int32.Parse(Request.Form["menuId"]);
            var permissionId = Int32.Parse(Request.Form["permissionId"]);
            var roleId = Int32.Parse(Request.Form["roleId"]);
            List<string> success = new List<string>();
            try
            {
                var result =
                    db.UserRoleAllocations.FirstOrDefault(x => x.MenuId == menuId && x.RoleId == roleId);
                if (result != null && result.CrudActions == null)
                {
                    //                 success.Add("false");
                }
                else
                {
                    var allPermissions = result.CrudActions.Trim(new Char[] { '{', '}' }).TrimStart(new char[] { ',' }).TrimEnd(new char[] { ',' });
                    var singlePermissions = allPermissions.Split(new char[] { ',' });
//                    Debug.WriteLine("permission id "+ allPermissions);

                    if (singlePermissions.Contains(permissionId.ToString()))
                    {
                        return Json("true");
                    }
                }
            }
            catch
            {

            }

            return Json(success);
        }


        //function to allocate permissions to subscriptions
        [HttpPost]
        public JsonResult AttachDettachCrudAction()
        {

            int menuId = Int32.Parse(Request.Form["MenuId"]);
            var actionId = Int32.Parse(Request.Form["actionId"]);
            var roleId = Int32.Parse(Request.Form["roleId"]);
            var crudActions = db.UserRoleAllocations.FirstOrDefault(x => x.MenuId == menuId && x.RoleId == roleId);
            List<string> status = new List<string> { };

            if (Request.Form["action"] == "allocate")
            {
                if (crudActions != null && crudActions.CrudActions == null)
                {
                    crudActions.CrudActions = "{" + actionId.ToString() + "}";
                    db.SaveChanges();
                    status.Add("permission-attached");
                }
                else if (crudActions != null && crudActions.CrudActions != null)
                {
                    var allPermissions = crudActions.CrudActions.Trim(new Char[] { '{', '}' });
                    // split the permissions and parse them as integers
                    var singlePermissions = allPermissions.Split(new char[] { ',' });
                    if (singlePermissions.Length > 1)
                    {
                        Debug.WriteLine(menuId + " " + actionId + " " + roleId);


                        if (!singlePermissions.Contains(actionId.ToString()))
                        {
                            crudActions.CrudActions = "{" + allPermissions + "," + actionId.ToString() + "}";
                            //                             Debug.Write(permissions.Permissions);
                            try
                            {
                                db.SaveChanges();
                                status.Add("permission-attached");
                            }
                            catch
                            {
                                status.Add("failed-to-attach");
                            }
                        }
                        else
                        {
                            status.Add("already-attached");
                        }
                    }
                    else
                    {
                        if (allPermissions == actionId.ToString())
                        {
                            status.Add("already-attached");
                        }
                        else
                        {
                            crudActions.CrudActions = "{" + allPermissions + "," + actionId.ToString() + "}";
                            try
                            {
                                db.SaveChanges();
                                status.Add("permission-attached");
                            }
                            catch
                            {
                                status.Add("failed-to-attach");
                            }
                        }
                    }


                    //                     Debug.Write(allPermissions);
                }
            }
            else //remove the permission from the menu item
            {
                if (crudActions != null && crudActions.CrudActions != null)
                {
                    var allPermissions = crudActions.CrudActions.Trim(new Char[] {'{', '}'});
                    // split the permissions and parse then as integers
                    if (allPermissions.Length > 1)
                    {
                        var singlePermissions = allPermissions.Split(new char[] {','});
                        var newP = "{";
                        foreach (var perm in singlePermissions)
                        {
                            if (perm != actionId.ToString())
                            {
                                newP = newP + perm + ",";
                            }
                        }
                        newP = newP.TrimEnd(new char[] {','});
                        newP = newP + "}";
                        try
                        {
                            crudActions.CrudActions = newP;
                            db.SaveChanges();
                            status.Add("detached-success");
                        }
                        catch
                        {
                            status.Add("detached-failed");
                        }
                    }
                    else
                    {
                        crudActions.CrudActions = null;
                        try
                        {
                            db.SaveChanges();
                            status.Add("detached-success");
                        }
                        catch
                        {
                            status.Add("detached-failed");
                        }
                    }
                }
            }

            return Json(status);
        }
        [HttpPost]
        public JsonResult AssignStationRoles(FormCollection collection)
        {
            List<object> status = new List<object>();
            var action = collection["Action"];
            var roleId = Int64.Parse(collection["RoleId"]);
            var menuId = Int64.Parse(collection["MenuId"]);
            switch (action)
            {
                case "assign":
                    StationRoles role = new StationRoles
                    {
                        UserId = roleId,
                        StationMenuId = menuId
                    };
                    db.StationRoles.AddOrUpdate(x=> new { x.UserId,x.StationMenuId}, role);

                    db.SaveChanges();
                    status.Add("Success");
                    break;

                case "remove":
                    var record = db.StationRoles.Where(x => x.UserId == roleId && x.StationMenuId == menuId);
                    if (record.Any())
                    {
                        db.StationRoles.Remove(record.FirstOrDefault());
                        db.SaveChanges();
                        status.Add("success");
                    }
                    break;
            }

            return Json(status);
        }
        [HttpPost]
        public JsonResult CheckStationRoles(FormCollection collection)
        {
            bool attached = false;
            var roleId = Int64.Parse(collection["roleId"]);
            var menuId = Int64.Parse(collection["menuId"]);

            if (db.StationRoles.Any(x => x.UserId == roleId && x.StationMenuId == menuId))
            {
                attached = true;
            }
            return Json(attached);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
