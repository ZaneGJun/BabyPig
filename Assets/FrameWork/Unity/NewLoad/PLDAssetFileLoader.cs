using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    class PLDAssetFileLoader : PLDLoaderAbstract
    {
        protected PLDAssetBundleLoader mAssetBundleLoader;
        protected PLDResourceResLoader mResourceResLoader;

        protected LoadOption mLoadOption;

        protected bool mIsEditorLoad
        {
            get
            {
                return Application.isEditor;
            }
        }

        public static PLDAssetFileLoader Load(string path, LoadOption option, FinishDelgate callBack)
        {
            PLDAssetFileLoader loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetFileLoader>(path, callBack);
            loader.StartLoad(callBack);
            loader.mLoadOption = option;
            return loader;
        }

        public override void Init(string path, FinishDelgate finishcallback = null)
        {
            base.Init(path, finishcallback);

           
        }

        protected override void DoDispose()
        {
            base.DoDispose();

            PLDResourceLoaderCache.ReleaseLoader(mAssetBundleLoader);
            PLDResourceLoaderCache.ReleaseLoader(mResourceResLoader);
        }

        protected void StartLoad(FinishDelgate callBack)
        {
            if (mIsEditorLoad)
            {
                mResourceResLoader = PLDResourceResLoader.Load(Url, mLoadOption, callBack);
            }
            else
            {
                mAssetBundleLoader = PLDAssetBundleLoader.Load(Url, mLoadOption, callBack);
            }
        }
    }
}
