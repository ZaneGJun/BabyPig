using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public class ResManagerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadFromResources ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//打包后只能从Resources,StreamingAssets,Persistant加载资源
	void LoadFromResources()
	{
		//同步加载
		GameObject obj = PLDResourcesManager.Instance.Load<GameObject> ("Pre2",LoadType.LOAD_FROM_RESOURCES);
		Debug.Assert (obj != null);
		Debug.Log ("LoadFromResources sync success:" + obj.name);

		GameObject cloneObj = Instantiate (obj);
		Debug.Assert (cloneObj != null);
		Debug.Log ("LoadFromResources sync Instantiate success:" + cloneObj.name);

		GameObject bb = Resources.Load<GameObject> ("pre3") ;
		Debug.Assert (bb != null);

		//AssetBundle bb2 = AssetBundle.LoadFromFile ("Assets/Resources/pre3") ;
		//Debug.Assert (bb2 != null);
		//Debug.Log (bb2.LoadAsset("pre3").name);

		//AssetBundle bundle = PLDResourcesManager.Instance.Load ("pre3", LoadType.LOAD_FROM_RESOURCES);
		//Debug.Assert (bundle != null);
		//Debug.Log ("LoadFromResources AssetBundle succcess" + bundle.name);

		//异步加载
		PLDResourcesManager.Instance.LoadAsync<GameObject> ("Pre2", LoadType.LOAD_FROM_RESOURCES, onLoadFromResources);
	}

	void onLoadFromResources(GameObject loadObj)
	{
		Debug.Assert (loadObj != null);
		Debug.Log ("onLoadFromResources async success:" + loadObj.name);

		GameObject cloneObj = Instantiate (loadObj);
		Debug.Assert (cloneObj != null);
		Debug.Log ("onLoadFromResources async Instantiate success:" + cloneObj.name);

	}

	void LoadFromStreamingAssets()
	{
		//

	}

	void LoadFromPersistant()
	{
	}
}
