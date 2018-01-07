using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pld
{
    class PLDAssetFileLoader : PLDLoaderAbstract
    {
        protected PLDAssetBundleLoader mAssetBundleLoader;
        protected PLDResourceResLoader mResourceResLoader;

        protected LoadOption mLoadOption;

        protected bool mIsEditorLoad
        {
            get
            {
                return Application.isEditor;
            }
        }

        public override object ResultObj
        {
            get
            {
                if(mIsEditorLoad)
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
                if (mIsEditorLoad)
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
                if (mIsEditorLoad)
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
                if (mIsEditorLoad)
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
                if (mIsEditorLoad)
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
                if (mIsEditorLoad)
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

        public static PLDAssetFileLoader Load(string path, LoadOption option, FinishDelgate callBack)
        {
            PLDAssetFileLoader loader = PLDResourceLoaderCache.GetResourceLoader<PLDAssetFileLoader>(path, callBack);
            loader.mLoadOption = option;
            loader.StartLoad(callBack);
            return loader;
        }

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

            if (mIsEditorLoad)
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
