using DoCover.Common;
using DoCover.Filter;
using DoCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoCover.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (var db = new DoCoverEntities())
            {
                ViewBag.User = db.Users.FirstOrDefault(m => m.user_id == UserID);
            }
            return View();
        }

        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Link()
        {
            ViewBag.id = UserID;
            return View();
        }

        [HttpPost]
        public string post()
        {
            return "ok";
        }
    }
}
