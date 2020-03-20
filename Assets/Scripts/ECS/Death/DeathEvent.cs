﻿using Unity.Entities;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Marks the entity to command other entities to die
    /// </summary>
    public struct DeathEvent : IComponentData
    {
    }
}
