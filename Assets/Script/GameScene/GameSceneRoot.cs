using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public class GameSceneRoot : MonoBehaviour {

    private Player mPlayer;

    void Awake()
    {
        mPlayer = this.gameObject.AddComponent(typeof(Player)) as Player;

        
    }

    // Use this for initialization
    void Start () {

       
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    
}
