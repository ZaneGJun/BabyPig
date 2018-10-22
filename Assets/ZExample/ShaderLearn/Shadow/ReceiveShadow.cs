using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveShadow : MonoBehaviour {

    public Camera lightCamera;

	// Use this for initialization
	void Start () {
        if (Shader.Find("Test/Shadow/ReciveShadow") && lightCamera)
        {
            Matrix4x4 WVMat = lightCamera.worldToCameraMatrix;
            Matrix4x4 PMat = GL.GetGPUProjectionMatrix(lightCamera.projectionMatrix, false);
            Matrix4x4 posToUV = new Matrix4x4();
            posToUV.SetRow(0, new Vector4(0.5f, 0, 0, 0.5f));   //x转换到[0,1]
            posToUV.SetRow(1, new Vector4(0, 0.5f, 0, 0.5f));   //y转换到[0,1]
            posToUV.SetRow(2, new Vector4(0, 0, 1, 0));   //z不转换，仍然是[-1,1]
            posToUV.SetRow(3, new Vector4(0f, 0, 0, 1));
            Matrix4x4 mat = posToUV * PMat * WVMat;
            Shader.SetGlobalMatrix("lightProjectionMatrix", mat);
        }

        float _FilterSize = 1.0f;
        float total = (float)(_FilterSize * 2.0f + 1.0f); //(_FilterSize * 2.0f + 1.0f) * (_FilterSize * 2.0f + 1.0f);

        Debug.Log(total);
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
