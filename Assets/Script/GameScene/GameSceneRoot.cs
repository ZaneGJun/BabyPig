using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pld;

public class GameSceneRoot : MonoBehaviour {

    private Player m_Player;
    private GridManager m_GridManager;

    void Awake()
    {
        //m_Player = gameObject.AddComponent<Player>();
        m_GridManager = gameObject.AddComponent<GridManager>();
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(StartGenGrid());
       
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private IEnumerator StartGenGrid()
    {
        yield return new WaitForSeconds(2);

        m_GridManager.GenGridFromTexture(PLDGlobalDef.STREAMING_PATH+"/map/mapInfo.map");
    }
}
