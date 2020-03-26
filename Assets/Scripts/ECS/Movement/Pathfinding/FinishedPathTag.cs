using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Tag component that is added once the entity has no more path to follow.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct FinishedPathTag : IComponentData
    {
    }
}
