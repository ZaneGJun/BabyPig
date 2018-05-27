using System;

namespace Pld
{
	public class PLDCommonPool<T> : PLDPool<T> 
	{
		public PLDCommonPool (Func<T> factoryMethod, Action<T> destroyAction, int initCount)
		{
			mFactory = new PLDCommonObjectFactory<T> (factoryMethod, destroyAction);

			//push init count
			for (int i = 0; i < initCount && i < MaxCount; i++) 
			{
				mStack.Push (mFactory.Create ());
			}
		}
	}
}

