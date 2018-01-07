using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    class PLDAssetFileLoader : PLDLoaderAbstract
    {
        // AssetBundle方式Loader
        protected PLDAssetBundleLoader mAssetBundleLoader;
        // Resource方式Loader
        protected PLDResourceResLoader mResourceResLoader;

        protected LoadOption mLoadOption;

        // 根据配置绝对用哪种方式读取
        protected bool mIsResourceLoad
        {
            get
            {
                return true;
            }
        }

        #region 继承的成员变量,根据读取方式返回对应Loader的值
        public override object ResultObj
        {
            get
            {
                if(mIsResourceLoad)
                {
                    if (mResourceResLoader != null)
                        return mResourceResLoader.ResultObj;
                }
                else
                {
                    if (mAssetBundleLoader != null)
                        return mAssetBundleLoader.ResultObj;
                }

                return null;
            }
        }

        public override bool IsError
        {
            get
            {
                if (mIsResourceLoad)
                {
                    if (mResourceResLoader != null)
                        return mResourceResLoader.IsError;
                }
                else
                {
                    if (mAssetBundleLoader != null)
                        return mAssetBundleLoader.IsError;
                }

                return false;
            }
        }

        public override bool IsSuccess
        {
            get
            {
                if (mIsResourceLoad)
                {
                    if (mResourceResLoader != null)
                        return mResourceResLoader.IsSuccess;
                }
                else
                {
                    if (mAssetBundleLoader != null)
                        return mAssetBundleLoader.IsSuccess;
                }

                return false;
            }
        }

        public override bool IsFinish
        {
            get
            {
                if (mIsResourceLoad)
                {
                    if (mResourceResLoader != null)
                        return mResourceResLoader.IsFinish;
                }
                else
                {
                    if (mAssetBundleLoader != null)
                        return mAssetBundleLoader.IsFinish;
                }

                return false;
            }
        }

        public override string Message
        {
            get
            {
                if (mIsResourceLoad)
                {
                    if (mResourceResLoader != null)
                        return mResourceResLoader.Message;
                }
                else
                {
                    if (mAssetBundleLoader != null)
                        return mAssetBundleLoader.Message;
                }

                return base.Message;
            }
        }

        public override float Process
        {
            get
            {
                if (mIsResourceLoad)
                {
                    if (mResourceResLoader != null)
                        return mResourceResLoader.Process;
                }
                else
                {
                    if (mAssetBundleLoader != null)
                        return mAssetBundleLoader.Process;
                }

                return base.Process;
            }
        }

        public override ProcessDelgate ProcessCallback
        {
            set
            {
                base.ProcessCallback = value;
                if (mResourceResLoader != null)
                    mResourceResLoader.ProcessCallback = value;
                if (mAssetBundleLoader != null)
                    mAssetBundleLoader.ProcessCallback = value;

            }
        }

        public override SuccessDelgate SuccessCallback
        {
            set
            {
                base.SuccessCallback = value;
                if (mResourceResLoader != null)
                    mResourceResLoader.SuccessCallback = value;
                if (mAssetBundleLoader != null)
                    mAssetBundleLoader.SuccessCallback = value;
            }
        }

        public override ErrorDelgate ErrorCallbcak
        {
            set
            {
                base.ErrorCallbcak = value;
                if (mResourceResLoader != null)
                    mResourceResLoader.ErrorCallbcak = value;
                if (mAssetBundleLoader != null)
                    mAssetBundleLoader.ErrorCallbcak = value;
            }
        }
        #endregion

        /// <summary>
        /// 静态方法，读取资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="option">可选项</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public static PLDAssetFileLoader Load(string path, LoadOption option, FinishDelgate callBack)
        {
            PLDAssetFileLoader loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetFileLoader>(path, callBack);
            loader.mLoadOption = option;
            loader.StartLoad(callBack);
            return loader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="option"></param>
        /// <param name="callBack"></param>
        /// <param name="startCallBack"></param>
        /// <returns></returns>
        public static PLDAssetFileLoader Load(string path, LoadOption option, FinishDelgate callBack, StartDelgate startCallBack)
        {
            PLDAssetFileLoader loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetFileLoader>(path, callBack);
            loader.mLoadOption = option;
            loader.StartCallback = startCallBack;
            loader.StartLoad(callBack);
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
            OnStart();

            if (mIsResourceLoad)
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
