using DoCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoCover.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 在操作方法调用后
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (noAuth)
                filterContext.Result = Content(JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过")));//视图
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 在操作方法调用前
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Request.IsAuthenticated) return;
            Response.Redirect("/User/Login");//未登录，跳转
        }

        /// <summary>
        /// 没有操作权限
        /// </summary>
        public bool noAuth { get; set; } = false;

        /// <summary>
        /// 登录用户信息
        /// </summary>
        public int UserType
        {
            get
            {
                try
                {
                    var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string data = ticket.UserData;
                    return int.Parse(data);
                }
                catch { return -1; }
            }
        }

        public int UserID
        {
            get
            {
                try
                {
                    return int.Parse(User.Identity.Name);
                }
                catch { return -1; }
            }
        }
    }
}