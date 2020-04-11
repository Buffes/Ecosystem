using System;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting.Sensing
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Hearing : IComponentData
    {
        public float Range;
    }
}
