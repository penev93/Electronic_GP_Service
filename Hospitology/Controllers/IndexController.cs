﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospitology.Controllers
{
    public class IndexController : Controller
    {
        
        public ActionResult Index()
        {
            return View("~/Views/Index/Index.cshtml");
        }

    }
}
