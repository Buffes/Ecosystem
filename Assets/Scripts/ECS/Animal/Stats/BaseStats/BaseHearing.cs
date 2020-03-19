using Unity.Entities;

namespace Ecosystem.ECS.Animal.Stats
{
    [GenerateAuthoringComponent]
    public struct BaseHearing : IComponentData
    {
        public float Range;
    }
}
