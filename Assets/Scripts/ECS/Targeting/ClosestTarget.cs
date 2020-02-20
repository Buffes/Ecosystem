using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting
{
    public struct ClosestTarget : IComponentData
    {
        public Entity targetEntity;
    }
}
