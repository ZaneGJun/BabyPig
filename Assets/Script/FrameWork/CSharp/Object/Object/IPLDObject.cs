using System;

namespace Pld
{
	public interface IPLDObject
	{
		void onCreate();

		void onDestroy();
	}
}

