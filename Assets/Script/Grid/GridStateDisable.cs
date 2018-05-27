using System;
using System.Collections.Generic;
using UnityEngine;

public class GridStateDisable : GridState
{
    public GridStateDisable(string name) : base(name)
    {
    }

    protected override void OnEnter()
    {
        m_Machine.Target.GetComponent<MeshRenderer>().enabled = false;
        m_Machine.Target.GetComponent<BoxCollider>().enabled = false;
    }

    protected override void OnExit()
    {
        
    }
}

