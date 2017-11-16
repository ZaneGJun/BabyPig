using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
	public interface IPLDPool<T> where T : IPLDObject
	{
		/// <summary>
		/// Allocate this instance.
		/// 获取
		/// </summary>
		T Allocate();

		/// <summary>
		/// Recytle the specified .
		/// 回收
		/// </summary>
		/// <param name="">.</param>
		void Recycle(T obj);

	}
}
