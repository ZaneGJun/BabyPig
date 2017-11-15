using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld{
	
	/// <summary>
	/// Singleton.
	/// 不会随场景切换而销毁的单例实现
	/// 将单例挂载在一个不会随场景切换而销毁的组件上
	/// </summary>
	public abstract class PLDMOSingleton<T> : MonoBehaviour, IPLDSingleton where T : PLDMOSingleton<T> {

		protected static T _instance = null;
		public static T Instance
		{
			get {
				if (null == _instance) {
					//先寻找挂载单例的组件 SingletonHold
					GameObject hold = GameObject.Find ("PLD");
					if (null == hold) {
						hold = new GameObject ("PLD");
						//设置为不可销毁
						DontDestroyOnLoad (hold);
					}

					//在组件上得到T组件
					_instance = hold.GetComponent<T>();

					//如果没有T组件,则添加
					if (null == _instance) {
						_instance = hold.AddComponent<T> ();
					}

					_instance.Init ();
				}

				return _instance;
			}
		}

		public virtual void Init() {
			
		}
	}
}
