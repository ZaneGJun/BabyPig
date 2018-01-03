﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 拼接
        /// </summary>
        public static string ConnectFilePath(string connectChar, params string[] strList)
        {
            if(strList.Length <= 0)
            {
                return "";
            }
            else if(strList.Length == 1)
            {
                return string.Format("{0}{1}", strList[0], connectChar);
            }

            string resStr = strList[0];
            for(int i=0; i<strList.Length - 1; i++)
            {
                resStr = string.Format("{0}{1}{2}", resStr, connectChar, strList[i + 1]);
            }

            return resStr;
        }

    }
}
