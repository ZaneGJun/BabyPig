using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pld;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour {

    private GameObject m_Model = null;
    private Animator m_Animator;

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
        m_Model = Instantiate(res, transform) as GameObject;

        m_Animator = m_Model.GetComponent<Animator>();
    }

}
