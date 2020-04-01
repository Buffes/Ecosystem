using Ecosystem.StateMachines;
using Unity.Entities;

namespace Ecosystem.ECS.Debugging
{
    /// <summary>
    /// Reference to the state machine controlling this entity.
    /// </summary>
    public class StateMachineRef : IComponentData
    {
        public StateMachine StateMachine;
    }
}
