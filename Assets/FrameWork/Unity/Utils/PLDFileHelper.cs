using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    class PLDFileHelper
    {
        /// <summary>
        /// 判断文件是否存在于磁盘上
        /// 在StreamingAssets和Resources中的文件因为不存在于磁盘上，不能用此接口判断
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool IsFileExist(string path)
        {
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 判断Persistent文件夹中是否存在文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExistInPersistent(string path)
        {
            return IsFileExist(path);
        }
    }
}
