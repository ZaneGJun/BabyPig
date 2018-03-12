using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pld;
using System;

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

    public GameObject m_RowInput;
    public GameObject m_ColInput;
    public GameObject m_TypeInput;
    public GameObject m_HeightInput;

    private int m_InputRow;
    private int m_InputCol;
    private int m_InputType;
    private int m_InputHeight;
    //8位的单像素格式，高4位为类型，低4位为高度
    private byte m_RawTexturePixel;

	// Use this for initialization
	void Start () {
        m_LogicGridItemAttachNode = GameObject.Find("MainUI").transform.Find("VirtualTextureInfoPanel/ItemAttachNode").gameObject;
        Debug.Assert(m_LogicGridItemAttachNode != null, "m_LogicGridItemAttachNode not found");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static Color ByteToColor(byte value)
    {
        byte[] bytes = new byte[1]{value};
        float redValue = BitConverter.ToSingle(bytes, 0);
        return new Color(redValue, 0, 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static byte ColorToByte(Color color)
    {
        byte[] bytes = BitConverter.GetBytes(color.r);
        return bytes[0];
    }

    /// <summary>
    ///  生成只有单8位通道，高4位表示类型，低4位表示高度
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="color"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    public void OnCreateNewTextureBtn()
    {
        m_MainPanel.SetActive(false);
        m_CreateNewTextureWindow.SetActive(true);
        m_VirtualTextureInfpPanel.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnSureCreateBtn()
    {
        m_MainPanel.SetActive(false);
        m_CreateNewTextureWindow.SetActive(false);
        m_VirtualTextureInfpPanel.SetActive(true);

        m_InputRow = StringUtil.StringToInt(m_RowInput.GetComponent<InputField>().text);
        m_InputCol = StringUtil.StringToInt(m_ColInput.GetComponent<InputField>().text);
        m_InputType = StringUtil.StringToInt(m_TypeInput.GetComponent<InputField>().text);
        m_InputHeight = StringUtil.StringToInt(m_HeightInput.GetComponent<InputField>().text);

        m_InputType = Math.Min(15,m_InputType);
        m_InputHeight = Math.Min(15, m_InputHeight);

        byte tmpType = BitConverter.GetBytes(m_InputType)[0];
        byte tmpHeight = BitConverter.GetBytes(m_InputHeight)[0];

        byte tmpByteColor = (byte)((tmpType << 4) + tmpHeight);
        Color color = ByteToColor(tmpByteColor);

        CreateNewTexture(m_InputRow, m_InputCol, color);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnBtnComeToMainPanle()
    {
        m_MainPanel.SetActive(true);
        m_CreateNewTextureWindow.SetActive(false);
        m_VirtualTextureInfpPanel.SetActive(false);
    }
}
