using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pld
{
	public class PLDSimpleResourseLoader 
	{
		//////////////////////////////////////////////////////////////////////////

		#region ResourceLoad

		/// <summary>
		/// Gets the data from resources.
		/// 同步从Resources目录中加载资源,用Resources.Load读取
		/// </summary>
		/// <returns>The data from resources.</returns>
		/// <param name="path">Path 相对于Resources的路径,文件名不需要扩展名.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T LoadFromResources<T>(string path) where T : UnityEngine.Object
		{
			T obj = Resources.Load<T> (path);
			return obj;
		}

		/// <summary>
		/// 协程实现从Resources文件异步载入资源.
		/// </summary>
		/// <returns>The from resources async coroutine.</returns>
		/// <param name="path">Path.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static IEnumerator LoadFromResourcesAsyncCoroutine<T>(string path, Action<T> callback) where T : UnityEngine.Object
		{
			ResourceRequest req = Resources.LoadAsync (path);
			yield return req;

			callback ((T)req.asset);
		}

		/// <summary>
		/// 开启协程异步从Resources中加载资源,完成执行回调.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void LoadFromResourcesAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
		{
			PLDResourcesManager.Instance.StartCoroutine(LoadFromResourcesAsyncCoroutine(path, callback));
		}

		#endregion

		//////////////////////////////////////////////////////////////////////////

		#region StreamingAssetsLoadWWW

		/// <summary>
		/// 协程实现异步读取StreamingAssets文件的资源，用WWW
		/// </summary>
		/// <returns>The from streaming assets async coroutine.</returns>
		/// <param name="path">Path.</param>
		/// <param name="callback">Callback.</param>
		static IEnumerator LoadWWWAsyncCoroutine(string path, Action<WWW> callback) 
		{
			WWW www = new WWW (path);
			yield return www;

			callback (www);

			//释放www对象
			www.Dispose();
			www = null;
		}
		
		/// <summary>
		/// 开启协程用WWW方式异步加载StreamingAssets文件夹中的资源.
		/// </summary>
		/// <param name="path">路径,需要包含扩展名.</param>
		/// <param name="callback">Callback,回调函数,返回成功加载的WWW对象作为参数.</param>
		public void LoadFromStreamingAssetsWWWAsync(string path, Action<WWW> callback) 
		{
			string fullpath = "file://" + PLDGlobalDef.STREAMING_PATH + "/" + path;
			PLDResourcesManager.Instance.StartCoroutine (LoadWWWAsyncCoroutine(fullpath, callback));
		}

		/// <summary>
		/// 开启协程用WWW方式异步加载Persistant文件夹中的资源.
		/// </summary>
		/// <param name="path">路径.</param>
		/// <param name="callback">Callback,回调函数,返回成功加载的WWW对象作为参数.</param>
		public void LoadFromPersistantWWWAsync(string path, Action<WWW> callback) 
		{
			string fullpath = "file://" + PLDGlobalDef.STREAMING_PATH + "/" + path;
            PLDResourcesManager.Instance.StartCoroutine(LoadWWWAsyncCoroutine(fullpath, callback));
		}

		#endregion

		//////////////////////////////////////////////////////////////////////////

		#region EditorLoad

		/// <summary>
		/// Gets the data editor.
		/// 编辑器模式下同步获取资源 
		/// </summary>
		/// <returns>The data editor.</returns>
		/// <param name="path">路径,需要扩展名.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T LoadEditor<T>(string path) where T : UnityEngine.Object
		{
		#if UNITY_EDITOR
			T obj = AssetDatabase.LoadAssetAtPath<T>(path);
			return obj;
#endif
		}

		#endregion
	}
}

