using System;

namespace Pld
{
	public class PLDObjectFactory<T> : IPLDObjectFactory<T> where T : IPLDObject, new()
	{
		public virtual T Create ()
		{
			T obj = new T ();
			obj.onCreate ();
			return obj;
		}

		public virtual void Destroy(T obj)
		{
			obj.onDestroy ();
		}
	}
}

