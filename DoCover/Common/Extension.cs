using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoCover.Common
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static class Extension
    {
        #region String拓展
        #endregion

        #region Enum拓展
        /// <summary>
        /// 获取枚举的中文描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>没有描述返回空字符串</returns>
        public static string GetDescription(this Enum obj)
        {
            string objName = obj.ToString();
            Type t = obj.GetType();
            System.Reflection.FieldInfo fi = t.GetField(objName);
            System.ComponentModel.DescriptionAttribute[] arrDesc = (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (arrDesc.Length > 0)
                return arrDesc[0].Description;
            return "";
        }
        #endregion
    }
}