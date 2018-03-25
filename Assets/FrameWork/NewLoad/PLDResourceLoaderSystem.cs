using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pld
{
    public enum LoadOption
    {
        Sync,   //同步
        Async,  //异步
    }

    #region 定义委托

    /// <summary>
    /// 开始委托
    /// </summary>
    public delegate void StartDelgate();

    /// <summary>
    /// 载入过程委托
    /// </summary>
    /// <param name="process"></param>
    public delegate void ProcessDelgate(float process);

    /// <summary>
    /// 成功委托
    /// </summary>
    /// <param name="resultObject">载入得到的对象</param>
    public delegate void SuccessDelgate(object resultObject);

    /// <summary>
    /// 失败委托
    /// </summary>
    /// <param name="msg">失败的信息</param>
    public delegate void ErrorDelgate(string msg);

    /// <summary>
    /// 完成委托
    /// </summary>
    /// <param name="isOk">是否成功</param>
    /// <param name="resultObject">载入得到的对象</param>
    public delegate void FinishDelgate(bool isOk, object resultObject);

    #endregion

    class PLDResourceLoaderSystem : PLDMOSingleton<PLDResourceLoaderSystem>
    {
        public void Awake()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            PLDResourceLoaderCache.CheckGcCollect();
        }

        /// <summary>
        /// 根据相对地址返回绝对地址
        /// 如果是Editor下，返回地址为Assets/ + GAME_ASSET_PATH + "/" + relativePath
        /// 在非Editor下，先判断PERSISTENT_PATH + "/" + GAME_ASSET_PATH + "/" + relativePath下是否存在文件，
        /// 如果存在，就返回该路径，如果不存在，返回包里面StreamAssets的相对路径 STREAMING_PATH + "/" +  GAME_ASSET_PATH + "/" + relativePath
        /// </summary>
        /// <param name="relativePath">相对于GAME_ASSET_PATH的地址</param>
        /// <returns>返回的路径</returns>
        public string GetFullPath(string relativePath)
        {
            string fullpath = "";
#if UNITY_EDITOR
            //检查是否带有扩展名
            Debug.Assert(Utils.HaveExt(relativePath), string.Format("Editor relativePath:{0} must have extension",relativePath));

            fullpath = "Assets/" + PLDGlobalDef.GAME_ASSET_PATH + "/" + relativePath;
            return fullpath;
#else
            string noExtPath = Utils.RemoveExt(relativePath);

            fullpath = GameHotfixWorkPath + "/" + noExtPath;
            if (PLDFileHelper.IsFileExistInPersistent(fullpath))
                return fullpath;

            fullpath = GameOrinigalWorkPath + "/" + noExtPath;
            
            return fullpath;
#endif
        }

        // 游戏热更新工作路径
        public static string GameHotfixWorkPath
        {
            get
            {
                return PLDGlobalDef.PERSISTENT_PATH + "/" + PLDGlobalDef.GAME_ASSET_PATH;
            }
        }

        // 游戏原本的工作路径
        public static string GameOrinigalWorkPath
        {
            get
            {
                return PLDGlobalDef.STREAMING_PATH + "/" + PLDGlobalDef.GAME_ASSET_PATH;
            }
        }

        public static string FileProtocol
        {
            get
            {
                return GetFileProtocol();
            }
        }

        /// <summary>
        /// On Windows, file protocol has a strange rule that has one more slash
        /// </summary>
        /// <returns>string, file protocol string</returns>
        public static string GetFileProtocol()
        {
            string fileProtocol = "file://";
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer
#if !UNITY_5_4_OR_NEWER
                || Application.platform == RuntimePlatform.WindowsWebPlayer
#endif
)
                fileProtocol = "file:///";

            return fileProtocol;
        }

    }
}
