using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour {

    private GridType m_GridType;
    public GridType GridType
    {
        get { return m_GridType; }
        set { m_GridType = value; }
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetState(GridItemState state)
    {
        switch(state)
        {
            case GridItemState.DISABLE:
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                break;
            case GridItemState.ENABLE:
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
                break;
            default:
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
                break;
        }
    }
}
