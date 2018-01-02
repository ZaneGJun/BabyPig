using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    /// <summary>
    /// 从Resources中加载资源
    /// </summary>
    class PLDResourceResLoader : PLDResourceLoaderAbstract
    {
        protected LoadOption mLoadOption;

        /// <summary>
        /// 静态函数，加载资源
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="option">可选项</param>
        /// <param name="callback">完成回调</param>
        /// <returns></returns>
        public static PLDResourceResLoader Load(string url, LoadOption option = LoadOption.Async, FinishDelgate callback = null)
        {
            PLDResourceResLoader loader;
            loader = PLDResourceLoaderCache.GetResourceLoader<PLDResourceResLoader>(url, callback);

            loader.mLoadOption = option;
            loader.StartLoad();

            return loader;
        }

        /// <summary>
        /// 继承的初始化函数
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="finishcallback">完成回调</param>
        public override void Init(string url, FinishDelgate finishcallback = null)
        {
            base.Init(url, finishcallback);
        }

        /// <summary>
        /// 开始加载，根据可选项进行不同处理
        /// </summary>
        protected void StartLoad()
        {
            OnStart();

            if(mLoadOption == LoadOption.Sync)
            {
                //同步加载
                UnityEngine.Object obj = Resources.Load(Url);
                OnFinish(obj);
            }
            else if(mLoadOption == LoadOption.Async)
            {
                //异步加载
                PLDResourceLoaderSystem.Instance.StartCoroutine(DoLoadAsync());
            }
            else
            {
                OnFinish(null, string.Format("[PLDResourceResLoader] LoadOption error:{0}",mLoadOption));
            }
        }

        /// <summary>
        /// 异步加载协程
        /// </summary>
        /// <returns></returns>
        IEnumerator DoLoadAsync()
        {
            ResourceRequest req = Resources.LoadAsync(Url);

            while(!req.isDone)
            {
                OnProcess(req.progress);
                yield return null;
            }

            yield return req;

            if (IsReadyDispose)
            {
                Debug.LogError(string.Format("[PLDResourceResLoader]Too early release: { 0}", Url));
                OnFinish(null);
                yield break;
            }

            //通知回调结果
            OnFinish(req.asset);
        }
    }
}
