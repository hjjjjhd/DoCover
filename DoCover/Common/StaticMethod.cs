using DoCover.Models;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace DoCover.Common
{
    public class StaticMethod
    {

        #region 公共方法
        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return "data:image/jpeg;base64,"+ Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 
        /// </summary> 
        public static string GetClientIP()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理 
                if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIPAddress(temparyip[i])
                            && temparyip[i].Substring(0, 3) != "10."
                            && temparyip[i].Substring(0, 7) != "192.168"
                            && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i];    //找到不是内网的地址 
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式 
                        return result;
                    else
                        result = null;    //代理中的内容 非IP，取IP 
                }

            }

            string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;

            return result;

        }
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^/d{1,3}[/.]/d{1,3}[/.]/d{1,3}[/.]/d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetType(int id)
        {
            try
            {
                using (var db = new DoCoverEntities())
                {
                    Users user = db.Users.FirstOrDefault(m => m.user_id == id);
                    if (user == null) return -1;
                    return (int)user.user_type;
                }
            }
            catch{ return -1; }
        }

        /// <summary>
        /// 随机字母
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GetRandomWord(int Length)
        {
            char[] constant = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            StringBuilder newRandom = new System.Text.StringBuilder(constant.Length);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(constant.Length - 1)]);

            }
            return newRandom.ToString();
        }
        #endregion

        #region 私有方法
                /// <summary>
                /// 获取ip地址
                /// </summary>
                /// <returns></returns>
                private static string GetServerIpv4()
                {
                    IPHostEntry host;
                    string localIP = "";
                    host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress ip in host.AddressList)
                    {
                        Console.WriteLine(ip.AddressFamily.ToString());
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localIP = ip.ToString();
                            return localIP;
                        }
                    }
                    return localIP;
                }
                #endregion
    }
}