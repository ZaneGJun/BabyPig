using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public enum GridType
{
    NONE,
    GRASS,
}

public struct GridInfoData
{
    public GridType type;
    public int height;

    public GridInfoData(GridType type = GridType.NONE, int height = 0)
    {
        this.type = type;
        this.height = height;
    }
}

public class GridManager : MonoBehaviour {   

    private GameObject [,] m_GridRef;
    private GameObject m_GridParentNode;
    private Vector3 m_GridSize;

    public static float GridGenInterval = 0.15f;
    

	// Use this for initialization
	void Start () {
        m_GridParentNode = new GameObject("GridParentNode");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clear()
    {
        m_GridRef = null;
    }

    public IEnumerator GenGrids(int row, int col, GridInfoData[,] gridData)
    {
        m_GridRef = new GameObject[row, col];

        for(int i=0;i<row;i++)
        {
            for(int j = 0; j < col;j++ )
            {
                m_GridRef[i,j] = GenOneGrid(i, j, gridData[i,j]);

                if (gridData[i, j].type != GridType.NONE)
                    yield return new WaitForSeconds(GridGenInterval);
                else
                    yield return null;
            }
        }
    }

    public void GenGridFromTexture(string texturePath)
    {
        PLDWWWLoader loader = PLDWWWLoader.Create(texturePath);
        loader.LoadAsync((bool isSuccess, object result) =>
        {
            if(isSuccess)
            {
                WWW www = result as WWW;
                byte[] rawdata = www.bytes;
                int size = www.bytesDownloaded;
                int row = rawdata[0];
                int col = rawdata[1];

                byte[] realyData = new byte[size - 2];
                Utils.CopyBytes(rawdata, 2, realyData, 0, size - 2);

                GridInfoData[,] typeData = new GridInfoData[row, col];

                for(int i=0;i<row;i++)
                    for(int j=0;j<col;j++)
                    {
                        byte rawData = realyData[i * col + j];
                        int tmp = (int)rawData;

                        GridInfoData data;
                        data.type = (GridType)(rawData >> 4);
                        data.height = tmp & 15;
                        typeData[i, j] = data;
                    }

                StartCoroutine(GenGrids(row, col, typeData));
            }
        });

        /*
        Texture2D gridInfoTexture = PLDAssetFileLoader.Create(texturePath).Load() as Texture2D;
        m_GridRef = new GameObject[gridInfoTexture.width, gridInfoTexture.height];

        for (int i = 0; i < gridInfoTexture.width; i++)
        {
            for (int j = 0; i < gridInfoTexture.height; j++)
            {
                //Color color = gridInfoTexture.GetPixel(i, j);
                
            }
        }
        */
    }

    public GameObject GenOneGrid(int row, int col, GridInfoData gridInfoData, float delay = 0)
    {
        string gridPrefabFullPath = PLDResourceLoaderSystem.Instance.GetFullPath("Prefab/Grid/NormalGrid.prefab");
        GameObject gridPrefab = PLDAssetFileLoader.Create(gridPrefabFullPath).Load() as GameObject;

        if (m_GridSize.x == 0)
        {
            m_GridSize = gridPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        }

        Vector3 newGridPos = new Vector3(col * m_GridSize.x, gridInfoData.height, row * m_GridSize.z);
        Vector3 genPos = newGridPos;
        genPos.y += 30;

        GameObject gridGameObject = Instantiate(gridPrefab, genPos, new Quaternion(), m_GridParentNode.transform);
        gridGameObject.GetComponent<GridItem>().GType = gridInfoData.type;
        gridGameObject.GetComponent<GridItem>().Height = gridInfoData.height;
        gridGameObject.GetComponent<GridItem>().InitStateMachine();

        switch (gridInfoData.type)
        {
            case GridType.NONE:
                gridGameObject.GetComponent<GridItem>().StateMachine.ChangeState("Disable");
                break;
            case GridType.GRASS:
                gridGameObject.GetComponent<GridItem>().StateMachine.ChangeState("PreReady");
                break;
            default:
                gridGameObject.GetComponent<GridItem>().StateMachine.ChangeState("PreReady");
                break;
        }

        return gridGameObject;
    }

}
