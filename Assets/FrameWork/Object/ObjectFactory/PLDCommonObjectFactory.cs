using System;
using UnityEngine;

namespace Pld
{
	public class PLDCommonObjectFactory<T> : IPLDObjectFactory<T> 
	{

		protected Func<T> mCreateMothod;
		protected Action<T> mDestroyAction;

		public PLDCommonObjectFactory(Func<T> createMethod, Action<T> destroyAction)
		{
			mCreateMothod = createMethod;
			mDestroyAction = destroyAction;
		}

		public virtual T Create ()
		{
			return mCreateMothod ();
		}

		public virtual void Destroy(T obj)
		{
			mDestroyAction (obj);
		}
	}
}

