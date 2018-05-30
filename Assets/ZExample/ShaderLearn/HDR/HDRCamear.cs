using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HDRCamear : MonoBehaviour {

    public enum ToneMappingType
    {
        Reinhard,
        SimpleReinhard,
        ACES
    }

    public Material mMaterial;
    public ToneMappingType toneMappingType = ToneMappingType.Reinhard;
    public float exposureAdjustment;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRenderImage(RenderTexture srcTex, RenderTexture destTex)
    {
        if (mMaterial)
        {
            mMaterial.SetFloat("_ExposureAdjustment", exposureAdjustment);
            Graphics.Blit(srcTex, destTex, mMaterial,(int)toneMappingType);
        }
    }
}
