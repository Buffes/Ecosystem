using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Holds information about parents of non-adult entities.
    /// </summary>
    public struct ParentData : IComponentData
    {
        public Entity Entity;
    }
}