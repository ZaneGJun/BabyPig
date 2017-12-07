using System;
using System.Collections;
using System.Collections.Generic;
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

	public enum LoadType
	{
		LOAD_FROM_RESOURCES,
		LOAD_COMMON,
		LOAD_FROM_STREAMING_ASSETS,
		LOAD_FROM_PERSISTANT,
	}

	[RequireComponent(typeof(PLDAssetBundleLoader))]
	[RequireComponent(typeof(PLDCommonResourseLoader))]
	public class PLDResourcesManager : PLDMOSingleton<PLDResourcesManager>
	{
			
		private PLDAssetBundleLoader mAssetBundleLoader;
		private PLDCommonResourseLoader mCommonResourcesLoader;

		public override void Init() {
			mAssetBundleLoader = GetComponent<PLDAssetBundleLoader> ();
			mCommonResourcesLoader = GetComponent<PLDCommonResourseLoader> ();

			Debug.Assert (mAssetBundleLoader != null && mCommonResourcesLoader != null);
		}

		/// <summary>
		/// 同步加载资源,Resources实现,非AssetBundle类资源只能加载Resources文件夹中的.
		/// AssetBundle加载请用重载的接口AssetBundle Load(string assetBundlePath, LoadType loadType) 
		/// </summary>
		/// <param name="path">相对于Resources目录的路径，不用带扩展名.</param>
		/// <param name="loadType">只有LOAD_FROM_RESOURCES是可选的.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Load<T>(string path, LoadType loadType) where T : UnityEngine.Object
		{
			switch (loadType) {
			case LoadType.LOAD_FROM_RESOURCES:
				return  mCommonResourcesLoader.LoadFromResources<T>(path);
			case LoadType.LOAD_COMMON:
				Debug.AssertFormat (false, "cannot load common Sync");
				return null;
			case LoadType.LOAD_FROM_STREAMING_ASSETS:
				Debug.AssertFormat (false,"cannot load from StreamingAssets Sync");
				return null;
			case LoadType.LOAD_FROM_PERSISTANT:
				Debug.AssertFormat (false,"cannot load from Persistant Sync");
				return null;
			default:
				return null;
			}
		}

		/// <summary>
		/// 同步加载AssetBundle，用AssetBundle实现，可从Resources,StreamingAssets,Persistant中加载
		/// </summary>
		/// <param name="assetBundlePath">相对于加载目录的路径,不用扩展名.</param>
		/// <param name="loadType">LOAD_FROM_RESOURCES,LOAD_COMMON,LOAD_FROM_STREAMING_ASSETS,LOAD_FROM_PERSISTANT皆可选.</param>
		public AssetBundle Load(string assetBundlePath, LoadType loadType) 
		{
			switch (loadType) {
			case LoadType.LOAD_FROM_RESOURCES:
				return Load<AssetBundle>(assetBundlePath, loadType);
			case LoadType.LOAD_COMMON:
				return mAssetBundleLoader.LoadAssetBundleFullpath (assetBundlePath);
			case LoadType.LOAD_FROM_STREAMING_ASSETS:
				return mAssetBundleLoader.LoadFromStreamingAssets (assetBundlePath);
			case LoadType.LOAD_FROM_PERSISTANT:
				return mAssetBundleLoader.LoadFromPersistant (assetBundlePath);
			default:
				return null;
			}
		}

		/// <summary>
		/// 异步加载资源,Resources实现,非AssetBundle类资源只能加载Resources文件夹中的.
		/// 非AssetBundle资源要从StreamingAsset,Persistant文件夹中加载必须用WWW方式,
		/// 请用重载的方法LoadAsync(string path, LoadType loadType, Action<WWW> callback) 
		/// </summary>
		/// <param name="path">相对于Resources目录的路径，不用带扩展名.</param>
		/// <param name="loadType">只有LOAD_FROM_RESOURCES是可选的.</param>
		/// <param name="callback">回调加载的资源.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void LoadAsync<T>(string path, LoadType loadType, Action<T> callback) where T : UnityEngine.Object
		{
			switch (loadType) {
			case LoadType.LOAD_FROM_RESOURCES:
				mCommonResourcesLoader.LoadFromResourcesAsync<T>(path, callback);
				break;
			case LoadType.LOAD_COMMON:
				Debug.AssertFormat (false, "please use LoadAsync(string path, LoadType loadType, Action<WWW> callback) instead");
				callback (null);
				break;
			case LoadType.LOAD_FROM_STREAMING_ASSETS:
				Debug.AssertFormat (false, "please use LoadAsync(string path, LoadType loadType, Action<WWW> callback) instead");
				callback (null);
				break;
			case LoadType.LOAD_FROM_PERSISTANT:
				Debug.AssertFormat (false, "please use LoadAsync(string path, LoadType loadType, Action<WWW> callback) instead");
				callback (null);
				break;
			default:
				callback (null);
				break;
			}
		}

		/// <summary>
		/// 异步加载资源，用WWW方式实现,可加载StreamingAssets,Persistant文件夹中的资源
		/// 要加载AssetBundle资源最好用重载的方法LoadAsync(string assetBundlePath, LoadType loadType, Action<AssetBundle> callback)
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="loadType">Load type.</param>
		/// <param name="callback">Callback.</param>
		public void LoadAsync(string path, LoadType loadType, Action<WWW> callback) 
		{
			switch (loadType) {
			case LoadType.LOAD_FROM_RESOURCES:
				Debug.AssertFormat (false, "please use  LoadAsync<T>(string path, LoadType loadType, Action<T> callback) instead");
				callback (null);
				break;
			case LoadType.LOAD_COMMON:
				Debug.AssertFormat (false, "cannot load common www");
				callback (null);
				break;
			case LoadType.LOAD_FROM_STREAMING_ASSETS:
				mCommonResourcesLoader.LoadFromStreamingAssetsWWWAsync (path, callback);
				break;
			case LoadType.LOAD_FROM_PERSISTANT:
				mCommonResourcesLoader.LoadFromPersistantWWWAsync (path, callback);
				break;
			default:
				callback (null);
				break;
			}
		}

		/// <summary>
		/// 异步加载AssetBundle，主要用AssetBundle实现，可加载Resources,StreamingAssets,Persistant文件夹中的资源
		/// </summary>
		/// <param name="assetBundlePath">Asset bundle path.</param>
		/// <param name="loadType">Load type.</param>
		/// <param name="callback">Callback.</param>
		public void LoadAsync(string assetBundlePath, LoadType loadType, Action<AssetBundle> callback)
		{
			switch (loadType) {
			case LoadType.LOAD_FROM_RESOURCES:
				LoadAsync<AssetBundle> (assetBundlePath, loadType, callback);
				break;
			case LoadType.LOAD_COMMON:
				mAssetBundleLoader.LoadAssetBundleFullpathAsync (assetBundlePath, callback);
				break;
			case LoadType.LOAD_FROM_STREAMING_ASSETS:
				mAssetBundleLoader.LoadFromStreamingAssetsAsync (assetBundlePath, callback);
				break;
			case LoadType.LOAD_FROM_PERSISTANT:
				mAssetBundleLoader.LoadFromPersistantAsync (assetBundlePath, callback);
				break;
			default:
				callback (null);
				break;
			}
		}
	


		public void toPrint()
		{
			Debug.Log ("sdf");
		}

	}
}

