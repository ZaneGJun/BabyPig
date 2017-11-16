using System.Collections;
using System.Collections.Generic;

namespace Pld
{
	public class PLDSingleton<T> : IPLDSingleton where T : PLDSingleton<T>, new()
	{
		protected static T _instance = null;
		public static T Instance{
			get{
				if (null == _instance) {
					_instance = new T ();
					_instance.Init ();
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
