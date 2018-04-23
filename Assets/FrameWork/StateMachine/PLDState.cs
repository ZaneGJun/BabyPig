using System;

namespace Pld
{
    public abstract class PLDState<T> : IPLDState
    {
        //禁用默认构造函数
        private PLDState()
        {

        }

        public PLDState(string name)
        {
            m_Name = name;
        }

        //对应的状态机
        protected PLDStateMachine<T> m_Machine;
        public PLDStateMachine<T> Machine
        {
            get { return m_Machine; }
        }

        //名字
        protected string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public virtual void Enter(IPLDStateMachine machine)
        {
            if (m_Machine != (PLDStateMachine<T>)machine)
                m_Machine = (PLDStateMachine<T>)machine;
        }

        public virtual void Exit()
        {
            
        }
    }
}
