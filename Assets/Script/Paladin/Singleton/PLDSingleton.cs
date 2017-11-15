using System.Collections;
using System.Collections.Generic;

namespace Pld{
	public abstract class PLDSingleton<T> where T : class, new(){

		protected static T _instance = null;
		public static T Instance{
			get{
				if (null == _instance) {
					_instance = new T ();
				}

				return _instance;
			}
		}

		protected PLDSingleton() {
			if (null != _instance) {
			}
			Init ();
		}

		public virtual void Init() {
		}

	}
}
