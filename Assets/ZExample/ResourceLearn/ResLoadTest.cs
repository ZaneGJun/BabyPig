using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

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
		//loadFromResourcesFile();

		//从StreamingAsset中加载资源
		//StartCoroutine(loadFromStreamingAsset());

		//AssetBundle Test
		//加载AssetBundle
		//1.载入AssetBundle
		//2.load成可识别的资源，如Texture,Modle
		//3.Initantiate
		//StartCoroutine(loadAssetBundle());

		PLDResourcesManager.Instance.toPrint ();
	}

	void loadEditor()
	{
		//AssetDatabase可以读取Resources以及其他文件夹中的非AssetBundle资源,StreamingAsset中的文件以及所有AssetBundle类型文件不可读取
		//StreamingAssets中的文件不能读取，必须用WWW
		//AssetBundle类型的所有文件不可读取

		//读取一般文件夹中的prefab,成功
		//GameObject res = AssetDatabase.LoadAssetAtPath("Assets/ZExample/ResourceLearn/Pre3.prefab", typeof(GameObject)) as GameObject ;
		//GameObject obj = Instantiate (res);
		//Debug.Log("obj name:" + obj.name);

		//读取Resources中的prefab,成功
		//GameObject res = AssetDatabase.LoadAssetAtPath("Assets/Resources/Pre3.prefab", typeof(GameObject)) as GameObject ;
		//GameObject obj = Instantiate (res);
		//Debug.Log("obj name:" + obj.name);

		//读取StreamingAssets中的prefab,失败,StreamingAssets中的文件必须用WWW读取
		//GameObject res = AssetDatabase.LoadAssetAtPath("Assets/StreamingAssets/Pre3.prefab", typeof(GameObject)) as GameObject ;
		//GameObject obj = Instantiate (res);
		//Debug.Log("obj name:" + obj.name);

		//AssetBundle类型文件用AssetDatabase读取失败
		//AssetBundle asset = AssetDatabase.LoadAssetAtPath<AssetBundle> ("Assets/Output/resourcelearn/pre3");
		//GameObject obj = Instantiate (asset.LoadAsset<GameObject>("Pre3"));
		//Debug.Log("obj name:" + obj.name);

		//AssetBundle类型文件用AssetBundle读取成功
		//AssetBundle asset = AssetBundle.LoadFromFile("Assets/Output/resourcelearn/pre3");
		//GameObject obj = Instantiate (asset.LoadAsset<GameObject>("Pre3"));
		//Debug.Log("obj name:" + obj.name);
	}

	void loadFromResourcesFile()
	{
		//编辑器下可以用AssetDatabase读取
		//Resources.Load可以在编辑器获取运行时都可读取
		GameObject ooo = Resources.Load<GameObject> ("Pre3");
		Debug.Log ("name:" + ooo.name);
	}

	IEnumerator loadFromStreamingAsset(){
		//在StreamingAssets文件夹中的文件必须用WWW读取
		WWW www = new WWW("file://" + Application.streamingAssetsPath + "/TestConfig.txt");
		yield return www;
		Debug.Log (www.text);
	}

	IEnumerator loadAssetBundle(){
		//可以用WWW获取AssetBundle读取
		WWW www = new WWW ("file://" + Application.streamingAssetsPath + "/pre3");
		yield return null;

		GameObject obj = www.assetBundle.LoadAsset<GameObject> ("Pre3");
		Instantiate (obj);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
