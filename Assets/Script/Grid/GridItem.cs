using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : GridBase
{
    protected GridType m_Type;
    public GridType GType
    {
        get { return m_Type; }
        set { m_Type = value; } 
    }

    protected float m_Height;
    public float Height
    {
        get { return m_Height; }
        set { m_Height = value; }
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override List<GridState> GetStates()
    {
        List<GridState> states = new List<GridState>();

        states.Add(new GridStateDisable("Disable"));
        states.Add(new GridStatePreReady("PreReady"));
        states.Add(new GridStateFall("Fall", m_Height, 2.0f));
        states.Add(new GridStateIdle("Idle"));

        return states;
    }
}
