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
        /// <summary>
        /// 静态创建方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static PLDAssetBundleLoader Create(string url)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetBundleLoader>(url);
            return loader;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <returns></returns>
        public AssetBundle Load()
        {
            OnStart();

            //同步加载
            AssetBundle result = AssetBundle.LoadFromFile(Url);
            OnFinish(result);

            return result;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="callback">完成回调</param>
        public void LoadAsync(FinishDelgate callback = null)
        {
            if (callback != null)
                FinishCallback = callback;

            OnStart();

            //异步加载
            PLDResourceLoaderSystem.Instance.StartCoroutine(DoLoadAsync());
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url">路径</param>
        public override void Init(string url)
        {
            base.Init(url);
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
