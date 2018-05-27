using System;
using Pld;
using UnityEngine;

public abstract class GridState : PLDState<GridBase>
{
    public GridState(string name)
        :base(name)
    {

    }

    //子类必须实现的方法
    protected abstract void OnEnter();
    protected abstract void OnExit();

    public override void Enter(IPLDStateMachine machine)
    {
        base.Enter(machine);
        Debug.Log(m_Machine.Target.name + " OnEnter State:" + m_Name.ToString());
        OnEnter();
    }

    public override void Exit()
    {
        Debug.Log(m_Machine.Target.name + " OnExit State:" + m_Name.ToString());
        OnExit();
        base.Exit();
    }
}

