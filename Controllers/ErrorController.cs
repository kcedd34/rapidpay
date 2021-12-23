﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RapidPay.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error

        public ActionResult InternalServerError()
        {
            return new HttpStatusCodeResult(401);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return new HttpStatusCodeResult(401);
        }
    }
}