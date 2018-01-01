using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{

    class PLDNewAssetBundleLoader : PLDResourceLoaderAbstract
    {
        private LoadOption mLoadOption;

        public static PLDNewAssetBundleLoader Load(string url, LoadOption option, FinishDelgate callback)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDNewAssetBundleLoader>(url, callback);

            loader.mLoadOption = option;
            loader.StartLoad();

            return loader as PLDNewAssetBundleLoader;
        }

        public override void Init(string url, FinishDelgate finishcallback = null)
        {
            base.Init(url, finishcallback);
        }

        protected virtual void StartLoad()
        {
            OnStart();

            if(mLoadOption == LoadOption.Sync)
            {
                //同步加载
                AssetBundle result = AssetBundle.LoadFromFile(Url);

                OnFinish(result);
            }else
            {
                //异步加载
                PLDResourceLoaderSystem.Instance.StartCoroutine(LoadAssetBundle());
            }
        }

        protected IEnumerator LoadAssetBundle()
        {
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(Url);

            while(!req.isDone)
            {
                OnProcess(req.progress);
                yield return null;
            }

            yield return req;

            if (IsReadyDispose)
            {
                Debug.LogError(string.Format("[PLDNewAssetBundleLoader]Too early release: { 0}", Url));
                OnFinish(null);
                yield break;
            }

            OnFinish(req.assetBundle);
        }
    }
}
