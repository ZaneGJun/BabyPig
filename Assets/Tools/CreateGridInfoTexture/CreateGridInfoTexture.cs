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

    public GameObject m_MainPanel;
    public GameObject m_CreateNewTextureWindow;
    public GameObject m_VirtualTextureInfpPanel;
    public Transform m_EditorWindow;

    public GameObject m_RowInput;
    public GameObject m_ColInput;
    public GameObject m_TypeInput;
    public GameObject m_HeightInput;

    public GameObject m_EditorTypeInput;
    public GameObject m_EditorHeightInput;

    private Transform m_CurSelectedGridItem;

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
        if (width == 0 || height == 0)
            return;

        m_TextureSize = new Rect(0, 0, width, height);
        m_SaveTexture = new Texture2D(width, height, TextureFormat.R8, false);
        m_LogicGridItems = new GameObject[height, width];

        //Grid Layout
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraintCount = width;
        int maxSizeLength = (SCREEN_HEIGHT - 100) / height;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().cellSize = new Vector2(maxSizeLength, maxSizeLength);


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

                ++index;
            }
        }

        //将数据load到texture，并保存
        m_SaveTexture.LoadRawTextureData(rawData);
        SaveTexture(m_SaveTexture, "Assets/Tools/CreateGridInfoTexture/bin/mapInfo.map");
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

        //set color
        item.GetComponent<Image>().color = ByteToColor(rawSingleGridItemData);
        item.GetComponent<ToolGridItem>().Col = col;
        item.GetComponent<ToolGridItem>().Row = row;

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
    /// 保存最终修改的图片
    /// </summary>
    public void OnBtnSaveResultTexture()
    {
        int height = (int)m_TextureSize.height;
        int width = (int)m_TextureSize.width;
        //二进制数据数组
        byte[] rawData = new byte[width * height];

        Texture2D compareNormalTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        int index = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //生成逻辑网格格子
                rawData[index] = ColorToByte(m_LogicGridItems[i, j].GetComponent<Image>().color);

                compareNormalTexture.SetPixel(j, height - i, m_LogicGridItems[i, j].GetComponent<Image>().color);

                ++index;
            }
        }

        //将数据load到texture，并保存
        m_SaveTexture.LoadRawTextureData(rawData);
        SaveTexture(m_SaveTexture, "Assets/Tools/CreateGridInfoTexture/bin/mapInfo.map");

        byte[] bytes = compareNormalTexture.EncodeToPNG();
        Utils.SaveBytesToLocal(bytes, "Assets/Tools/CreateGridInfoTexture/bin/compare.png");

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

        int inputRow = StringUtil.StringToInt(m_RowInput.GetComponent<InputField>().text);
        int inputCol = StringUtil.StringToInt(m_ColInput.GetComponent<InputField>().text);
        int inputType = StringUtil.StringToInt(m_TypeInput.GetComponent<InputField>().text);
        int inputHeight = StringUtil.StringToInt(m_HeightInput.GetComponent<InputField>().text);

        inputType = Math.Min(15, inputType);
        inputHeight = Math.Min(15, inputHeight);

        byte tmpType = BitConverter.GetBytes(inputType)[0];
        byte tmpHeight = BitConverter.GetBytes(inputHeight)[0];
        byte tmpByteColor = (byte)((tmpType << 4) + tmpHeight);

        //创建图片
        CreateNewTexture(inputRow, inputCol, tmpByteColor);
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

    /// <summary>
    /// 确定修改
    /// </summary>
    public void OnBtnChangeItemSure()
    {
        m_EditorWindow.gameObject.SetActive(false);

        if (m_CurSelectedGridItem)
        {
            int changeType = StringUtil.StringToInt(m_EditorTypeInput.GetComponent<InputField>().text);
            int changeHeight = StringUtil.StringToInt(m_EditorHeightInput.GetComponent<InputField>().text);

            changeType = Math.Min(15, changeType);
            changeHeight = Math.Min(15, changeHeight);

            byte tmpType = BitConverter.GetBytes(changeType)[0];
            byte tmpHeight = BitConverter.GetBytes(changeHeight)[0];
            byte tmpByteColor = (byte)((tmpType << 4) + tmpHeight);
            m_CurSelectedGridItem.GetComponent<Image>().color = ByteToColor(tmpByteColor);
        }
    }

    //鼠标点击处理
    private void MousePick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject touchObject = UtilUI.GetTopTouchGameObject();
                if (touchObject && touchObject.tag=="LogicGridItem")
                {
                    if(m_CurSelectedGridItem == null)
                    {
                        m_CurSelectedGridItem = touchObject.transform;
                        m_CurSelectedGridItem.GetComponent<Animation>().Play();
                        m_CurSelectedGridItem.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                    }

                    if(m_CurSelectedGridItem != touchObject.transform)
                    {
                        //当前选中的跟上一次的不同
                        Animation ani = m_CurSelectedGridItem.GetComponent<Animation>();
                        AnimationState state = ani["GridItemHighLight"];
                        if (state)
                        {
                            //上一次的item采样到开始状态，然后停止
                            state.time = 0;
                            m_CurSelectedGridItem.GetComponent<Animation>().Sample();
                            m_CurSelectedGridItem.GetComponent<Animation>().Stop();
                        }

                        //当前选中的开始播放动画
                        m_CurSelectedGridItem = touchObject.transform;
                        m_CurSelectedGridItem.GetComponent<Animation>().Play();
                        m_CurSelectedGridItem.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                    }

                    m_EditorWindow.gameObject.SetActive(true);
                }
        
            }
        }
    }

    
}
