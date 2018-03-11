using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pld;

using UnityEditor;

using LogicGridItem = UnityEngine.GameObject;

public class CreateGridInfoTexture : MonoBehaviour {

    private const int DEFAULT_ITEM_LENGTH = 10;

    private const int SCREEN_WIDTH = 1280;
    private const int SCREEN_HEIGHT = 720;

    private Texture2D m_SaveTexture;
    private Rect m_TextureSize;
    private LogicGridItem[,] m_LogicGridItems;
    private GameObject m_LogicGridItemAttachNode;

    private float m_LogicGridItemSize;

    public GameObject m_MainPanel;
    public GameObject m_CreateNewTextureWindow;
    public GameObject m_VirtualTextureInfpPanel;

	// Use this for initialization
	void Start () {
        m_LogicGridItemAttachNode = GameObject.Find("MainUI").transform.Find("VirtualTextureInfoPanel/ItemAttachNode").gameObject;
        Debug.Assert(m_LogicGridItemAttachNode != null, "m_LogicGridItemAttachNode not found");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //生成只有单8位通道，高4位表示类型，低4位表示高度
    public void CreateNewTexture(int width, int height, Color col)
    {
        //Debug.Assert(width == height, "width not equal to height");

        m_TextureSize = new Rect(0, 0, width, height);
        m_SaveTexture = new Texture2D(width, height, TextureFormat.R8, false);
        m_LogicGridItems = new GameObject[height, width];

        for(int i=0;i< height; i++)
        {
            for(int j=0;j<width;j++)
            {
                //m_SaveTexture.SetPixel(i, j, col);
                m_LogicGridItems[i, j] = GenNewLogicGridItem(i,j,col);
            }
        }

        //set scale, height fix 
        int maxSizeLength = (SCREEN_HEIGHT - 100) / height;
        float itemScaleValue = maxSizeLength / DEFAULT_ITEM_LENGTH;
        m_LogicGridItemAttachNode.transform.localScale = new Vector3(itemScaleValue, itemScaleValue, itemScaleValue);
    }

    private LogicGridItem GenNewLogicGridItem(int row, int col, Color color)
    {
        GameObject res = (PLDEditorLoader.Create("Assets/Tools/CreateGridInfoTexture/LogicGridItem.prefab").Load()) as GameObject;
        GameObject item = Instantiate<GameObject>(res, m_LogicGridItemAttachNode.transform);

        //set pos
        float halfCol = m_TextureSize.width / 2.0f - 0.5f;
        float halfRow = m_TextureSize.height / 2.0f - 0.5f;
        Vector3 newPos = new Vector3((col-halfCol) * DEFAULT_ITEM_LENGTH, (row-halfRow)*DEFAULT_ITEM_LENGTH,0);
        item.GetComponent<RectTransform>().localPosition = newPos;

        //set color
        item.GetComponent<Image>().color = color;

        return item;
    }

    //保存图片
    public void SaveTexture(Texture2D texture, string path)
    {
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        //or byte[] bytes = texture.GetRawTextureData();

        Utils.SaveBytesToLocal(bytes, path);

    }

    public void OnCreateNewTextureBtn()
    {
        m_MainPanel.SetActive(false);
        m_CreateNewTextureWindow.SetActive(true);
        m_VirtualTextureInfpPanel.SetActive(false);
    }

    public void OnSureCreateBtn()
    {
        m_MainPanel.SetActive(false);
        m_CreateNewTextureWindow.SetActive(false);
        m_VirtualTextureInfpPanel.SetActive(true);

        CreateNewTexture(4, 4, new Color(150,0,0));
    }

    public void OnBtnComeToMainPanle()
    {
        m_MainPanel.SetActive(true);
        m_CreateNewTextureWindow.SetActive(false);
        m_VirtualTextureInfpPanel.SetActive(false);
    }
}
