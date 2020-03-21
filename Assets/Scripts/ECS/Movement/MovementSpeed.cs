using System;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Defines how fast the entity moves.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }
}
