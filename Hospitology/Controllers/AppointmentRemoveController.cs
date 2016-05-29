using Hospitology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospitology.Controllers
{
    public class AppointmentRemoveController : Controller
    {
        public string RemoveData(AppointmentRemove d)
        {

            string isSucces="";

            return isSucces;
        }

        [HttpPost]
        public JsonResult getUser(AppointmentRemove d)
        {

            var userLog = RemoveData(d);
           
            return this.Json(userLog, JsonRequestBehavior.AllowGet);
        }

    }
}
