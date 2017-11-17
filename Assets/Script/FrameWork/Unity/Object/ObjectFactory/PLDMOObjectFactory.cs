using System;
using UnityEngine;

namespace Pld
{
	public class PLDMOObjectFactory<T> : IPLDObjectFactory<T> where T : PLDMOObject
	{

		protected Func<T> mCreateMothod;

		public PLDMOObjectFactory(Func<T> createMethod)
		{
			mCreateMothod = createMethod;
		}

		public virtual T Create ()
		{
			return mCreateMothod ();
		}

		public virtual void Destroy(T obj)
		{
			
		}
	}
}

