using Unity.Entities;

namespace Ecosystem.ECS.Stats
{
    [GenerateAuthoringComponent]
    public struct BaseVision : IComponentData
    {
        public float Range;
    }
}
