using System;
using Pld;
using UnityEngine;

public class GridStatePreReady : GridState
{
    public GridStatePreReady(string name) : base(name)
    {
       
    }

    protected override void OnEnter()
    {
        m_Machine.Target.GetComponent<MeshRenderer>().enabled = true;
        m_Machine.Target.GetComponent<BoxCollider>().enabled = false;

        m_Machine.ChangeState("Fall");
    }

    protected override void OnExit()
    {
        
    }
}

