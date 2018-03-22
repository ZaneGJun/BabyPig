using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pld
{ 
    class PLDEditorLoader : PLDLoaderAbstract
    {
        /// <summary>
        /// 静态创建方法
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static PLDEditorLoader Create(string url)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDEditorLoader>(url);
            return loader as PLDEditorLoader;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <returns></returns>
        public object Load()
        {
            object res = startLoad();
            return res;
        }

        // 初始化
        public override void Init(string url)
        {
            base.Init(url);
        }

        // 内部加载方法
        protected object startLoad()
        {
            OnStart();

#if UNITY_EDITOR
            Object res = AssetDatabase.LoadAssetAtPath<Object>(Url);
            OnFinish(res);
            return res;
#endif

            return null;
        }
    }
}
