using System;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting.Targets
{
    [Serializable]
    [InternalBufferCapacity(20)]
    [GenerateAuthoringComponent]
    public struct DetectedEntityElement : IBufferElementData
    {
        public Entity entity;
    }
}
