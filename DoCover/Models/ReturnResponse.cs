using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoCover.Models
{
    public class ReturnResponse
    {
        /// <summary>
        /// 操作状态码，详细参考返回代码对照表
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 操作消息，详细参考返回代码对照表
        /// </summary>
        public object message { get; set; }

        public object result { get; set; }

        public ReturnResponse()
        {
            code = 00000;
            message = "";
        }

        public ReturnResponse(int Code, string Message)
        {
            this.code = Code;
            this.message = Message;
        }
        public ReturnResponse(int Code)
        {
            this.code = Code;
            this.message = "";
        }
    }
}