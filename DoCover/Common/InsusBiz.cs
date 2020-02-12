using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoCover.Common
{
    /// <summary>
    /// Summary description for InsusBiz
    /// </summary>
    public class InsusBiz
    {
        private static HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }

        private static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
        public static string insusCookie { get { return Security.EncryptQueryString("InsusCookie"); } private set { } }
        //定义一个Cookie集合
        private static HttpCookie InsusCookie
        {
            get
            {
                return Request.Cookies[insusCookie] as HttpCookie;
            }
            set
            {
                if (Request.Cookies[insusCookie] != null)
                {
                    Request.Cookies.Remove(insusCookie);
                }
                Response.Cookies.Add(value);
            }
        }

        //New Cookie集合
        private static HttpCookie NewInsusCookie
        {
            get
            {
                HttpCookie httpCookie = new HttpCookie(insusCookie);
                return httpCookie;
            }
        }

        //Remove Cookie集合
        public static void RemoveInsusCookie()
        {
            if (InsusCookie == null)
                Response.Cookies.Remove(insusCookie);
            else
                Response.Cookies[insusCookie].Expires = DateTime.Now.AddDays(-1);
        }
        public string RandomWord { get; set; }

        private static string token = Security.EncryptQueryString("token");
        public static string Token
        {
            get
            {
                return InsusCookie == null ? string.Empty : Security.DecryptQueryString(InsusCookie.Values[token]);
            }
            set
            {
                HttpCookie httpCookie = InsusCookie == null ? NewInsusCookie : InsusCookie;
                httpCookie.Values[username] = Security.EncryptQueryString(value);
                InsusCookie = httpCookie;
            }
        }

        //用户登录状态
        private static string loginok = Security.EncryptQueryString("LoginOk");
        public static bool LoginOk
        {
            get
            {
                return InsusCookie == null ? false : bool.Parse(Security.DecryptQueryString(InsusCookie.Values[loginok]));
            }
            set
            {
                HttpCookie httpCookie = InsusCookie == null ? NewInsusCookie : InsusCookie;
                httpCookie.Values[loginok] = value.ToString();
                InsusCookie = httpCookie;
            }
        }

        //登录用户的帐号
        private static string userid = Security.EncryptQueryString("UserID");
        public static int UserID
        {
            get
            {
                return InsusCookie == null ? -1 : int.Parse(Security.DecryptQueryString(InsusCookie.Values[userid]));
            }
            set
            {
                HttpCookie httpCookie = InsusCookie == null ? NewInsusCookie : InsusCookie;
                httpCookie.Values[userid] = Security.EncryptQueryString(value.ToString());
                InsusCookie = httpCookie;
            }
        }

        //登录用户名称
        private static string username = Security.EncryptQueryString("UserName");
        public static string UserName
        {
            get
            {
                return InsusCookie == null ? string.Empty : Security.DecryptQueryString(InsusCookie.Values[username]);
            }
            set
            {
                HttpCookie httpCookie = InsusCookie == null ? NewInsusCookie : InsusCookie;
                httpCookie.Values[username] = Security.EncryptQueryString(value);
                InsusCookie = httpCookie;
            }
        }

        //登录用户昵称
        private static string usernickname = Security.EncryptQueryString("UserNickName");
        public static string UserNickName
        {
            get
            {
                return InsusCookie == null ? string.Empty : Security.DecryptQueryString(InsusCookie.Values[usernickname]);
            }
            set
            {
                HttpCookie httpCookie = InsusCookie == null ? NewInsusCookie : InsusCookie;
                httpCookie.Values[usernickname] = Security.EncryptQueryString(value);
                InsusCookie = httpCookie;
            }
        }

        //登录用户类型
        private static string usertype = Security.EncryptQueryString("UserType");
        public static string UserType
        {
            get
            {
                return InsusCookie == null ? string.Empty : Security.DecryptQueryString(InsusCookie.Values[usertype]);
            }
            set
            {
                HttpCookie httpCookie = InsusCookie == null ? NewInsusCookie : InsusCookie;
                httpCookie.Values[usertype] = Security.EncryptQueryString(value);
                InsusCookie = httpCookie;
            }
        }
    }
}