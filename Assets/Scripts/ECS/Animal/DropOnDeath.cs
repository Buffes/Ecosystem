﻿using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Spawns an entity on death.
    /// </summary>
    [Serializable]
    public struct DropOnDeath : IComponentData
    {
        public Entity Prefab;
    }
}
