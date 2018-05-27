using System;

namespace Pld
{
    public interface IPLDState
    {
        void Enter(IPLDStateMachine machine);
        void Exit();
    }
}
