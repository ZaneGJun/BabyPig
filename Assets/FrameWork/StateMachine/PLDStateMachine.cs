using System;
using System.Collections.Generic;

namespace Pld
{
    public class PLDStateMachine<T> : IPLDStateMachine
    {
        public PLDStateMachine(T target)
        {
            m_Target = target;
        }

        protected T m_Target;
        public T Target
        {
            get { return m_Target; }
        }

        protected PLDState<T> m_CurState;
        protected Dictionary<string, PLDState<T>> m_StateMap = new Dictionary<string, PLDState<T>>();
        protected Dictionary<string, IPLDCondition> m_ConditionMap = new Dictionary<string, IPLDCondition>();

        public virtual void ChangeState(string stateName, string conditionName = "")
        {
            if(m_CurState == null || m_CurState.Name != stateName)
            {
                bool isConditionPass = true;
                if(conditionName != "" && m_ConditionMap.ContainsKey(conditionName))
                {
                    //条件判断
                    isConditionPass = m_ConditionMap[conditionName].IsPass(this);
                }

                if(m_StateMap.ContainsKey(stateName) && isConditionPass)
                {
                    PLDState<T> toState = m_StateMap[stateName];

                    //退出旧的状态
                    if (m_CurState != null)
                    {
                        m_CurState.Exit();
                    }

                    //进入新的状态
                    toState.Enter(this);

                    //替换
                    m_CurState = toState;
                }
            }
        }

        public virtual void Update()
        {

        }

        public void AddState(IPLDState state)
        {
            if (!m_StateMap.ContainsKey(((PLDState<T>)state).Name))
            {
                m_StateMap.Add(((PLDState<T>)state).Name, (PLDState<T>)state);
            }
            else
            {
                //如果已经存在，则替换
                m_StateMap[((PLDState<T>)state).Name] = (PLDState<T>)state;
            }
        }

        public void RemoveState(string stateName)
        {
            m_StateMap.Remove(stateName);
        }

        public void Flush()
        {
            m_StateMap.Clear();
            m_ConditionMap.Clear();
        }

        public void AddCondition(IPLDCondition condition)
        {
            PLDCondition<T> cond = (PLDCondition<T>)condition;
            if(!m_ConditionMap.ContainsKey(cond.Name))
            {
                m_ConditionMap.Add(cond.Name, cond);
            }
            else
            {
                m_ConditionMap[cond.Name] = cond;
            }
        }

        public void RemoveCondition(string conditionName)
        {
            m_ConditionMap.Remove(conditionName);
        }
    }
}
