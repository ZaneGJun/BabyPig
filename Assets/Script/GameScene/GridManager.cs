using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pld;

public enum GridType
{
    NONE,
    GRASS,
}

public enum GridItemState
{
    DISABLE,
    ENABLE
}

public class GridManager : MonoBehaviour {   

    private GameObject [,] m_GridRef;
    private GameObject m_GridParentNode;
    private Vector3 m_GridSize;
    

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

    public void GenGrids(int row, int col, GridType[,] gridTypeData)
    {
        m_GridRef = new GameObject[row, col];

        for(int i=0;i<row;i++)
        {
            for(int j = 0; j < col;j++ )
            {
                 m_GridRef[i, j] = GenOneGrid(i, j, gridTypeData[i,j]);
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

                GridType[,] typeData = new GridType[row, col];

                for(int i=0;i<row;i++)
                    for(int j=0;j<col;j++)
                    {
                        byte rawData = realyData[i * col + j];
                        GridType tmpType = (GridType)(rawData >> 4);
                        typeData[i, j] = tmpType;
                    }

                GenGrids(row, col, typeData);
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

    public GameObject GenOneGrid(int row, int col, GridType gridType = GridType.NONE)
    {
        string gridPrefabFullPath = PLDResourceLoaderSystem.Instance.GetFullPath("Prefab/Grid/NormalGrid.prefab");
        GameObject gridPrefab = PLDAssetFileLoader.Create(gridPrefabFullPath).Load() as GameObject;

        if (m_GridSize.x == 0)
        {
            m_GridSize = gridPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        }

        Vector3 newGridPos = new Vector3(row * m_GridSize.x,0, col * m_GridSize.z);
        GameObject gridGameObject = Instantiate(gridPrefab, newGridPos, new Quaternion(), m_GridParentNode.transform);

        switch(gridType)
        {
            case GridType.NONE:
                gridGameObject.GetComponent<GridItem>().SetState(GridItemState.DISABLE);
                break;
            case GridType.GRASS:
                gridGameObject.GetComponent<GridItem>().SetState(GridItemState.ENABLE);
                break;
            default:
                gridGameObject.GetComponent<GridItem>().SetState(GridItemState.ENABLE);
                break;
        }

        return gridGameObject;
    }

}
