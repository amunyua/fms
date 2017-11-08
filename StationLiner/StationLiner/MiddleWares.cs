using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Schema;
using AppFunc = Antlr.Runtime.Misc.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using StationLinerMVC.Models;


namespace StationLinerMVC
{
    public class MiddleWares
    {
//        private IMSDataEntities db = new IMSDataEntities();
        private AppFunc next;
        public MiddleWares(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
//            Debug.WriteLine("Begin Request");
            var url = HttpContext.Current.Request.Url.AbsolutePath;
            var acceptedUrl = "";
            if (url == "/")
            {
                acceptedUrl = "Dashboard";
            }
            else
            {
                var unProcessedUrl = url.TrimStart('/').TrimEnd('/');
                var processedUrl = unProcessedUrl.Split('/');
                acceptedUrl = processedUrl[0];

                if (acceptedUrl != "__browserLink" || acceptedUrl != "Account" || acceptedUrl != "Error" || acceptedUrl != "Staffs")
                {
                    using (IMSDataEntities db = new IMSDataEntities())
                    {
                        var menuName = db.Menus.Where(x => x.Controller == acceptedUrl && x.Action == "Index");
                        if (menuName.Any())
                        {
                            //                            Debug.WriteLine("anuthorised");
                            var id = menuName.First().MenuId;
                            var permissions = db.UserRoleAllocations.Where(x => x.RoleId == Library.UserRole)
                                .Select(x => x.MenuId);
                            if (!permissions.Contains(id))
                            {
                                UrlHelper u = new UrlHelper(HttpContext.Current.Request.RequestContext);

                                var redUrl = u.Action("NotFound", "Error");
                                //                                HttpContext.Current.Response.Headers.Set("Location", redUrl);
                                HttpContext.Current.Response.Redirect("Error/UnAuthorized");

                            }

                        }
                    }
                }

            }

            await next.Invoke(environment);

        }
    }
}