using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    abstract class PLDLoaderAbstract : PLDRefObject, IPLDDisposeObject
    {
        #region 定义委托

        /// <summary>
        /// 开始委托
        /// </summary>
        public delegate void StartDelgate();

        /// <summary>
        /// 载入过程委托
        /// </summary>
        /// <param name="process"></param>
        public delegate void ProcessDelgate(float process);

        /// <summary>
        /// 成功委托
        /// </summary>
        /// <param name="resultObject">载入得到的对象</param>
        public delegate void SuccessDelgate(object resultObject);

        /// <summary>
        /// 失败委托
        /// </summary>
        /// <param name="msg">失败的信息</param>
        public delegate void ErrorDelgate(string msg);

        /// <summary>
        /// 完成委托
        /// </summary>
        /// <param name="isOk">是否成功</param>
        /// <param name="resultObject">载入得到的对象</param>
        public delegate void FinishDelgate(bool isOk, object resultObject);

        #endregion

        #region 成员对象

        /// <summary>
        /// 最终加载的结果
        /// </summary>
        public virtual object ResultObj { get; private set; }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public virtual bool IsError { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public virtual bool IsSuccess { get; private set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public virtual bool IsFinish { get; private set; }

        /// <summary>
        /// 是否准备Dispose
        /// </summary>
        public virtual bool IsReadyDispose { get; private set; }

        /// <summary>
        /// Url
        /// </summary>
        public virtual string Url { get; private set; }

        /// <summary>
        /// 信息
        /// </summary>
        public virtual string Message { get; private set; }

        /// <summary>
        /// 进度
        /// </summary>
        public virtual float Process { get; private set; }

        /// <summary>
        /// 开始回调
        /// </summary>
        public virtual StartDelgate StartCallback { private get; set; }

        /// <summary>
        /// 进行回调
        /// </summary>
        public virtual ProcessDelgate ProcessCallback { private get; set; }

        /// <summary>
        /// 成功回调
        /// </summary>
        public virtual SuccessDelgate SuccessCallback { private get; set; }

        /// <summary>
        /// 失败回调
        /// </summary>
        public virtual ErrorDelgate ErrorCallbcak { private get; set; }

        /// <summary>
        /// 完成回调
        /// </summary>
        public virtual FinishDelgate FinishCallback { private get; set; }

        protected float mInitTime = -1;
        protected float mFinishTime = -1;

        /// <summary>
        /// 用时
        /// </summary>
        public float UseTime {
            get
            {
                return (mFinishTime - mInitTime);
            }
        }

        #endregion

        public PLDLoaderAbstract()
        {
            
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init(string url, FinishDelgate finishcallback = null)
        {
            mInitTime = Time.realtimeSinceStartup;
            ResultObj = null;
            IsReadyDispose = false;

            IsError = false;
            IsSuccess = false;
            IsFinish = false;

            Url = url;
            Message = "";

            Process = 0.0F;

            StartCallback = null;
            ProcessCallback = null;
            SuccessCallback = null;
            ErrorCallbcak = null;

            FinishCallback = finishcallback;
        }

        /// <summary>
        /// 从准备Dispose中恢复
        /// </summary>
        public virtual void Revice()
        {
            IsReadyDispose = false;
        }

        /// <summary>
        /// 准备Dispose回调用
        /// </summary>
        public virtual void OnReadyDispose()
        {
            IsReadyDispose = true;
        }

        /// <summary>
        /// 释放对象，子类一般重写DoDispose
        /// </summary>
        public void Dispose()
        {
            DoDispose();
        }

        /// <summary>
        /// 释放对象时进行的操作
        /// </summary>
        protected virtual void DoDispose()
        {
            
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="msg">信息</param>
        public virtual void OnStart(string msg = null)
        {
            Message = string.IsNullOrEmpty(msg) ? "Loader OnStart" : msg;
            Process = 0.0F;

            if (StartCallback != null)
                StartCallback();
        }

        /// <summary>
        /// 进度变化
        /// </summary>
        /// <param name="process">进度，[0.0F,1.0F]区间</param>
        /// <param name="msg">信息</param>
        public virtual void OnProcess(float process, string msg = null)
        {
            Message = string.IsNullOrEmpty(msg) ? string.Format("Loader OnProcess:{0}",process) : msg;
            Process = process;

            if (ProcessCallback != null)
                ProcessCallback(Process);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="msg">信息</param>
        public virtual void OnSuccess(string msg = null)
        {
            Message = string.IsNullOrEmpty(msg) ? "Loader OnSucccess" : msg;

            if (SuccessCallback != null)
                SuccessCallback(ResultObj);
        }

        /// <summary>
        /// 出错
        /// </summary>
        /// <param name="msg">信息</param>
        public virtual void OnError(string msg = null)
        {
            Message = string.IsNullOrEmpty(msg) ? "Loader OnError" : msg;

            if (ErrorCallbcak != null)
                ErrorCallbcak(Message);
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="resultObj">加载完成的结果</param>
        /// <param name="msg">信息</param>
        public virtual void OnFinish(object resultObj, string msg = null)
        {
            ResultObj = resultObj;
            Process = 1.0F;

            IsError = resultObj == null;
            IsSuccess = !IsError;

            mFinishTime = Time.realtimeSinceStartup;

            if (IsSuccess)
                OnSuccess();

            if (IsError)
                OnError();

            if(msg != null)
            {
                Message = msg;
            }

            IsFinish = true;
            if(FinishCallback != null)
            {
                FinishCallback(IsSuccess, ResultObj);
            }
        }
    }

    
}
