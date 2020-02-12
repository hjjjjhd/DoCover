using DoCover.Common;
using DoCover.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoCover.Controllers
{
    public class SetController : BaseController
    {
        public ActionResult Info()
        {
            using (var db = new DoCoverEntities())
            {
                ViewBag.User = db.Users.FirstOrDefault(m => m.user_id == UserID);
            }
            return View();
        }

        public ActionResult Password()
        {
            return View();
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateInfo(string data)
        {
            if (!Request.IsAuthenticated)
                return JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过"));
            try
            {
                Users info = JsonConvert.DeserializeObject<Users>(data);
                using (var db = new DoCoverEntities())
                {
                    Users user = db.Users.FirstOrDefault(m => m.user_id == UserID);
                    user.user_nick_name = info.user_nick_name;
                    user.user_qq = info.user_qq;
                    user.user_phone = info.user_phone;
                    user.user_email = info.user_email;
                    user.user_last_update_ip = StaticMethod.GetClientIP();
                    user.user_last_update_time = DateTime.Now;
                    db.SaveChanges();
                }
                return JsonConvert.SerializeObject(new ReturnResponse(200));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ReturnResponse(25001,ex.Message));
            }
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdatePassword(string oldPassword, string newPassword)
        {
            if (!Request.IsAuthenticated)
                return JsonConvert.SerializeObject(new ReturnResponse(24000, "身份验证未通过"));
            try
            {
                using (var db = new DoCoverEntities())
                {
                    Users user = db.Users.FirstOrDefault(m => m.user_id == UserID);
                    if(user.user_pwd != Security.EncryptQueryString(oldPassword))
                        return JsonConvert.SerializeObject(new ReturnResponse(24001, "当前密码不正确"));
                    user.user_pwd = Security.EncryptQueryString(newPassword);
                    user.user_last_update_ip = StaticMethod.GetClientIP();
                    user.user_last_update_time = DateTime.Now;
                    db.SaveChanges();
                }
                return JsonConvert.SerializeObject(new ReturnResponse(200));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ReturnResponse(25001, ex.Message));
            }
        }
    }
}