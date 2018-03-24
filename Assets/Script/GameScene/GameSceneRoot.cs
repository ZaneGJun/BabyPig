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

        /*
        GridType[,] gridInfo = new GridType[30, 30];
        for(int i=0;i<30;i++)
        {
            for(int j=0;j<30;j++)
            {
                gridInfo[i, j] = GridType.GRASS;
            }
        }
        m_GridManager.GenGrids(30, 30, gridInfo);
        */

        m_GridManager.GenGridFromTexture(PLDGlobalDef.STREAMING_PATH+"/map/mapInfo.map");
    }
}
