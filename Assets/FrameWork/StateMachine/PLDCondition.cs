using System;

namespace Pld
{
    public abstract class PLDCondition<T> : IPLDCondition
    {
        //禁用默认构造函数
        private PLDCondition()
        {

        }

        public PLDCondition(string name)
        {
            m_Name = name;
        }

        protected PLDStateMachine<T> m_Machine;
        public PLDStateMachine<T> Machine
        {
            get { return m_Machine; }
        }

        protected string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public virtual bool IsPass(IPLDStateMachine machine)
        {
            if (m_Machine != (PLDStateMachine<T>)machine)
                m_Machine = (PLDStateMachine<T>)machine;

            return true;
        }
    }
}
