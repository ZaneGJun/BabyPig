using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public class NewLoaderTest : MonoBehaviour {

    private PLDWWWLoader mWWWLoader = null;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        string fullpath = "file://" + PLDGlobalDef.STREAMING_PATH + "/" + "Pre3";
        mWWWLoader = PLDWWWLoader.Load(fullpath, LoaderFinish);
	}
	
	// Update is called once per frame
	void Update () {
		if(mWWWLoader != null)
        {
            Debug.Log(string.Format("WWWLoader msg:{0}", mWWWLoader.Message));
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
    }
}
