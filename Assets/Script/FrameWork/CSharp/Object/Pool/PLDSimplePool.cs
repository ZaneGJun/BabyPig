using System;

namespace Pld
{
	public class PLDSimplePool<T> : PLDPool<T> where T : IPLDObject, new()
	{
		public PLDSimplePool (int initCount = 0)
		{
			//实例化工厂,可以实现不同的工厂来提供不同对象的实例化
			mFactory = new PLDObjectFactory<T> ();

			//push init count
			for (int i = 0; i < initCount; i++) 
			{
				mStack.Push (mFactory.Create ());
			}
		}
	}
}

