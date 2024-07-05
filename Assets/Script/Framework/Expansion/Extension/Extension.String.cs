using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,06,15:50
// @Description:
// </summary>

namespace zhaorh.UI
{
    public static partial class Extension
    {
        /// <summary>
        /// 将字符串转变数字，如果不是数字就返回0
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static int ToInteger (this string me)
        {
            return me.IsInteger () ? int.Parse (me) : 0;
        }
        /// <summary>
        /// 判断字符串是不是全为数字
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static bool IsInteger (this string me)
        {
            var isInt = true;
            if (string.IsNullOrEmpty(me))
            {
                return false;
            }

            var startIndex = 0;
            //开头是不是-号，是的话继续保留
            if (me.Length > 0 && me[0] == '-')
            {
                startIndex = 1;
            }

            for (var i = startIndex; i < me.Length && isInt; i++)
            {
                isInt = char.IsNumber (me [i]);
            }
            return isInt;
        }
        //首字母转为大写
        public static string Capitalize(this string s)
        {
            if (String.IsNullOrEmpty(s)) {
                throw new ArgumentException("String is mull or empty");
            }
 
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
    }
}