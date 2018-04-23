using System;

namespace Pld
{
    public interface IPLDCondition
    {
        bool IsPass(IPLDStateMachine machine);
    }
}
