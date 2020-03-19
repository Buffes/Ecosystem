using Unity.Entities;

namespace Ecosystem.ECS.Animal.Stats
{
    [GenerateAuthoringComponent]
    public struct BaseVision : IComponentData
    {
        public float Range;
    }
}
