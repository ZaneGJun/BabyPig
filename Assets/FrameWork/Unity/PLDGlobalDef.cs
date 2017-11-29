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
		//游戏资源存放相对地址
		public static string GAME_ASSET_PATH{
			get
			{
			#if UNITY_EDITOR
				return "Assets/GameAssets/";
			#else
				return "";
			#endif
			}
		}

		/// <summary>
		/// Sets the PERSISTEN t PAT h DATABAS.
		/// 一个可读可写的目录，程序安装后生成，用于保存热更新下载的资源
		/// </summary>
		/// <value>The PERSISTEN t PAT h DATABAS.</value>
		public static string PERSISTENT_PATH
		{
			get 
			{
			#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
				return Application.persistentDataPath;
			#else
				return Application.persistentDataPath;
			#endif
			}
		}

		/// <summary>
		/// Gets the STREAMIN g PAT h DATABAS.
		/// StreamingAssets目录
		/// </summary>
		/// <value>The STREAMIN g PAT h DATABAS.</value>
		public static string STREAMING_PATH 
		{
			get 
			{
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
				return "file://" + Application.streamingAssetsPath;
			#elif UNITY_ANDROID
				return Application.streamingAssetsPath;
			#elif UNITY_IOS
				return Application.streamingAssetsPath;
			#else
				return Application.streamingAssetsPath;
			#endif
			}
		}
		
		
	}
}