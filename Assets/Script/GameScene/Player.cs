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
        var loader = PLDAssetFileLoader.Create(fullpath);
        loader.LoadAsync(onLoadFinish);
        //GameObject res = loader.Load() as GameObject;
        //mModel = Instantiate(res, transform) as GameObject;
        //if (mModel == null)
        //{
        //    Debug.Assert(false, "load prefab failed");
        //}

    }

    private void onLoadFinish(bool isOk, object obj)
    {
        Debug.Log(isOk);
        if (isOk)
        {
            GameObject tmpObj = obj as GameObject;
            mModel = Instantiate(tmpObj, transform) as GameObject;
            if (mModel == null)
            {
                Debug.Assert(false, "load prefab failed");
            }
        }
    }

    
}
