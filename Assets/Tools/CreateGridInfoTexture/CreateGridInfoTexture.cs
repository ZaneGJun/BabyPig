using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pld;
using System;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

using LogicGridItem = UnityEngine.GameObject;

public class CreateGridInfoTexture : MonoBehaviour {

    private const int DEFAULT_ITEM_LENGTH = 10;

    private const int SCREEN_WIDTH = 1280;
    private const int SCREEN_HEIGHT = 720;

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

    #region Tool
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
        int realVal = (int)(color.r * 255.0f);
        realVal = realVal & 511;
        return (byte)realVal;
    }
    #endregion

    /// <summary>
    /// 显示只有单8位通道的逻辑图片信息
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="col">列</param>
    /// <param name="rawSingleGridItemData">一个byte保存信息，高4位表示类型，低4位表示高度,所有逻辑像素用这个颜色填充</param>
    public void CreateNewTexture(int row, int col, byte rawSingleGridItemData)
    {
        //Debug.Assert(row == col, "width not equal to height");
        if (row == 0 || col == 0)
            return;

        m_TextureSize = new Rect(0, 0, col, row);
        m_LogicGridItems = new GameObject[row, col];

        //Grid Layout
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraintCount = col;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.UpperLeft;
        int maxSizeLength = (SCREEN_HEIGHT - 200) / row;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().cellSize = new Vector2(maxSizeLength, maxSizeLength);

        for(int i=0;i< row; i++)
        {
            for(int j=0;j<col;j++)
            {
                //生成逻辑网格格子
                m_LogicGridItems[i, j] = GenNewLogicGridItem(i,j, rawSingleGridItemData);
                //GenNewLogicGridItem(i, j, rawSingleGridItemData);
            }
        }
    }

    /// <summary>
    /// 用本地文件数据显示只有单8位通道的逻辑图片信息
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="col">列</param>
    /// <param name="rawGridItemDatas">图片的像素信息</param>
    public void CreateNewTextureFromFile(int row, int col, byte[] rawGridItemDatas)
    {
        //Debug.Assert(row == col, "width not equal to height");
        if (row == 0 || col == 0)
            return;

        m_TextureSize = new Rect(0, 0, col, row);
        m_LogicGridItems = new GameObject[row, col];

        //Grid Layout
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraintCount = col;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().startCorner = GridLayoutGroup.Corner.LowerLeft;
        int maxSizeLength = (SCREEN_HEIGHT - 200) / row;
        m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().cellSize = new Vector2(maxSizeLength, maxSizeLength);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //生成逻辑网格格子
                m_LogicGridItems[row-i-1, j] = GenNewLogicGridItem(row-i-1, j, rawGridItemDatas[i*col + j]);
                //GenNewLogicGridItem(row - i - 1, j, rawGridItemDatas[i * col + j]);
            }
        }
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
        PLDResourceResLoader loader = PLDResourceResLoader.Create("LogicGridItem");
        GameObject resObj = loader.Load() as GameObject;
        Debug.Assert(resObj != null, "Resources Load prefab faild");

        GameObject item = Instantiate<GameObject>(resObj, m_LogicGridItemAttachNode.transform);

        //set color
        item.GetComponent<Image>().color = ByteToColor(rawSingleGridItemData);
        item.GetComponent<ToolGridItem>().Col = col;
        item.GetComponent<ToolGridItem>().Row = row;

        return item;
    }

    //读取二进制文件,生成图片显示
    public void LoadTexture(string path)
    {
        if (path == "")
            return;

        Utils.ReadBytes(path, (bool isSuccess, object result) =>
         {
             if(isSuccess)
             {
                 WWW www = result as WWW;
                 byte[] rawdata = www.bytes;
                 int size = www.bytesDownloaded;
                 int row = rawdata[0];
                 int col = rawdata[1];

                 byte[] realyData = new byte[size-2];
                 Utils.CopyBytes(rawdata, 2, realyData, 0, size - 2);

                 m_TextureSize = new Rect(0, 0, col, row);
                 m_LogicGridItems = new GameObject[row, col];

                 //Grid Layout
                 m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                 m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().constraintCount = col;
                 int maxSizeLength = (SCREEN_HEIGHT - 200) / row;
                 m_LogicGridItemAttachNode.GetComponent<GridLayoutGroup>().cellSize = new Vector2(maxSizeLength, maxSizeLength);

                 CreateNewTextureFromFile(row, col, realyData);

                 /*
                 byte[] textureRawData = new byte[row * col * 4];
                 for(int i=0, j =0;i<size-2;i++, j+=4)
                 {
                     textureRawData[j] = realyData[i];
                     textureRawData[j+1] = 0;
                     textureRawData[j+2] = 0;
                     textureRawData[j+3] = 255;
                 }

                 Texture2D texture = new Texture2D(col,row,TextureFormat.RGBA32,false);
                 texture.LoadRawTextureData(textureRawData);
                 texture.Apply();

                 Sprite sp = Sprite.Create(texture, new Rect(0, 0, col, row), Vector2.zero);

                 LogicGridItem newItem = GenNewLogicGridItem(0, 0, 0);
                 newItem.GetComponent<Image>().sprite = sp;
                 newItem.transform.SetParent(m_MainPanel.transform);
                 */
             }
         });
    }

    //载入图片
    public void OnBtnLoadTexture()
    {
        m_MainPanel.SetActive(false);
        m_VirtualTextureInfpPanel.SetActive(true);

        string path = Utils.ShowDialogGetSelectFilePath();
        LoadTexture(path);
    }

    /// <summary>
    /// 保存最终修改的图片
    /// </summary>
    public void OnBtnSaveResultTexture()
    {
        int row = (int)m_TextureSize.height;
        int col = (int)m_TextureSize.width;
        //二进制数据数组
        byte[] rawData = new byte[row * col + 2];
        rawData[0] = (byte)row;
        rawData[1] = (byte)col;

        Texture2D compareNormalTexture = new Texture2D(col, row, TextureFormat.RGBA32, false);

        int index = 2;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //生成逻辑网格格子
                rawData[index] = ColorToByte(m_LogicGridItems[row-i-1, j].GetComponent<Image>().color);
                compareNormalTexture.SetPixel(j, i, m_LogicGridItems[row-i-1, j].GetComponent<Image>().color);

                ++index;
            }
        }

        //将数据保存
        string savePath = Utils.ShowDialogGetSaveFilePath();
        if (savePath == "")
            return;

        if (!Utils.HaveExt(savePath))
            savePath += ".map";
        Utils.SaveBytesToLocal(rawData, savePath);

        //并保存一幅Png图作对比
        byte[] bytes = compareNormalTexture.EncodeToPNG();
        string pngPath = Utils.RemoveExt(savePath);
        Utils.SaveBytesToLocal(bytes, pngPath + ".png");
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
