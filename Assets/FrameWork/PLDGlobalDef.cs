using System;
using UnityEngine;

namespace Pld{
    /// <description>
    /// IOS:
    /// Application.dataPath:					Application/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx/xxx.app/Data
    /// Application.streamingAssetsPath:		Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw
    /// Application.persistentDataPath:			Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Documents
    /// Application.temporaryCachePath:			Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches
    ///
    /// Android:
    ///	Application.dataPath :                  /data/app/xxx.xxx.xxx.apk
    ///	Application.streamingAssetsPath :      	jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
    ///	Application.persistentDataPath :        /data/data/xxx.xxx.xxx/files
    ///	Application.temporaryCachePath :      	/data/data/xxx.xxx.xxx/cache
    ///
    /// Windows:
    ///	Application.dataPath :                  /Assets
    ///	Application.streamingAssetsPath :      	/Assets/StreamingAssets
    ///	Application.persistentDataPath :        C:/Users/xxxx/AppData/LocalLow/CompanyName/ProductName
    ///	Application.temporaryCachePath :      	C:/Users/xxxx/AppData/Local/Temp/CompanyName/ProductName
    ///
    /// Mac:
    ///	Application.dataPath :                  /Assets
    ///	Application.streamingAssetsPath :      	/Assets/StreamingAssets
    ///	Application.persistentDataPath :        /Users/xxxx/Library/Caches/CompanyName/Product Name
    ///	Application.temporaryCachePath :     	/var/folders/57/6b4_9w8113x2fsmzx_yhrhvh0000gn/T/CompanyName/Product Name
    ///
    /// Windows Web Player:
    ///	Application.dataPath :             		file:///D:/MyGame/WebPlayer (即导包后保存的文件夹，html文件所在文件夹)
    ///	Application.streamingAssetsPath :
    ///	Application.persistentDataPath :
    ///	Application.temporaryCachePath :
    /// </description>

    public static class PLDGlobalDef
    {
        /// <summary>
        /// 游戏资源存放相对地址
        /// </summary>
        public static string GAME_ASSET_PATH
        {
            get
            {
#if UNITY_EDITOR
                return "Assets/GameAssets";
#else
				return "GameAssets";
#endif
            }
        }

        /// <summary>
        /// 临时目录
        /// </summary>
        public static string GAME_TEMP_PATH 
        {
            get
            {
                return "_Temp";
            }
        }

        /// <summary>
        /// 游戏缓存目录
        /// </summary>
        public static string GAME_CACHE_PATH
        {
            get
            {
                return "_Cache";
            }
        }

        /// <summary>
        /// 一个可读可写的目录，程序安装后生成
        /// </summary>
        public static string PERSISTENT_PATH
		{
			get 
			{
				return Application.persistentDataPath;
			}
		}

		/// <summary>
		/// StreamingAssets目录
		/// </summary>
		public static string STREAMING_PATH 
		{
			get 
			{
				return Application.streamingAssetsPath;
            }
		}
		
		
	}
}
