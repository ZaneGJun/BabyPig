using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PoolTesteHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//全局单例初始化
		CommonObjCache.Instance.RegisterCommonObjectPool("TestObject", new Func<GameObject>(genObj), new Action<GameObject>(destroyObj), 20, 50);
		CommonObjCache.Instance.RegisterCommonObjectPool("TestObject2", new Func<GameObject>(genObject2), new Action<GameObject>(destroyObj), 10, 15);

		CommonObjCache.Instance.PrintObjectPools();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onBtn() {
		GameObject obj = CommonObjCache.Instance.Get ("TestObject");
		if (obj != null) {
			Debug.Log ("get obj name:" + obj.name);
			GameObject.Destroy (obj);
		}

		GameObject obj2 = genObject2 ();
		if (obj2 != null) {
			CommonObjCache.Instance.Push (obj2);
		}

		CommonObjCache.Instance.PrintObjectPools ();
	}

	private GameObject genObj() {
#if UNITY_EDITOR
		GameObject obj = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ZExample/Pool/Model/TestObj.prefab"));
		obj.name = obj.name.Replace ("(Clone)","");
		return obj;
#else
		return null;
#endif
	}

	private GameObject genObject2(){
#if UNITY_EDITOR
		GameObject obj = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ZExample/Pool/Model/TestObject2.prefab"));
		obj.name = obj.name.Replace ("(Clone)","");
		return obj;
#else
        return null;
#endif
    }

	private void destroyObj(GameObject obj) {
		GameObject.Destroy (obj);
	}
}
