using System;
using System.Collections.Generic;

namespace Pld
{
    class PLDResourceLoaderSystem : PLDMOSingleton<PLDResourceLoaderSystem>
    {


        public void Update()
        {
            PLDResourceLoaderCache.CheckGcCollect();
        }
    }
}
