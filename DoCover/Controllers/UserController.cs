using Aop.Api.Domain;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using DoCover.Common;
using DoCover.Models;
using MessagingToolkit.QRCode.Codec;
using Newtonsoft.Json;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DoCover.Controllers
{
    public class UserController : Controller
    {
        private static IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(Config.serverUrl, Config.appId, Config.merchant_private_key, Config.version,
                     Config.sign_type, Config.alipay_public_key, Config.charset);
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 前台推广页面
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public ActionResult Index(int id = -1)
        {
            using (var db = new DoCoverEntities())
            {
                var user = db.Users.FirstOrDefault(m => m.user_id == id);
                if (user == null || user.user_type != 2)
                {
                    Response.StatusCode = 404;
                }
                ViewBag.id = id;
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                Response.Redirect("/Home/Index");//登录了，跳转
            return View();
        }
        /// <summary>
        /// 验证登录用户（ajax）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public string Validate(string name, string password)
        {
            ReturnResponse response = new ReturnResponse();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return JsonConvert.SerializeObject(new ReturnResponse() { code = 500, message = "用户名或者密码错误", result = null });
            using (var db = new DoCoverEntities())
            {
                try
                {
                    password = Security.EncryptQueryString(password);
                    Users user = db.Users.FirstOrDefault(m => m.user_name == name && m.user_pwd == password);
                    if (user == null) return JsonConvert.SerializeObject(new ReturnResponse() { code = 500, message = "用户名或者密码错误", result = null });
                    if (user.user_status == false)
                        return JsonConvert.SerializeObject(new ReturnResponse() { code = 501, message = $"账号 {user.user_name} 被禁用，原因：{user.user_remark ?? "无"}", result = null });

                    string data = user.user_type.ToString();
                    var ticket = new FormsAuthenticationTicket(2, user.user_id.ToString(),DateTime.Now, DateTime.Now.AddDays(1), true, data);
                    var cookieValue = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
                    {
                        HttpOnly = true,
                        Secure = FormsAuthentication.RequireSSL,
                        Domain = FormsAuthentication.CookieDomain,
                        Path = FormsAuthentication.FormsCookiePath
                    };
                    cookie.Expires = DateTime.Now.AddDays(1);
                    var context = System.Web.HttpContext.Current;
                    context.Response.Cookies.Remove(cookie.Name);
                    context.Response.Cookies.Add(cookie);

                    user.user_login_num = (user.user_login_num ?? 0) + 1;
                    user.user_last_login_time = DateTime.Now;
                    user.user_last_login_ip = StaticMethod.GetClientIP();
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new ReturnResponse(25001, ex.Message));
                }
            }
            return JsonConvert.SerializeObject(new ReturnResponse() { code = 200, message = "", result = null });
        }

        public string Logout()
        {
            FormsAuthentication.SignOut();
            return JsonConvert.SerializeObject(new ReturnResponse() { code = 200, message = "", result = null });
        }

        //private Orders orderInfo = new Orders();

        [HttpPost]
        public string CreateOrder(string data)
        {
            try
            {
                Orders orderInfo = JsonConvert.DeserializeObject<Orders>(data);
                if (orderInfo == null) return JsonConvert.SerializeObject(new ReturnResponse(21500, "参数错误"));

                AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent(orderInfo);
                string out_trade_no = builder.out_trade_no;

                using (var db = new DoCoverEntities())
                {
                    orderInfo.order_status = 2;
                    db.Orders.Add(orderInfo);
                    string a = "";
                    db.Database.Log = (c) => {
                        a = c;
                    };
                    db.SaveChanges();
                }

                AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);

                //以下返回结果的处理供参考。
                //payResponse.QrCode即二维码对于的链接
                //将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。
                string result = "";
                int code = 0;
                string message = "";
                switch (precreateResult.Status)
                {
                    case ResultEnum.SUCCESS:
                        code = 200;
                        result = DoWaitProcess(precreateResult);
                        message = "生成订单成功";
                        break;
                    case ResultEnum.FAILED:
                        code = 500;
                        message = "生成订单失败";
                        break;

                    case ResultEnum.UNKNOWN:
                        code = 500;
                        if (precreateResult.response == null)
                        {
                            message = "配置或网络异常，请检查后重试";
                        }
                        else
                        {
                            message = "系统异常，请更新外部订单后重新发起请求";
                        }
                        break;
                }
                return JsonConvert.SerializeObject(new ReturnResponse() { code = code, message = message, result = new { order_id = out_trade_no, money = orderInfo.order_price, data = result } });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ReturnResponse() { code = 15001, message = ex.Message});
            }
        }

        [HttpPost]
        public string CheckOrder(string out_trade_no)
        {
            AlipayF2FQueryResult queryResult = serviceClient.tradeQuery(out_trade_no);
            if (queryResult != null)
            {
                if (queryResult.Status == ResultEnum.SUCCESS)
                {
                    return JsonConvert.SerializeObject(new ReturnResponse(200));
                }
            }
            return JsonConvert.SerializeObject(new ReturnResponse(25101)); ;
        }

        /// <summary>
        /// 构造支付请求数据
        /// </summary>
        /// <returns>请求数据集</returns>
        private AlipayTradePrecreateContentBuilder BuildPrecreateContent(Orders orderInfo)
        {
            //线上联调时，请输入真实的外部订单号。
            string out_trade_no = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();
            orderInfo.order_id = out_trade_no;

            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            builder.seller_id = Config.pid;
            //订单编号
            builder.out_trade_no = out_trade_no;
            //订单总金额
            builder.total_amount = orderInfo.order_price.ToString();
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = $"《{orderInfo.order_title}》封面 - {orderInfo.order_price.ToString()}元档";
            //自定义超时时间
            builder.timeout_express = "5m";
            //订单描述
            builder.body = $"《{orderInfo.order_title}》封面 - {orderInfo.order_price.ToString()}元档";
            //门店编号，很重要的参数，可以用作之后的营销
            builder.store_id = Config.pid;
            //操作员编号，很重要的参数，可以用作之后的营销
            builder.operator_id = "docover";

            //传入商品信息详情
            List<GoodsInfo> gList = new List<GoodsInfo>();
            GoodsInfo goods = new GoodsInfo();
            goods.goods_id = orderInfo.order_id;
            goods.goods_name = orderInfo.order_title;
            goods.price = orderInfo.order_price.ToString();
            goods.quantity = "1";
            gList.Add(goods);
            builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="precreateResult">二维码串</param>
        private string DoWaitProcess(AlipayF2FPrecreateResult precreateResult)
        {
            //打印出 preResponse.QrCode 对应的条码
            Bitmap bt;
            string enCodeString = precreateResult.response.QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            //pictureBox1.Image = bt;
            //string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
            // + ".jpg";
            //bt.Save(Server.MapPath("~/images/") + filename);
            //this.Image1.ImageUrl = "~/images/" + filename;

            //轮询订单结果
            //根据业务需要，选择是否新起线程进行轮询
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(LoopQuery);
            Thread myThread = new Thread(ParStart);
            myThread.IsBackground = true;
            object o = precreateResult.response.OutTradeNo;
            myThread.Start(o);

            return "data:image/jpeg;base64," + ImgToBase64String(bt);

        }

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
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="o">订单号</param>
        public void LoopQuery(object o)
        {
            AlipayF2FQueryResult queryResult = new AlipayF2FQueryResult();
            int count = 300;
            int interval = 1000;
            string out_trade_no = o.ToString();

            for (int i = 1; i <= count; i++)
            {
                Thread.Sleep(interval);
                queryResult = serviceClient.tradeQuery(out_trade_no);
                if (queryResult != null)
                {
                    if (queryResult.Status == ResultEnum.SUCCESS)
                    {
                        DoSuccessProcess(queryResult);
                        return;
                    }
                }
            }
            DoFailedProcess(queryResult);
        }

        /// <summary>
        /// 请添加支付成功后的处理
        /// </summary>
        private void DoSuccessProcess(AlipayF2FQueryResult queryResult)
        {
            using (var db = new DoCoverEntities())
            {
                var order = db.Orders.FirstOrDefault(m => m.order_id == queryResult.response.OutTradeNo);
                if (order == null) return;
                string mail = order.order_cust_email;
                order.order_status = 3;
                db.SaveChanges();
                SendMailUse(mail, "才几美工系统", "订单提示", $"你的订单号为{queryResult.response.OutTradeNo}的订单已成功付款,请静候佳音！");
            }
        }
        /// <summary>
        /// 发信
        /// </summary>
        /// <param name="strto">收件人</param>
        /// <param name="name">发信人名称</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        public void SendMailUse(string strto, string name, string subject, string body)
        {

            string host = "smtp.ym.163.com";// 邮件服务器smtp.163.com表示网易邮箱服务器    
            string userName = "admin@49os.com";// 发送端账号   
            string password = "qq12345.";// 发送端密码(这个客户端重置后的密码)




            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = host;//邮件服务器
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(userName, password);//用户名、密码

            //////////////////////////////////////
            string strfrom = userName;
            //string strto = "1457522662@qq.com";
            //string strcc = "2605625733@qq.com";//抄送


            //string subject = "这是测试邮件标题5";//邮件的主题             
            //string body = "测试邮件内容5";//发送的邮件正文  

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(strfrom, name);
            msg.To.Add(strto);
            //msg.CC.Add(strcc);

            msg.Subject = subject;//邮件标题   
            msg.Body = body;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.Normal;//邮件优先级   

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// 请添加支付失败后的处理
        /// </summary>
        private void DoFailedProcess(AlipayF2FQueryResult queryResult)
        {
            using (var db = new DoCoverEntities())
            {
                var order = db.Orders.FirstOrDefault(m => m.order_id == queryResult.response.OutTradeNo);
                if (order == null) return;
                order.order_status = 0;
                db.SaveChanges();
            }
        }
    }
}