using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Input data defining what direction the entitiy wants to go and if to sprint or not.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MovementInput : IComponentData
    {
        public float3 Direction;
        public bool Sprint;
    }
}
