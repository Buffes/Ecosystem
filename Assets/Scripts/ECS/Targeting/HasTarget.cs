using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting
{
    public struct HasTarget : IComponentData
    {
        public Entity targetEntity;
    }
}
