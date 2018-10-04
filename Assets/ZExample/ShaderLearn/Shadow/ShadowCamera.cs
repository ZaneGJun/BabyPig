using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera cam = this.GetComponent<Camera>();
        if (cam)
        {
            Debug.Log("get camera success");
            if (Shader.Find("Test/Shadow/ShadowCatch"))
            {
                Debug.Log("get shader ShadowCatch");
            }

            cam.SetReplacementShader(Shader.Find("Test/Shadow/ShadowCatch"), "RenderType");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
