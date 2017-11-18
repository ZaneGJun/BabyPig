using System;

namespace Pld
{
	using System.Collections.Generic;
	
	public abstract class PLDPool<T> : IPLDPool<T> 
	{
		/// <summary>
		/// Gets the current count.
		/// </summary>
		/// <value>The current count.</value>
		public int CurCount 
		{
			get 
			{
				return mStack.Count;
			}
		}

		/// <summary>
		/// The max count.
		/// 最大缓存数量
		/// </summary>
		public int MaxCount = 10;

		//工厂
		protected IPLDObjectFactory<T> mFactory;
	
		protected Stack<T> mStack = new Stack<T>();

		/// <summary>
		/// Allocate this instance.
		/// 获取,如果没有,调用对象工厂创建新的对象,缓存中存在空闲对象,出栈
		/// </summary>
		public virtual T Allocate() 
		{
			return mStack.Count == 0 ? mFactory.Create () : mStack.Pop ();
		}

		/// <summary>
		/// Recytle the specified .
		/// 回收
		/// 如果栈满,释放对象,否则进栈
		/// </summary>
		/// <param name="">.</param>
		/// <param name="obj">Object.</param>
		public virtual void Recycle(T obj) 
		{
			if (mStack.Count == MaxCount)
			{
				mFactory.Destroy (obj);
			} 
			else 
			{
				mStack.Push (obj);
			}
		}
	}
}

