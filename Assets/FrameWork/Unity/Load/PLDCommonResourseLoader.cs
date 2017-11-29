using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
	public static class PLDCommonResourseLoader
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
		/// 从StreamingAssets文件夹中读取资源,必须用WWW的方式读取,必须要扩展名
		/// </summary>
		/// <returns>The data from streaming assets.</returns>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T getDataFromStreamingAssets<T>(string path) where T : UnityEngine.Object
		{
			string fullpath = PLDGlobalDef.STREAMING_PATH + path;
			WWW www = new WWW (fullpath);
			
			if (typeof(T) == typeof(UnityEngine.Texture)) {
			}
			return null;
		}
		
		/// <summary>
		/// Gets the asset bundle from streaming assets.
		/// </summary>
		/// <returns>The asset bundle from streaming assets.</returns>
		/// <param name="name">Name AssetBundle的name.</param>
		public static AssetBundle getAssetBundleFromStreamingAssets(string name) 
		{
			string fullpath = PLDGlobalDef.STREAMING_PATH + name;
			WWW www = new WWW (fullpath);
			return www.assetBundle;
		}

		/// <summary>
		/// Gets the data editor.
		/// 编辑器模式下获取资源 
		/// </summary>
		/// <returns>The data editor.</returns>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T getDataEditor<T>(string path) where T : UnityEngine.Object
		{
		#if UNITY_EDITOR
			T obj = AssetDatabase.LoadAssetAtPath<T>(path);
			return obj;
		#endif
		}

		/// <summary>
		/// Gets the res data.
		/// 获取资源
		/// </summary>
		/// <returns>The res data.</returns>
		/// <param name="path">Path 相对GAME_ASSETS的路径,必须带扩展名.</param>
		/// <param name="loadType">Load type.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T getResData<T>(string path, LoadType loadType) where T : UnityEngine.Object
		{
			#if UNITY_EDITOR
				string fullpath = PLDGlobalDef.GAME_ASSET_PATH + path;
				return getDataEditor<T>(fullpath);
			#endif

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

