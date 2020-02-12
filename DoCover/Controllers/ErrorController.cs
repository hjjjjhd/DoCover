using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoCover.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult InternalServerError()
        {
            Response.StatusDescription = "500 Internal Server Error";
            Response.StatusCode = 500;
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusDescription = "404 Not Found";
            Response.StatusCode = 404;
            return View();
        }
    }
}