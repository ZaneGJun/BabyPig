using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTesteHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//全局单例初始化
		TestObjCache.Instance.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onBtn() {


		TestObjCache.Instance.printCacheCount ();
	}
}
