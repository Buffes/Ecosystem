using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{   
    public struct SightedBufferElement : IBufferElementData
    {
        public Entity e;
    }
}
