using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{
    public struct Target : IBufferElementData
    {
        public Entity e;
    }
}

