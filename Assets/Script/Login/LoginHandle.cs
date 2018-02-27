using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public class LoginHandle : MonoBehaviour {

    public Transform LoginPanel;
    public Transform SetPanel;

    private void Awake()
    {
        Debug.Assert(LoginPanel != null);
        Debug.Assert(SetPanel != null);

        LoginPanel.gameObject.SetActive(true);
        SetPanel.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

	public void onBtnStart()
    {
		Debug.Log ("onBtnStart");

        //切换场景
        BabyPigGame.Instance.GoToScene(GameScene.GAME_SCENE);
	}

	public void onBtnSetting()
    {
        Debug.Log ("onBtnSetting");
        LoginPanel.gameObject.SetActive(false);
        SetPanel.gameObject.SetActive(true);
	}

    public void onBtnReturnStart()
    {
        LoginPanel.gameObject.SetActive(true);
        SetPanel.gameObject.SetActive(false);
    }
}
