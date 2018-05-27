using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public class ResManagerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //LoadResourcesTest();

        LoadAssetBundleTest ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    ////////////////////////////Resources加载/////////////////////////////////////////////

    //打包后只能从Resources,StreamingAssets,Persistant加载资源
    void LoadResourcesTest()
	{
		//同步加载
		GameObject obj = PLDResourcesManager.Instance.Load<GameObject> ("Pre2",LoadType.LOAD_FROM_RESOURCES);
		Debug.Assert (obj != null);
		Debug.Log ("LoadFromResources sync success:" + obj.name);

		GameObject cloneObj = Instantiate (obj);
		Debug.Assert (cloneObj != null);
		Debug.Log ("LoadFromResources sync Instantiate success:" + cloneObj.name);

		//异步加载
		PLDResourcesManager.Instance.LoadAsync<GameObject> ("Pre2", LoadType.LOAD_FROM_RESOURCES, onLoadResourcesTest);
	}

    //异步加载完成回调
	void onLoadResourcesTest(GameObject loadObj)
	{
		Debug.Assert (loadObj != null);
		Debug.Log ("onLoadFromResources async success:" + loadObj.name);

		GameObject cloneObj = Instantiate (loadObj);
		Debug.Assert (cloneObj != null);
		Debug.Log ("onLoadFromResources async Instantiate success:" + cloneObj.name);

	}

	/////////////////////////////AssetBundle加载/////////////////////////////////////////////
    void LoadAssetBundleTest()
    {
        //---------------------------------------------------------------------
        //同步加载
        //StreamingAssets
        AssetBundle assetbundleStreamingAssets = PLDResourcesManager.Instance.Load("pre3", LoadType.LOAD_FROM_STREAMING_ASSETS);
        Debug.Assert(assetbundleStreamingAssets != null,"load assetbundle streamingAssets failed");
        GameObject objStreamingAssets = Instantiate(assetbundleStreamingAssets.LoadAsset<GameObject>("Pre3"));
        Debug.Log("obj name:" + objStreamingAssets.name);
        
#if !UNITY_EDITOR
        //Persistent
        AssetBundle assetbundlePersistent = PLDResourcesManager.Instance.Load("pre3", LoadType.LOAD_FROM_PERSISTANT);
        Debug.Assert(assetbundlePersistent != null, "load assetbundle persistent failed");
        GameObject objPersistent = Instantiate(assetbundlePersistent.LoadAsset<GameObject>("Pre3"));
        Debug.Log("obj name:" + objPersistent.name);
#endif

        //---------------------------------------------------------------------
        //异步加载
        //AssetBundle实现,StreamingAssets
        PLDResourcesManager.Instance.LoadAsync("Pre3", LoadType.LOAD_FROM_STREAMING_ASSETS, onLoadAssetBundleTest);

#if !UNITY_EDITOR
        //AssetBundle实现,Persistent
        PLDResourcesManager.Instance.LoadAsync("Pre3", LoadType.LOAD_FROM_PERSISTANT, onLoadAssetBundleTest);
#endif

        //WWW实现,StreamingAssets
        PLDResourcesManager.Instance.LoadAsync("Pre3", LoadType.LOAD_FROM_STREAMING_ASSETS, onLoadAssetBundleWWWTest);

#if !UNITY_EDITOR
        //WWW实现,Persistent
        PLDResourcesManager.Instance.LoadAsync("Pre3", LoadType.LOAD_FROM_PERSISTANT, onLoadAssetBundleWWWTest);
#endif
    }

    void onLoadAssetBundleTest(AssetBundle loadAssetBundle)
    {
        Debug.Assert(loadAssetBundle != null, "load assetbundle Async failed");
        GameObject obj = Instantiate(loadAssetBundle.LoadAsset<GameObject>("Pre3"));
        Debug.Log("obj name:" + obj.name);
    }

    void onLoadAssetBundleWWWTest(WWW wwwObj)
    {
        Debug.Assert(wwwObj != null, "load assetbundle Async WWW failed");
        GameObject obj = Instantiate(wwwObj.assetBundle.LoadAsset<GameObject>("Pre3"));
        Debug.Log("obj name:" + obj.name);
    }
}
