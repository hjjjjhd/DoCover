using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DoCover.Common
{
    public enum OptionsEnum
    {
        /// <summary>
        /// 网站名称
        /// </summary>
        [Description("网站名称")]
        website_name = 1,
        [Description("站长邮箱")]
        admin_email = 2

    }
}