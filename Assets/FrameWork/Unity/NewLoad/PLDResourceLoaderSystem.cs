using System;
using System.Collections.Generic;

namespace Pld
{
    public enum LoadOption
    {
        Sync,   //同步
        Async,  //异步
    }

    class PLDResourceLoaderSystem : PLDMOSingleton<PLDResourceLoaderSystem>
    {
        public void Awake()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            PLDResourceLoaderCache.CheckGcCollect();
        }

        

    }
}
