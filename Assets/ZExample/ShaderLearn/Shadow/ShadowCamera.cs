using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera cam = this.GetComponent<Camera>();
        if (cam)
        {
            cam.SetReplacementShader(Shader.Find("Test/Shadow/ShadowCatch"), "RenderType");

            
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
