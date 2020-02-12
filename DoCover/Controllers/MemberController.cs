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
    public class MemberController : BaseController
    {
        // GET: Member
        public ActionResult List()
        {
            if (StaticMethod.GetType(UserID) != 0)
                noAuth = true;
            return View();
        }

        public ActionResult AddMember()
        {
            if (StaticMethod.GetType(UserID) != 0)
                noAuth = true;
            return View();
        }

        public ActionResult EditMember(int user_id)
        {
            if (StaticMethod.GetType(UserID) != 0)
                noAuth = true;
            using (var db = new DoCoverEntities())
            {
                Users user = db.Users.FirstOrDefault(m => m.user_id == user_id && m.user_type != 0);
                if (user == null)
                    return Content(JsonConvert.SerializeObject(new ReturnResponse() { code = 31002, message = "该用户不存在" }));
                ViewBag.User = user;
            }
            return View();
        }

        /// <summary>
        /// 获取成员接口
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">一页数据</param>
        /// <returns></returns>
        public string GetMember(int page, int limit, int user_id = 0, string user_name = "", int user_type = 0, int user_status = 0)
        {
            if (!Request.IsAuthenticated || StaticMethod.GetType(UserID) != 0)
                return JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过"));
            int count = 0;
            List<Users> users;
            IEnumerable<Users> temp;
            using (var db = new DoCoverEntities())
            {
                temp = db.Users.Where(m => m.user_type != 0);
                if (user_id > 0)
                    temp = temp.Where(m => m.user_id == user_id);
                if (user_name != "")
                    temp = temp.Where(m => m.user_name == user_name);
                if (user_type > 0)
                    temp = temp.Where(m => m.user_type == user_type);
                if (user_status == 1)
                    temp = temp.Where(m => m.user_status != false);
                if (user_status == 2)
                    temp = temp.Where(m => m.user_status == false);
                users = temp.ToList();
            }
            count = users.Count();
            users = users.Skip((page - 1) * limit).Take(limit).ToList();
            if (users.Count == 0)
                return JsonConvert.SerializeObject(new { code = 201, msg = "暂时没有数据", count = count, data = users });
            users.ForEach(m => m.user_pwd = "别看了，没密码");
            return JsonConvert.SerializeObject(new { code = 0, msg = "", count = count, data = users });
        }

        /// <summary>
        /// 添加成员信息（密码默认123456）
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost]
        public string AddMember(string data)
        {
            if (!Request.IsAuthenticated || StaticMethod.GetType(UserID) != 0)
                return JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过"));
            Users user = JsonConvert.DeserializeObject<Users>(data);
            user.user_pwd = Security.EncryptQueryString("123456");
            user.user_reg_ip = StaticMethod.GetClientIP();
            user.user_reg_time = DateTime.Now;
            using (var db = new DoCoverEntities())
            {
                if (db.Users.Count(m => m.user_name == user.user_name) > 0)
                {
                    return JsonConvert.SerializeObject(new ReturnResponse() { code = 31001, message = "用户名重复" });
                }
                db.Users.Add(user);
                db.SaveChanges();
            }
            return JsonConvert.SerializeObject(new ReturnResponse(200));
        }

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost]
        public string EditMember(string data)
        {
            if (!Request.IsAuthenticated || StaticMethod.GetType(UserID) != 0)
                return JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过"));
            Users info = JsonConvert.DeserializeObject<Users>(data);
            info.user_last_update_ip = StaticMethod.GetClientIP();
            info.user_last_update_time = DateTime.Now;
            using (var db = new DoCoverEntities())
            {
                Users user = db.Users.FirstOrDefault(m => m.user_id == info.user_id);
                if (user == null)
                    return JsonConvert.SerializeObject(new ReturnResponse() { code = 31002, message = "该用户不存在" });
                if (user.user_name != info.user_name)
                    if (db.Users.Count(m => m.user_name != user.user_name && m.user_name == info.user_name) > 0)
                    {
                        return JsonConvert.SerializeObject(new ReturnResponse() { code = 31001, message = "用户名重复" });
                    }
                if(info.user_pwd!=null&& info.user_pwd != "")
                    user.user_pwd = Security.EncryptQueryString(info.user_pwd);
                user.user_name = info.user_name;
                user.user_nick_name = info.user_nick_name;
                user.user_qq = info.user_qq;
                user.user_phone = info.user_phone;
                user.user_email = info.user_email;
                user.user_last_update_ip = info.user_last_update_ip;
                user.user_last_update_time = info.user_last_update_time;
                db.SaveChanges();
            }
            return JsonConvert.SerializeObject(new ReturnResponse(200));
        }

        /// <summary>
        /// 修改成员状态
        /// </summary>
        /// <param name="user_id">成员id</param>
        /// <param name="user_status">成员状态</param>
        /// <param name="user_remark">备注</param>
        /// <returns></returns>
        [HttpPost]
        public string EditMemberStatus(int user_id, bool user_status, string user_remark = "")
        {
            if (!Request.IsAuthenticated || StaticMethod.GetType(UserID) != 0)
                return JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过"));
            using (var db = new DoCoverEntities())
            {
                Users user = db.Users.FirstOrDefault(m => m.user_id == user_id);
                if (user == null)
                    return JsonConvert.SerializeObject(new ReturnResponse() { code = 31002, message = "该用户不存在" });

                user.user_status = user_status;
                user.user_remark = user_remark;
                user.user_last_update_ip = StaticMethod.GetClientIP();
                user.user_last_update_time = DateTime.Now; ;
                db.SaveChanges();
            }
            return JsonConvert.SerializeObject(new ReturnResponse(200));
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="user_id">成员id</param>
        /// <returns></returns>
        [HttpPost]
        public string DelMember(int user_id)
        {
            if (!Request.IsAuthenticated || StaticMethod.GetType(UserID) != 0)
                return JsonConvert.SerializeObject(new ReturnResponse(24000,"身份验证未通过"));
            using (var db = new DoCoverEntities())
            {
                Users user = db.Users.FirstOrDefault(m => m.user_id == user_id);
                if (user == null)
                    return JsonConvert.SerializeObject(new ReturnResponse() { code = 31002, message = "该用户不存在" });
                if ((user.Orders_Create != null && user.Orders_Create.Count > 0) || (user.Orders_Finish != null && user.Orders_Finish.Count > 0))
                    return JsonConvert.SerializeObject(new ReturnResponse() { code = 21001, message = "该用户有关联订单，推荐使用禁用功能" });
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return JsonConvert.SerializeObject(new ReturnResponse(200));
        }
    }
}