using System;

namespace Pld
{
    public interface IPLDStateMachine
    {
        void ChangeState(string stateName, string conditionName);
        void AddState(IPLDState state);
        void RemoveState(string stateName);
        void Flush();
    }
}
