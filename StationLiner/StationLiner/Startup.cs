using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Owin;
using AppFunc = Antlr.Runtime.Misc.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using WebGrease.Css.Ast.Selectors;

[assembly: OwinStartupAttribute(typeof(StationLinerMVC.Startup))]
namespace StationLinerMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

//            app.Use(typeof(MiddleWares));
        }   

    }
}
