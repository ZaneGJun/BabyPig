using System;
using System.IO;
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

        /// <summary>
        /// 去掉扩展名
        /// </summary>
        public static string RemoveExt(string path)
        {
            string extension = Path.GetExtension(path);
            path = path.Substring(0, path.Length - extension.Length); // remove extensions
            return path;
        }

        /// <summary>
        /// 是否有扩展名
        /// </summary>
        public static bool HaveExt(string path)
        {
            string extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
                return false;

            return true;
        }

        /// <summary>
        /// 将字节码保存到本地
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        public static void SaveBytesToLocal(byte[] bytes, string path)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                File.WriteAllBytes(path, bytes);
            }else if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string fullpath = PLDGlobalDef.PERSISTENT_PATH + "/" + path;
                File.WriteAllBytes(fullpath, bytes);
            }
        }

        /// <summary>
        /// 读取二进制文件
        /// 二进制文件存放在StreamingAssets中，用WWW来读取
        /// </summary>
        /// <param name="path"></param>
        public static void ReadBytes(string path, FinishDelgate callback)
        {
            PLDWWWLoader loader = PLDWWWLoader.Create(path);
            loader.LoadAsync(callback);
        }

        /// <summary>
        /// 字节数组复制
        /// </summary>
        /// <param name="src">源</param>
        /// <param name="srcOffset">src的字节偏移</param>
        /// <param name="dest">目标</param>
        /// <param name="destOffset">dest的字节偏移</param>
        /// <param name="length">要复制的字节数</param>
        public static void CopyBytes(byte[] src, int srcOffset, byte[] dest, int destOffset, int length)
        {
            try
            {
                Buffer.BlockCopy(src, srcOffset, dest, destOffset, length);
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
            }
        }

        /// <summary>
        /// 打开窗口选择本地文件，返回路径
        /// </summary>
        /// <returns>已选择的本地文件路径</returns>
        public static string ShowDialogGetSelectFilePath()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            OpenFileName config = LocalDialog.GetOpenFileNameConfig();
            if(LocalDialog.GetOpenFileName(config))
            {
                Debug.Log("Get Select File Path:" + config.file);
                return config.file;
            }
#else

#endif
            return "";
        }

        /// <summary>
        /// 打开文件选择文件保存到本地的路径
        /// </summary>
        /// <returns>返回的路径</returns>
        public static string ShowDialogGetSaveFilePath()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            OpenFileName config = LocalDialog.GetSaveFileNameConfig();
            if (LocalDialog.GetSaveFileName(config))
            {
                Debug.Log("Get Select File Path:" + config.file);
                return config.file;
            }
#else
#endif
            return "";
        }
    }
}
