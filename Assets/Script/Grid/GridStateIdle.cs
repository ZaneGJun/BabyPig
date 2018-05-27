using System;
using UnityEngine;

public class GridStateIdle : GridState
{
    public GridStateIdle(string name) : base(name)
    {

    }

    protected override void OnEnter()
    {
        m_Machine.Target.GetComponent<MeshRenderer>().enabled = true;
        m_Machine.Target.GetComponent<BoxCollider>().enabled = true;
    }

    protected override void OnExit()
    {
        
    }
}

