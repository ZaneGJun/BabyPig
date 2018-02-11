using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pld
{
    class PLDEditorLoader : PLDLoaderAbstract
    {
        public static PLDEditorLoader Load(string url, FinishDelgate callback)
        {
            var loader = PLDResourceLoaderCache.GetResourceLoader<PLDEditorLoader>(url, callback);
            loader.startLoad();
            return loader as PLDEditorLoader;
        }

        public override void Init(string url, FinishDelgate finishcallback = null)
        {
            base.Init(url, finishcallback);

        }

        protected void startLoad()
        {
            OnStart();

#if UNITY_EDITOR
            GameObject res = AssetDatabase.LoadAssetAtPath<GameObject>(Url);
            OnFinish(res);
#endif
        }
    }
}
