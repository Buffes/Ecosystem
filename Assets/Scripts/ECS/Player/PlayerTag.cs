using System;
using Unity.Entities;

namespace Ecosystem.ECS.Player
{
    /// <summary>
    /// Marks an entity as player-controlled.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct PlayerTag : IComponentData
    {
    }
}