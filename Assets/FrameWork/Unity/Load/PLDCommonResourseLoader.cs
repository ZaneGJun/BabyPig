using System;
using UnityEngine;
namespace Pld
{
	/// <summary>
	/// PLD common resourse loader.
	/// .统一Resources和AssetBundles加载
	/// .类似的加载接口设计，包括同步与异步
	/// .强引用计数管理，Load与Unload匹配
	/// .支持按优先级加载
	/// .支持配置系统开销，异步加载开销
	/// </summary>
	public class PLDCommonResourseLoader
	{
		/// <summary>
		/// Load type.
		/// 加载的种类
		/// </summary>
		public enum LoadType{
			LOAD_RESOURCES,
			LOAD_STREAMING_ASSETS,
			LOAD_PERSISTENT_DATA_PATH
		}

		/// <description>
		/// IOS:
		/// 	Application.dataPath:					Application/xxxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx/xxx.app/Data
		/// 	Application.streamingAssetsPath:		Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw
		/// 	Application.persistentDataPath:			Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Documents
		/// 	Application.temporaryCachePath:			Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/Caches
		/// Android:
		///		Application.dataPath :                  /data/app/xxx.xxx.xxx.apk
		///		Application.streamingAssetsPath :      	jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
		///		Application.persistentDataPath :        /data/data/xxx.xxx.xxx/files
		///		Application.temporaryCachePath :      	/data/data/xxx.xxx.xxx/cache
		/// Windows:
		///		Application.dataPath :                  /Assets
		///		Application.streamingAssetsPath :      	/Assets/StreamingAssets
		///		Application.persistentDataPath :        C:/Users/xxxx/AppData/LocalLow/CompanyName/ProductName
		///		Application.temporaryCachePath :      	C:/Users/xxxx/AppData/Local/Temp/CompanyName/ProductName
		/// Mac:
		///		Application.dataPath :                  /Assets
		///		Application.streamingAssetsPath :      	/Assets/StreamingAssets
		///		Application.persistentDataPath :        /Users/xxxx/Library/Caches/CompanyName/Product Name
		///		Application.temporaryCachePath :     	/var/folders/57/6b4_9w8113x2fsmzx_yhrhvh0000gn/T/CompanyName/Product Name
		/// Windows Web Player:
		///		Application.dataPath :             		file:///D:/MyGame/WebPlayer (即导包后保存的文件夹，html文件所在文件夹)
		///		Application.streamingAssetsPath :
		///		Application.persistentDataPath :
		///		Application.temporaryCachePath :
		///
		/// </description>

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

		/// <summary>
		/// Gets the data from resources.
		/// 从Resources目录中加载资源,用Resources.Load读取，不需要扩展名
		/// </summary>
		/// <returns>The data from resources.</returns>
		/// <param name="path">Path 相对于Resources的路径,文件名不需要带上后缀名.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T getDataFromResources<T>(string path) where T : UnityEngine.Object
		{
			T obj = Resources.Load<T> (path);
			return obj;
		}
		
		/// <summary>
		/// Gets the data from streaming assets.
		/// 从StreamingAssets文件夹中读取资源,必须用WWW的方式读取
		/// </summary>
		/// <returns>The data from streaming assets.</returns>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T getDataFromStreamingAssets<T>(string path) where T : UnityEngine.Object
		{
			
		}


		/// <summary>
		/// Gets the res data.
		/// 获取资源
		/// </summary>
		/// <returns>The res data.</returns>
		/// <param name="path">Path.</param>
		/// <param name="loadType">Load type.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T getResData<T>(string path, LoadType loadType) where T : UnityEngine.Object
		{
			switch(loadType){
			case LoadType.LOAD_RESOURCES:

				break;
			case LoadType.LOAD_PERSISTENT_DATA_PATH:

				break;
			case LoadType.LOAD_STREAMING_ASSETS:

				break;
			default:

				break;
			}
		}
		

	}
}

