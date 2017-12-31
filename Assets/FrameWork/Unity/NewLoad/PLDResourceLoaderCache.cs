using System;
using System.Collections.Generic;
using Pld;
using UnityEngine;

namespace Pld
{
    class PLDResourceLoaderCache
    {
        /// <summary>
        /// 对象池字典
        /// </summary>
        protected static Dictionary<Type, Dictionary<string,PLDResourceLoaderAbstract>> mLoaderDictionary = 
            new Dictionary<Type, Dictionary<string, PLDResourceLoaderAbstract>>();

        #region 垃圾回收 Garbage Collect

        /// <summary>
        /// Loader延迟Dispose
        /// </summary>
        private const float LoaderDisposeTime = 0;

        /// <summary>
        /// 间隔多少秒做一次GC(在AutoNew时)
        /// </summary>
        public static float GcIntervalTime
        {
            get
            {
                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.OSXEditor)
                    return 1f;

                return Debug.isDebugBuild ? 5f : 10f;
            }
        }

        /// <summary>
        /// 上次做GC的时间
        /// </summary>
        private static float _lastGcTime = -1;

        /// <summary>
        /// 缓存起来要删掉的，供DoGarbageCollect函数用, 避免重复的new List
        /// </summary>
        private static readonly List<PLDResourceLoaderAbstract> mCacheLoaderToRemoveFromUnUsed =
            new List<PLDResourceLoaderAbstract>();

        /// <summary>
        /// 准备要进行Dispose的Loader
        /// </summary>
        protected static readonly Dictionary<PLDResourceLoaderAbstract, float> mUnUsesLoaders = 
            new Dictionary<PLDResourceLoaderAbstract, float>();

        #endregion

        public static Dictionary<string, PLDResourceLoaderAbstract> GetTypeDict(Type type)
        {
            Dictionary<string, PLDResourceLoaderAbstract> typesDict;
            if (!mLoaderDictionary.TryGetValue(type, out typesDict))
            {
                typesDict = mLoaderDictionary[type] = new Dictionary<string, PLDResourceLoaderAbstract>();
            }
            return typesDict;
        }

        public static int GetCount<T>()
        {
            return GetTypeDict(typeof(T)).Count;
        }

        public static int GetRefCount<T>(string url)
        {
            var dict = GetTypeDict(typeof(T));
            PLDResourceLoaderAbstract loader;
            if (dict.TryGetValue(url, out loader))
            {
                return loader.RefCount;
            }
            return 0;
        }

        /// <summary>
        /// 获取Loader对象
        /// </summary>
        /// <typeparam name="T"> T必须继承PLDResourceLoaderAbstract且具有构造函数的类</typeparam>
        /// <param name="url">url</param>
        /// <param name="finishcallback">完成回调</param>
        /// <returns>返回一个T的可用实例</returns>
        public static T GetResourceLoader<T>(string url, PLDResourceLoaderAbstract.FinishDelgate finishcallback = null) where T : PLDResourceLoaderAbstract, new()
        {
            Dictionary<string, PLDResourceLoaderAbstract> dict = GetTypeDict(typeof(T));

            PLDResourceLoaderAbstract loader;
            if(!dict.TryGetValue(url, out loader))
            {
                loader = new T();
                loader.Init(url, finishcallback);
            }

            // 增加一次引用
            loader.Retain();

            // RefCount++了，重新激活，在队列中准备清理的Loader
            if (mUnUsesLoaders.ContainsKey(loader))
            {
                mUnUsesLoaders.Remove(loader);
                loader.Revice();
            }

            return loader as T;
        }

        /// <summary>
        /// 释放loader
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="gcNow">是否立即执行GC</param>
        public static void ReleaseLoader(PLDResourceLoaderAbstract loader, bool gcNow = false)
        {
            //减少一次引用
            loader.Release();

            if(loader.RefCount <= 0)
            {
                if(mUnUsesLoaders.ContainsKey(loader))
                {
                    Debug.LogError(string.Format("[UnUsesLoader exists: {0}", loader.GetType()));
                }

                //加入到准备Dispose中
                mUnUsesLoaders[loader] = Time.time;
                loader.OnReadyDispose();
            }

            // 立即执行GC
            if (gcNow)
                DoGarbageCollect();
        }

        /// <summary>
        /// 是否进行垃圾收集
        /// </summary>
        public static void CheckGcCollect()
        {
            if (_lastGcTime.Equals(-1) || (Time.time - _lastGcTime) >= GcIntervalTime)
            {
                DoGarbageCollect();
            }
        }

        /// <summary>
        /// 进行垃圾回收
        /// </summary>
        private static void DoGarbageCollect()
        {
            foreach (var kv in mUnUsesLoaders)
            {
                var loader = kv.Key;
                var time = kv.Value;
                if ((Time.time - time) >= LoaderDisposeTime)
                {
                    mCacheLoaderToRemoveFromUnUsed.Add(loader);
                }
            }

            for (var i = mCacheLoaderToRemoveFromUnUsed.Count - 1; i >= 0; i--)
            {
                try
                {
                    var loader = mCacheLoaderToRemoveFromUnUsed[i];
                    mUnUsesLoaders.Remove(loader);
                    mCacheLoaderToRemoveFromUnUsed.RemoveAt(i);
                    loader.Dispose();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message.ToString());
                }
            }

            if (mCacheLoaderToRemoveFromUnUsed.Count > 0)
            {
                Debug.LogError("[DoGarbageCollect]CacheLoaderToRemoveFromUnUsed muse be empty!!");
            }

            // 记下时间
            _lastGcTime = Time.time;
        }
    }
}
