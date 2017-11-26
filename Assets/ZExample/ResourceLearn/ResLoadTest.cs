using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
#if UNITY_EDITOR
		loadEditor();
#endif

		//从Resources中加载文件
		loadFromResourcesFile();

		//从StreamingAsset中加载资源
		StartCoroutine(loadFromStreamingAsset());

		//AssetBundle Test
		//加载AssetBundle
		//1.载入AssetBundle
		//2.load成可识别的资源，如Texture,Modle
		//3.Initantiate
		StartCoroutine(loadAssetBundle());

	}

	void loadEditor()
	{
		GameObject res = AssetDatabase.LoadAssetAtPath("Assets/ZExample/ResourceLearn/Pre3.prefab", typeof(GameObject)) as GameObject ;
		GameObject obj = Instantiate (res);
		Debug.Log("obj name:" + obj.name);
	}

	void loadFromResourcesFile()
	{
		GameObject ooo = Resources.Load<GameObject> ("Pre3");
		Debug.Log ("name:" + ooo.name);
	}

	IEnumerator loadFromStreamingAsset(){
		WWW www = new WWW("file://" + Application.streamingAssetsPath + "/TestConfig.txt");
		yield return www;
		Debug.Log (www.text);
	}

	IEnumerator loadAssetBundle(){
		WWW www = new WWW ("file://" + Application.streamingAssetsPath + "/pre3");
		yield return null;

		GameObject obj = www.assetBundle.LoadAsset<GameObject> ("Pre3");
		Instantiate (obj);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
