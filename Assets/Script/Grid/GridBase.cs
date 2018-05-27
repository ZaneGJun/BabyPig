using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridBase : MonoBehaviour
{
    public GridBase()
    {
        m_StateMachine = new GridStateMachine(this);
    }

    protected GridType m_GridType;
    public GridType GridType
    {
        get { return m_GridType; }
        set { m_GridType = value; }
    }

    protected GridStateMachine m_StateMachine;
    public GridStateMachine StateMachine
    {
        get { return m_StateMachine; }
    }

    //子类必须重写
    protected abstract List<GridState> GetStates();

    public virtual void InitStateMachine()
    {
        List<GridState> states = GetStates();

        for(int i = 0; i < states.Count; i++)
        {
            m_StateMachine.AddState(states[i]);
        }
    }
}

