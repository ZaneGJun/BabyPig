using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    /// <summary>
    /// 从Resources中加载资源
    /// </summary>
    class PLDResourceResLoader : PLDLoaderAbstract
    {

        // 静态方法
        public static PLDResourceResLoader Create(string path)
        {
            path = Utils.RemoveExt(path);
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDResourceResLoader>(path);
            return loader;
        }

        //同步加载
        public object Load()
        {
            OnStart();

            //同步加载
            UnityEngine.Object obj = Resources.Load(Url);
            OnFinish(obj);

            return obj;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="callback">完成回调</param>
        public void LoadAsync(FinishDelgate callback)
        {
            if (callback != null)
                this.FinishCallback = callback;

            OnStart();

            //异步加载
            PLDResourceLoaderSystem.Instance.StartCoroutine(DoLoadAsync());
        }

        /// <summary>
        /// 继承的初始化函数
        /// </summary>
        /// <param name="path">路径</param>
        public override void Init(string path)
        {
            base.Init(path);
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
