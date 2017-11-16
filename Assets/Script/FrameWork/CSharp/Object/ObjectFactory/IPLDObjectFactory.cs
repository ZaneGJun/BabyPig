using System;

namespace Pld
{
	public interface IPLDObjectFactory<T> where T: IPLDObject
	{
		T Create ();

		void Destroy(T obj);
	}
}
