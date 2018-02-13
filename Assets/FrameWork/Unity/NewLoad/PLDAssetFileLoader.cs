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
        /// 静态创建方法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PLDAssetFileLoader Create(string path)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetFileLoader>(path);

            loader.mAssetBundleLoader = PLDAssetBundleLoader.Create(path);
            loader.mResourceResLoader = PLDResourceResLoader.Create(path);

            return loader;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <returns></returns>
        public object Load()
        { 
            OnStart();
            
            if(mIsResourceLoad)
            {
                return mResourceResLoader.Load();
            }
            else
            {
                return mAssetBundleLoader.Load();
            }
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="callback"></param>
        public void LoadAsync(FinishDelgate callback)
        {
            if (mIsResourceLoad)
            {
                mResourceResLoader.LoadAsync(callback);
            }
            else
            {
                mAssetBundleLoader.LoadAsync(callback);
            }
        }

        // 初始化 
        public override void Init(string path)
        {
            base.Init(path);
        }

        // 重写父类的方法
        protected override void DoDispose()
        {
            base.DoDispose();

            PLDResourceLoaderCache.ReleaseLoader(mAssetBundleLoader);
            PLDResourceLoaderCache.ReleaseLoader(mResourceResLoader);
        }      
    }
}
