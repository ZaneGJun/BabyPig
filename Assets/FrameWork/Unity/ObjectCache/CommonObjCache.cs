using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pld
{
	/// <summary>
	/// Common object cache.
	/// 对象缓存，一定要先注册对应的对象缓存池，才能缓存对应的对象
	/// </summary>
	public class CommonObjCache : PLDMOSingleton<CommonObjCache> {

		//游戏对象字典
		protected Dictionary<string, PLDCommonPool<GameObject>> mObjectPools;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public override void Init ()
		{
			base.Init ();
			mObjectPools = new Dictionary<string, PLDCommonPool<GameObject>> ();
		}
			
		/// <summary>
		/// Get the specified name.
		/// 获取一个缓存中的对象
		/// </summary>
		/// <param name="name">Name obj的名字.</param>
		public GameObject Get(string name) 
		{
			PLDCommonPool<GameObject> pool;
			if (mObjectPools.TryGetValue (name, out pool)) {
				return pool.Allocate ();
			}

			return null;
		}

		/// <summary>
		/// Push the specified obj.
		/// 将对象放进缓存
		/// </summary>
		/// <param name="obj">Object.</param>
		public bool Push(GameObject obj)
		{
			string objName = obj.name;
			PLDCommonPool<GameObject> pool;
			if (mObjectPools.TryGetValue (objName, out pool)) {
				pool.Recycle (obj);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Registers the common object pool.
		/// 注册一个对象池
		/// name必须唯一
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="genFunc">Gen func.</param>
		/// <param name="destroyAction">Destroy action.</param>
		/// <param name="initCount">Init count.</param>
		/// <param name="maxCount">MaxCount.</param>
		public void RegisterCommonObjectPool(string name, Func<GameObject> genFunc, Action<GameObject> destroyAction, int initCount, int maxCount)
		{
			if (!mObjectPools.ContainsKey (name)) {
				PLDCommonPool<GameObject> pool = new PLDCommonPool<GameObject> (genFunc, destroyAction, initCount);
				pool.MaxCount = maxCount;
				mObjectPools.Add (name, pool);
			}
		}

		public void RegisterCommonObjectPool(string name, PLDCommonPool<GameObject> pool)
		{
			if (!mObjectPools.ContainsKey (name)) {
				mObjectPools.Add (name, pool);
			}
		}

		public void Reset()
		{
			mObjectPools.Clear ();
		}

		public void PrintObjectPools()
		{
			foreach (KeyValuePair<string, PLDCommonPool<GameObject>> pair in mObjectPools) {
				Debug.Log (string.Format("{0} count:{1}", pair.Key, pair.Value.CurCount));
			}
		}
	}
}
