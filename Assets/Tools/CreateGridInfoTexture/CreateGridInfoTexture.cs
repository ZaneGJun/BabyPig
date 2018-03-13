using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pld;
using System;
using UnityEngine.EventSystems;

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

	// Use this for initialization
	void Start () {
        m_LogicGridItemAttachNode = GameObject.Find("MainUI").transform.Find("VirtualTextureInfoPanel/ItemAttachNode").gameObject;
        Debug.Assert(m_LogicGridItemAttachNode != null, "m_LogicGridItemAttachNode not found");
	}
	
	// Update is called once per frame
	void Update () {
        MousePick();

    }

    /// <summary>
    /// byte转为Color
    /// </summary>
    /// <param name="value">要转换的byte值</param>
    /// <returns>转换后返回的Color</returns>
    private static Color ByteToColor(byte value)
    {
        float redValue = (float)value;
        return new Color(redValue/255.0f, 0, 0);
    }

    /// <summary>
    /// color转为byte
    /// </summary>
    /// <param name="color">要转换的color</param>
    /// <returns>返回的byte</returns>
    private static byte ColorToByte(Color color)
    {
        byte[] bytes = BitConverter.GetBytes(color.r);
        return bytes[0];
    }

    /// <summary>
    /// 生成只有单8位通道的图片
    /// </summary>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <param name="rawSingleGridItemData">一个byte保存信息，高4位表示类型，低4位表示高度</param>
    public void CreateNewTexture(int width, int height, byte rawSingleGridItemData)
    {
        //Debug.Assert(width == height, "width not equal to height");

        m_TextureSize = new Rect(0, 0, width, height);
        m_SaveTexture = new Texture2D(width, height, TextureFormat.R8, false);
        m_LogicGridItems = new GameObject[height, width];

        //二进制数据数组
        byte[] rawData = new byte[width*height];

        int index = 0;
        for(int i=0;i< height; i++)
        {
            for(int j=0;j<width;j++)
            {
                rawData[index] = rawSingleGridItemData;
                //生成逻辑网格格子
                m_LogicGridItems[i, j] = GenNewLogicGridItem(i,j, rawSingleGridItemData);
            }
        }

        //将数据load到texture，并保存
        m_SaveTexture.LoadRawTextureData(rawData);
        SaveTexture(m_SaveTexture, "Assets/Tools/CreateGridInfoTexture/bin/mapInfo.map");

        //set scale, height fix 
        //根据宽高进行缩放从而获得更好的显示效果
        int maxSizeLength = (SCREEN_HEIGHT - 100) / height;
        float itemScaleValue = maxSizeLength / DEFAULT_ITEM_LENGTH;
        m_LogicGridItemAttachNode.transform.localScale = new Vector3(itemScaleValue, itemScaleValue, itemScaleValue);
    }

    /// <summary>
    /// 创建逻辑网格格子，根据行列数设定位置,将一个byte的信息用color的R通道表示
    /// </summary>
    /// <param name="row">所在行数</param>
    /// <param name="col">所在列数</param>
    /// <param name="rawSingleGridItemData">一个byte保存信息，高4位表示类型，低4位表示高度</param>
    /// <returns></returns>
    private LogicGridItem GenNewLogicGridItem(int row, int col, byte rawSingleGridItemData)
    {
        GameObject res = (PLDEditorLoader.Create("Assets/Tools/CreateGridInfoTexture/LogicGridItem.prefab").Load()) as GameObject;
        GameObject item = Instantiate<GameObject>(res, m_LogicGridItemAttachNode.transform);

        //set pos
        float halfCol = m_TextureSize.width / 2.0f - 0.5f;
        float halfRow = m_TextureSize.height / 2.0f - 0.5f;
        Vector3 newPos = new Vector3((col-halfCol) * DEFAULT_ITEM_LENGTH, (row-halfRow)*DEFAULT_ITEM_LENGTH,0);
        item.GetComponent<RectTransform>().localPosition = newPos;

        //set color
        item.GetComponent<Image>().color = ByteToColor(rawSingleGridItemData);

        return item;
    }

    //保存图片，获取Texture的二进制数据进行保存
    public void SaveTexture(Texture2D texture, string path)
    {
        texture.Apply();

        //byte[] bytes = texture.EncodeToPNG();
        byte[] bytes = texture.GetRawTextureData();

        Utils.SaveBytesToLocal(bytes, path);
    }

    //读取二进制文件
    public void LoadTexture()
    {
        
    }

    /// <summary>
    /// 开始创建图片回调
    /// </summary>
    public void OnCreateNewTextureBtn()
    {
        m_MainPanel.SetActive(false);
        m_CreateNewTextureWindow.SetActive(true);
        m_VirtualTextureInfpPanel.SetActive(false);
    }

    /// <summary>
    /// 确认创建图片回调
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

        //创建图片
        CreateNewTexture(m_InputRow, m_InputCol, tmpByteColor);
    }

    /// <summary>
    /// 返回主界面回调
    /// </summary>
    public void OnBtnComeToMainPanle()
    {
        m_MainPanel.SetActive(true);
        m_CreateNewTextureWindow.SetActive(false);
        m_VirtualTextureInfpPanel.SetActive(false);
    }

    private void MousePick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log(EventSystem.current.currentSelectedGameObject.name);
            }
        }
    }
}
