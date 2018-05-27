using System;
using UnityEngine;
using DG.Tweening;

public class GridStateFall : GridState
{
    private float m_EndHeight = 0.0f;
    public float EndHeight
    {
        get { return m_EndHeight; }
        set { m_EndHeight = value; }
    }

    private float m_FallDuration = 0.0f;
    public float FallDuration
    {
        get { return m_FallDuration; }
        set { m_FallDuration = value; }
    }

    public GridStateFall(string name, float endHeight, float fallDuration) : base(name)
    {
        m_EndHeight = endHeight;
        m_FallDuration = fallDuration;
    }

    protected override void OnEnter()
    {
        m_Machine.Target.GetComponent<MeshRenderer>().enabled = true;
        m_Machine.Target.GetComponent<BoxCollider>().enabled = false;

        StartFall();
    }

    private void StartFall()
    {
        m_Machine.Target.transform.DOMoveY(m_EndHeight, m_FallDuration).OnComplete(FallEnd);
    }

    private void FallEnd()
    {
        m_Machine.ChangeState("Idle");
    }

    protected override void OnExit()
    {
        
    }
}

