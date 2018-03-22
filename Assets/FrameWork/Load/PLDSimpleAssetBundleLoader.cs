using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
	public class PLDSimpleAssetBundleLoader
	{
		/////////////////////////////////////////////////////////////////////////////////

		#region SyncLoad

		/// <summary>
		/// Loads the asset bundle.
		/// 同步在路径读取AssetBundle
		/// 支持任何压缩类型的AssetBundle。
		/// 如果用lzma压缩，数据会解压存放到内存当中。
		/// 可以直接从磁盘读取未压缩和chunk压缩的AssetBundle
		/// </summary>
		/// <returns>The asset bundle.</returns>
		/// <param name="fullpath">Fullpath.</param>
		public AssetBundle LoadAssetBundleFullpath(string fullpath)
		{
			return AssetBundle.LoadFromFile (fullpath);
		}

		/// <summary>
		/// Loads from streaming assets.
		/// 从StreamingAssets中同步读取AssetBundle
		/// </summary>
		/// <returns>The from streaming assets.</returns>
		/// <param name="assetbundle">Assetbundle.</param>
		public AssetBundle LoadFromStreamingAssets(string assetbundle)
		{
			string fullpath = PLDGlobalDef.STREAMING_PATH + "/" + assetbundle;
			return LoadAssetBundleFullpath (fullpath);
		}

		/// <summary>
		/// 从Persistant中同步读取AssetBundle
		/// </summary>
		/// <returns>The from persistant.</returns>
		/// <param name="assetbundle">Assetbundle.</param>
		public AssetBundle LoadFromPersistant(string assetbundle)
		{
			string fullpath = PLDGlobalDef.PERSISTENT_PATH + "/" + assetbundle;
			return LoadAssetBundleFullpath (fullpath);
		}

		#endregion

		/////////////////////////////////////////////////////////////////////////////////

		#region AsyncLoad

		/// <summary>
		/// 加载协程实现.
		/// </summary>
		/// <returns>The async coroutine.</returns>
		/// <param name="path">Path.</param>
		/// <param name="callback">Callback.</param>
		static IEnumerator LoadAsyncCoroutine(string path, Action<AssetBundle> callback)
		{
			AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync (path);
			yield return req;

			callback (req.assetBundle);
		}

		/// <summary>
		/// 开启协程进行异步加载，完成回调.
		/// </summary>
		/// <param name="fullpath">Fullpath.</param>
		/// <param name="callback">Callback.</param>
		public void LoadAssetBundleFullpathAsync(string fullpath, Action<AssetBundle> callback)
		{
            PLDResourcesManager.Instance.StartCoroutine(LoadAsyncCoroutine(fullpath, callback));
		}

		/// <summary>
		/// 从StreamingAssets中异步加载AssetBundle.
		/// </summary>
		/// <param name="assetbundle">Assetbundle.</param>
		/// <param name="callback">Callback.</param>
		public void LoadFromStreamingAssetsAsync(string assetbundle, Action<AssetBundle> callback)
		{
			string fullpath = PLDGlobalDef.STREAMING_PATH + "/" + assetbundle;
			LoadAssetBundleFullpathAsync (fullpath, callback);
		}

		/// <summary>
		/// 从Persistant中异步加载AssetBundle.
		/// </summary>
		/// <param name="assetbundle">Assetbundle.</param>
		/// <param name="callback">Callback.</param>
		public void LoadFromPersistantAsync(string assetbundle, Action<AssetBundle> callback)
		{
			string fullpath = PLDGlobalDef.PERSISTENT_PATH + "/" + assetbundle;
			LoadAssetBundleFullpathAsync (fullpath, callback);
		}

		#endregion

		/////////////////////////////////////////////////////////////////////////////////

		#region WWWAsyncLoad

		/// <summary>
		/// 协程实现，用WWW方式
		/// </summary>
		/// <returns>The from WWW coroutine.</returns>
		/// <param name="path">Path.</param>
		/// <param name="callback">Callback.</param>
		static IEnumerator LoadFromWWWCoroutine(string path, Action<AssetBundle> callback)
		{
			WWW www = new WWW (path);
			yield return www;
			callback (www.assetBundle);

			www.Dispose ();
			www = null;
		}

		/// <summary>
		/// 开启协程从指定路径加载AssetBundle,用WWW的方式.
		/// </summary>
		/// <param name="fullpath">Fullpath.</param>
		/// <param name="callback">Callback.</param>
		public void LoadAssetBundleWWWFullpathAsync(string fullpath, Action<AssetBundle> callback)
		{
            PLDResourcesManager.Instance.StartCoroutine(LoadFromWWWCoroutine (fullpath, callback));
		}

		/// <summary>
		/// 从StreamingAssets中异步加载AssetBundle,用WWW的方式加载.
		/// </summary>
		/// <param name="assetbundle">Assetbundle.</param>
		/// <param name="callback">Callback.</param>
		public void LoadFromStreamingAssetsWWWAsync(string assetbundle, Action<AssetBundle> callback)
		{
			string fullpath = PLDGlobalDef.STREAMING_PATH + "/" + assetbundle;
			LoadAssetBundleWWWFullpathAsync (fullpath, callback);
		}

		/// <summary>
		/// 从Persistant中异步加载AssetBundle,用WWW的方式加载.
		/// </summary>
		/// <param name="assetbundle">Assetbundle.</param>
		/// <param name="callback">Callback.</param>
		public void LoadFromPersistantWWWAsync(string assetbundle, Action<AssetBundle> callback)
		{
			string fullpath = PLDGlobalDef.PERSISTENT_PATH + "/" + assetbundle;
			LoadAssetBundleWWWFullpathAsync (fullpath, callback);
		}

		#endregion
	}
}

