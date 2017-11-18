using System;

namespace Pld
{
	public interface IPLDObjectFactory<T> 
	{
		T Create ();

		void Destroy(T obj);
	}
}
