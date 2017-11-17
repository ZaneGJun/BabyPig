using System;

namespace Pld
{
	public class PLDMOPool<T> : PLDPool<T> where T : PLDMOObject
	{
		public PLDMOPool (Func<T> factoryMethod, int initCount)
		{
			mFactory = new PLDMOObjectFactory<T> (factoryMethod);

			//push init count
			for (int i = 0; i < initCount; i++) 
			{
				mStack.Push (mFactory.Create ());
			}
		}
	}
}

