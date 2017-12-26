using System;
using System.Collections.Generic;

namespace Pld
{
    abstract class PLDResourceLoaderAbstract : PLDRefObject, IPLDDisposeObject
    {

        /// <summary>
        /// 最终加载的结果
        /// </summary>
        public object ResultObj { get; private set; }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool IsError { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 是否准备Dispose
        /// </summary>
        public bool IsReadyDispose { get; private set; }

        public PLDResourceLoaderAbstract()
        {
            
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            ResultObj = null;
            IsReadyDispose = false;

            IsError = false;
            IsSuccess = false;
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
        /// 释放对象
        /// </summary>
        public virtual void Dispose()
        {

        }

        /// <summary>
        /// 一次加载完成
        /// </summary>
        /// <param name="resultObj"></param>
        public virtual void OnFinish(object resultObj)
        {

        }
    }
}
