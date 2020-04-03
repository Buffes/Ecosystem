using System;
using Unity.Entities;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Spawns an entity on death.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct DropOnDeath : IComponentData
    {
        public Entity Prefab;
    }
}
