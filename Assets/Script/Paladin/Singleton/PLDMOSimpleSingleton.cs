using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld{
	/// <summary>
	/// PLDMO simple singleton.
	/// 简单的单例实现
	/// </summary>
	public abstract class PLDMOSimpleSingleton<T> : MonoBehaviour where T : PLDMOSimpleSingleton<T> {

		protected static T _instance = null;
		public static T Instance {
			get{
				return _instance;
			}
		}

		void Awake() {
			_instance = this;
		}
	}
}
