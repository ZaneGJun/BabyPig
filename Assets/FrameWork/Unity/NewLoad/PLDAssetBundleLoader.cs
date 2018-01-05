using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    class PLDAssetBundleLoader : PLDLoaderAbstract
    {
        protected LoadOption mLoadOption;

        /// <summary>
        /// 静态函数，加载AssetBundle资源
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="option">选项</param>
        /// <param name="callback">完成回调</param>
        /// <returns></returns>
        public static PLDAssetBundleLoader Load(string url, LoadOption option = LoadOption.Async, FinishDelgate callback = null)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetBundleLoader>(url, callback);

            loader.mLoadOption = option;
            loader.StartLoad();

            return loader as PLDAssetBundleLoader;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="finishcallback">完成回调</param>
        public override void Init(string url, FinishDelgate finishcallback = null)
        {
            base.Init(url, finishcallback);
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        protected virtual void StartLoad()
        {
            OnStart();

            if(mLoadOption == LoadOption.Sync)
            {
                //同步加载
                AssetBundle result = AssetBundle.LoadFromFile(Url);
                OnFinish(result);
            }
            else if(mLoadOption == LoadOption.Async)
            {
                //异步加载
                PLDResourceLoaderSystem.Instance.StartCoroutine(DoLoadAsync());
            }
            else
            {
                OnFinish(null, string.Format("[PLDNewAssetBundleLoader] LoadOption error:{0}", mLoadOption));
            }
        }

        /// <summary>
        /// 异步加载协程
        /// </summary>
        /// <returns></returns>
        protected IEnumerator DoLoadAsync()
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
