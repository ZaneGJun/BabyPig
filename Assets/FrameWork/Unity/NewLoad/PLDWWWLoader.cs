using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    class PLDWWWLoader : PLDResourceLoaderAbstract
    {

        /// <summary>
        /// WWW对象
        /// </summary>
        private WWW mWWW = null;

        /// <summary>
        /// 
        /// </summary>
        private bool IsAsync = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static PLDWWWLoader Load(string url)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDWWWLoader>(url);
            loader.StartLoad();
            return loader;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        public override void Init(string url)
        {
            base.Init(url);

            OnProcess(10.0F);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void StartLoad()
        {
            //调用通知开始
            OnStart();
            OnProcess(20.0F);

            PLDResourceLoaderSystem.Instance.StartCoroutine(DoLoad());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IEnumerator DoLoad()
        {
            OnProcess(30.0F);

            mWWW = new WWW(Url);
            yield return mWWW;

            //完成
            OnFinish(mWWW);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DoDispose()
        {
            base.DoDispose();

            mWWW.Dispose();
            mWWW = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultObj"></param>
        public override void OnFinish(object resultObj)
        {
            base.OnFinish(resultObj);

        }

    }
}
