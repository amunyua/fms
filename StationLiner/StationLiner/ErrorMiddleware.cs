using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AppFunc = Antlr.Runtime.Misc.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace StationLinerMVC
{
    public class ErrorMiddleware
    {
        private AppFunc next;
        public ErrorMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {

            if (HttpContext.Current.Response.StatusCode == 404)
            {
                HttpContext.Current.Response.Redirect("Error/NotFound");
                await next.Invoke(environment);
            }
        }
    }
}