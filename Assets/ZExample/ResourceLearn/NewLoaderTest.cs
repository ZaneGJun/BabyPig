using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public class NewLoaderTest : MonoBehaviour {

    private PLDWWWLoader mWWWLoader = null;
    private PLDAssetBundleLoader mAssetBundleLoader = null;
    private PLDResourceResLoader mResourceResLoader = null;
    private PLDAssetFileLoader mAssetFileLoader = null;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        //string fullpath = "file://" + PLDGlobalDef.STREAMING_PATH + "/" + "Pre3";

        //mWWWLoader = PLDWWWLoader.Create(fullpath, LoaderFinish);
        //mWWWLoader.LoadAsny(LoaderFinish);

        //mAssetBundleLoader = PLDNewAssetBundleLoader.Load(PLDGlobalDef.STREAMING_PATH + "/" + "Pre3", LoadOption.Async, AssetBundleLoaderFinish);

        //mResourceResLoader = PLDResourceResLoader.Load("Pre2");
        //mResourceResLoader.Load();
        //mResourceResLoader.LoadAsny(ResourceResLoaderFinish);

        mAssetFileLoader = PLDAssetFileLoader.Create("Pre2");
        mAssetFileLoader.LoadAsync(AssetFileLoaderFinish);
    }
	
	// Update is called once per frame
	void Update () {
		if(mWWWLoader != null)
        {
            Debug.Log(string.Format("WWWLoader refCount:{0} msg:{1}",mWWWLoader.RefCount, mWWWLoader.Message));
        }

        if (mAssetBundleLoader != null)
        {
            Debug.Log(string.Format("AssetBundleLoader refCount:{0} msg:{1}", mAssetBundleLoader.RefCount, mAssetBundleLoader.Message));
        }

        if (mResourceResLoader != null)
        {
            Debug.Log(string.Format("ResourceResLoader refCount:{0} msg:{1}", mResourceResLoader.RefCount, mResourceResLoader.Message));
        }

        if(mAssetFileLoader != null)
        {
            Debug.Log(string.Format("ResourceResLoader refCount:{0} msg:{1}", mAssetFileLoader.RefCount, mAssetFileLoader.Message));
        }
    }

    private void LoaderFinish(bool isOk, object obj)
    {
        if(isOk)
        {
            WWW wwwObj = obj as WWW;
            if(wwwObj != null)
            {
                Debug.Assert(wwwObj != null, "WWWLoader failed");
                GameObject gameobj = Instantiate(wwwObj.assetBundle.LoadAsset<GameObject>("Pre3"));
                Debug.Log("gameobj name:" + gameobj.name);
            }
        }else
        {
            Debug.Assert(false, "WWW load failed");
        }

        Debug.Log(string.Format("Use time :{0}",mWWWLoader.UseTime));
        //进行一次释放
        PLDResourceLoaderCache.ReleaseLoader(mWWWLoader);
    }

    private void AssetBundleLoaderFinish(bool isOk, object retObj)
    {
        if (isOk)
        {
            AssetBundle assetbundle = retObj as AssetBundle;
            if(assetbundle)
            {
                Debug.Assert(assetbundle != null, "WWWLoader failed");
                GameObject gameobj = Instantiate(assetbundle.LoadAsset<GameObject>("Pre3"));
                Debug.Log("gameobj name:" + gameobj.name);
            }
        }else
        {
            Debug.Assert(false, "AssetBundle load failed");
        }


        Debug.Log(string.Format("Use time :{0}", mAssetBundleLoader.UseTime));
        //进行一次释放
        PLDResourceLoaderCache.ReleaseLoader(mAssetBundleLoader);
    }

    private void ResourceResLoaderFinish(bool isOk, object retObj)
    {
        if (isOk)
        {
            GameObject gameObj = retObj as GameObject;
            if (gameObj)
            {
                Debug.Assert(gameObj != null, "ResourceResLoader failed");
                GameObject gameobj = Instantiate(gameObj);
                Debug.Log("gameobj name:" + gameobj.name);
            }
        }
        else
        {
            Debug.Assert(false, "AssetBundle load failed");
        }


        //Debug.Log(string.Format("Use time :{0}", mResourceResLoader.UseTime));
        //进行一次释放
        //PLDResourceLoaderCache.ReleaseLoader(mResourceResLoader);
    }

    private void AssetFileLoaderFinish(bool isOk, object retObj)
    {
        if (isOk)
        {
            GameObject gameObj = retObj as GameObject;
            if (gameObj)
            {
                Debug.Assert(gameObj != null, "AssetFileLoader failed");
                GameObject gameobj = Instantiate(gameObj);
                Debug.Log("gameobj name:" + gameobj.name);
            }
        }
        else
        {
            Debug.Assert(false, "AssetFileLoader load failed");
        }


        //Debug.Log(string.Format("Use time :{0}", mResourceResLoader.UseTime));
        //进行一次释放
        //PLDResourceLoaderCache.ReleaseLoader(mResourceResLoader);
    }

    private void AssetFileLoaderOnStart()
    {
        Debug.Log("AssetFileLoaderOnStart --------->");
    }
}
