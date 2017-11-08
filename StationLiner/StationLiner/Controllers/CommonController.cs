using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StationLinerMVC.Models;

namespace StationLinerMVC.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
//        private IMSDataEntities db { get; set; }

        public class Parameters
        {
            public string Table { get; set; }
            public string Column { get; set; }
            public string Value { get; set; }
        }
        public JsonResult Check(Parameters parameters )
        {
            
            object status = new {valid = true};
            using (IMSDataEntities db = new IMSDataEntities())
            {
                try
                {
                    /*var result = db.Database
                        .SqlQuery<string>("SELECT * FROM  dbo." + parameters.Table + "  WHERE " +
                                         parameters.Column +"='"+ parameters.Value+"'").ToList();*/
//                    var result = db.Database
//                        .SqlQuery<Int32>("SELECT Count(*) FROM dbo." + parameters.Table + " WHERE @p0 = '@p1'",parameters.Column,parameters.Value).ToList();
                var result = db.Database
                        .SqlQuery<Int32>(@"SELECT Count(*) FROM dbo." + parameters.Table + " WHERE "+parameters.Column+" = {0}",parameters.Value).FirstOrDefault();
//                    var results = db.Database.SqlQuery<string>("SELECT LOWER(column_name) AS column_name FROM dbo.tables WHERE table_name = @p0", "Staff").ToArray();

                    if (result != default(Int32))
                    {
                        status = new {valid = false, message = parameters.Value +" already exist"};
                    }
                }
                catch(Exception e)
                {
                    status = new { valid = true};
                }
//                var results = db.Menus.ToList();
            }
            return Json(status);
            
        }

       
    }
}