﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line.Controllers
{
    public class LayoutsController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OffCanvas()
        {
            return View();
        }
	}
}