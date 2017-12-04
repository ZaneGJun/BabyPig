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

	[RequireComponent(typeof(PLDAssetBundleLoader))]
	[RequireComponent(typeof(PLDCommonResourseLoader))]
	public class PLDResourcesManager : PLDMOSingleton<PLDResourcesManager>
	{
		public enum LoadType
		{
			LOAD_COMMON,
			LOAD_FROM_RESOURCES,
			LOAD_FROM_STREAMING_ASSETS,
			LOAD_FROM_PERSISTANT,
		}

		private PLDAssetBundleLoader mAssetBundleLoader;
		private PLDCommonResourseLoader mCommonResourcesLoader;

		void Awake()
		{
			
		}

		void Start()
		{
			
		}

		void Update()
		{
			
		}

		public override void Init() {
			mAssetBundleLoader = GetComponent<PLDAssetBundleLoader> ();
			mCommonResourcesLoader = GetComponent<PLDCommonResourseLoader> ();

			Debug.Assert (mAssetBundleLoader != null && mCommonResourcesLoader != null);
		}


		public UnityEngine.Object Load<T>(string path, LoadType loadType) where T : UnityEngine.Object
		{
			bool isAssetBudle = typeof(T) == typeof(AssetBundle) ? true : false;

			if (isAssetBudle) {
				switch (loadType) {
				case LoadType.LOAD_FROM_RESOURCES:
					return  mCommonResourcesLoader.LoadFromResources<AssetBundle>(path);
				case LoadType.LOAD_COMMON:
					return mAssetBundleLoader.LoadAssetBundleFullpath (path);
				case LoadType.LOAD_FROM_STREAMING_ASSETS:
					return mAssetBundleLoader.LoadFromStreamingAssets (path);
				case LoadType.LOAD_FROM_PERSISTANT:
					return mAssetBundleLoader.LoadFromPersistant (path);
				default:
					return null;
				}
			} else {
				switch (loadType) {
				case LoadType.LOAD_FROM_RESOURCES:
					return  mCommonResourcesLoader.LoadFromResources<T>(path);
				case LoadType.LOAD_COMMON:
				case LoadType.LOAD_FROM_STREAMING_ASSETS:
				case LoadType.LOAD_FROM_PERSISTANT:
				default:
					return null;
				}
			}
		}

		public void LoadAsync<T>(string path, LoadType loadType, Action<UnityEngine.Object> callback) where T : UnityEngine.Object
		{
			bool isAssetBudle = typeof(T) == typeof(AssetBundle) ? true : false;

			if (isAssetBudle) {
				switch (loadType) {
				case LoadType.LOAD_FROM_RESOURCES:
					mCommonResourcesLoader.LoadFromResourcesAsync<AssetBundle>(path, callback);
					break;
				case LoadType.LOAD_COMMON:
					mAssetBundleLoader.LoadAssetBundleFullpathAsync (path, callback);
					break;
				case LoadType.LOAD_FROM_STREAMING_ASSETS:
					mAssetBundleLoader.LoadFromStreamingAssetsAsync (path, callback);
					break;
				case LoadType.LOAD_FROM_PERSISTANT:
					mAssetBundleLoader.LoadFromPersistantAsync (path, callback);
					break;
				default:
					callback (null);
					break;
				}
			} else {
				switch (loadType) {
				case LoadType.LOAD_FROM_RESOURCES:
					mCommonResourcesLoader.LoadFromResourcesAsync<T>(path, callback);
					break;
				case LoadType.LOAD_COMMON:
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
		}
	


		public void toPrint()
		{
			Debug.Log ("sdf");
		}

	}
}

