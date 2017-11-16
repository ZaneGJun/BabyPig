using System;
using UnityEngine;

namespace Pld
{
	public class PLDMOObjectFactory<T> : IPLDObjectFactory<T> where T : MonoBehaviour, IPLDObject
	{
		public virtual T Create ()
		{
			
		}

		public virtual void Destroy(T obj)
		{
			
		}
	}
}

