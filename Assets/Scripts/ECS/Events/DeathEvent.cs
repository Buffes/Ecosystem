using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace Ecosystem.ECS.Events
{
    /// <summary>
    /// Marks the entity to command other entities to die
    /// </summary>
    public struct DeathEvent : IComponentData
    {
        public Entity Target;
    }
}
