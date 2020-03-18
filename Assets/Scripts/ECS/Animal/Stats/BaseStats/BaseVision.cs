using Unity.Entities;

namespace Ecosystem.ECS.Animal.Stats
{
    public struct BaseVision : IComponentData
    {
        public float Range;
        public float Angle;
    }
}
