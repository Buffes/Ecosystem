using System;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting.Sensors
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Hearing : IComponentData
    {
        public float Range;
    }
}
