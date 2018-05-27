using System;

namespace Pld
{
	public class PLDSimpleObjectFactory<T> : IPLDObjectFactory<T> where T : new()
	{
		public virtual T Create ()
		{
			T obj = new T ();
			return obj;
		}

		public virtual void Destroy(T obj)
		{

		}
	}
}

