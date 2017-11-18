using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;
using System;

public class TestObjCache : PLDMOSingleton<TestObjCache> {

	//对象池
	private PLDCommonPool<GameObject> mPool;
	private Func<GameObject> mPoolCreateMethod; 
	private Action<GameObject> mPoolDestroyAction;

	void Start() {
		Debug.Log ("TestObjCache Start");
	}

	void Update() {
		
	}

	public override void Init() {
		Debug.Log ("TestObjCache Init");

		mPoolCreateMethod = new Func<GameObject> (genObj);
		mPoolDestroyAction = new Action<GameObject> (destroyObj);
		mPool = new PLDCommonPool<GameObject> (mPoolCreateMethod, mPoolDestroyAction, 20);
	}

	public void printCacheCount() {
		Debug.Log (string.Format("Pool Count: {0}", mPool.CurCount));
	}

	private GameObject genObj() {
		GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
		return obj;
	}

	private void destroyObj(GameObject obj) {
		GameObject.Destroy (obj);
	}
}
