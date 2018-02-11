using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Pld;

/// <summary>
/// 游戏场景
/// </summary>
public enum GameScene
{
    START_SCENE = 0,
    GAME_SCENE,
}

/// <summary>
/// Baby pig game.
/// 全局游戏单例
/// </summary>
public class BabyPigGame : PLDMOSingleton<BabyPigGame> {

	//
	void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 切换场景
    public void GoToScene(GameScene scene)
    {
        SceneManager.LoadScene((int)scene);
        
    }
}
