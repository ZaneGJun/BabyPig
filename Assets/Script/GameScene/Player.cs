using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour {

    private GameObject mModel = null;

	// Use this for initialization
	void Start () {
        loadGameModle();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void loadGameModle()
    {
        string fullpath = PLDResourceLoaderSystem.Instance.GetFullPath("Prefab/tiger.prefab");
        Debug.Log("fullpath:" + fullpath);
        GameObject res = PLDAssetFileLoader.Create(fullpath).Load() as GameObject;
        mModel = Instantiate(res, transform) as GameObject;
    }

}
