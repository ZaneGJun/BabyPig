using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    /// <summary>
    /// 加载WWW对象
    /// </summary>
    class PLDWWWLoader : PLDLoaderAbstract
    {

        /// <summary>
        /// WWW对象
        /// </summary>
        private WWW mWWW = null;

        /// <summary>
        /// 静态创建方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static PLDWWWLoader Create(string url)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDWWWLoader>(url);
            return loader;
        }

        /// <summary>
        /// 开始加载，调用内部的加载方法
        /// </summary>
        /// <param name="finishcallback">完成回调</param>
        /// <param name="startcallback">开始回调</param>
        /// <param name="processcallback">进度回调</param>
        /// <param name="successcallback">成功回调</param>
        /// <param name="errorcallback">失败回调</param>
        public void LoadAsync(FinishDelgate finishcallback = null, StartDelgate startcallback = null, ProcessDelgate processcallback = null,
                                            SuccessDelgate successcallback = null, ErrorDelgate errorcallback = null)
        {
            //设置回调
            if (startcallback != null)
                StartCallback = startcallback;

            if (processcallback != null)
                ProcessCallback = processcallback;

            if (successcallback != null)
                SuccessCallback = successcallback;

            if (errorcallback != null)
                ErrorCallbcak = errorcallback;

            if (finishcallback != null)
                FinishCallback = finishcallback;

            StartLoad();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        public override void Init(string url)
        {
            base.Init(url);
        }

        /// <summary>
        /// 开始加载,开启协程进行下载
        /// </summary>
        protected void StartLoad()
        {
            //调用通知开始
            OnStart();

            PLDResourceLoaderSystem.Instance.StartCoroutine(DoLoad());
        }

        /// <summary>
        /// 协程具体实现
        /// </summary>
        /// <returns></returns>
        protected IEnumerator DoLoad()
        {
            mWWW = new WWW(Url);

            //设置AssetBundle解压缩线程的优先级
            mWWW.threadPriority = Application.backgroundLoadingPriority; // 取用全局的加载优先速度
            while (!mWWW.isDone)
            {
                OnProcess(mWWW.progress);
                yield return null;
            }

            yield return mWWW;

            if (IsReadyDispose)
            {
                Debug.LogError(string.Format("[PLDWWWLoader]Too early release: { 0}", Url));
                OnFinish(null);
                yield break;
            }

            //检测是否有错误
            if (!string.IsNullOrEmpty(mWWW.error))
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    // TODO: Android下的错误可能是因为文件不存在!
                }

                OnFinish(null, string.Format("[PLDWWWLoader load error:{0} url:{1}]", mWWW.error, Url));
            }
            else
            {
                //正常完成
                OnFinish(mWWW);
            }
        }

        /// <summary>
        /// 进行释放
        /// </summary>
        protected override void DoDispose()
        {
            base.DoDispose();

            mWWW.Dispose();
            mWWW = null;
        }

        /// <summary>
        /// 完成回调
        /// </summary>
        /// <param name="resultObj">最后得到的结果</param>
        /// <param name="msg">信息</param>
        protected override void OnFinish(object resultObj, string msg = null)
        {
            base.OnFinish(resultObj, msg);
        }

    }
}
