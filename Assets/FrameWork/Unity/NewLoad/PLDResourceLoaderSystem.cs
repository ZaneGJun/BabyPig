using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Pld
{
    public enum LoadOption
    {
        Sync,   //同步
        Async,  //异步
    }

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

    }
}
