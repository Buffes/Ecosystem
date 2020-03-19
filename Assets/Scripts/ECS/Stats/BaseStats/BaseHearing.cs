using Unity.Entities;

namespace Ecosystem.ECS.Stats
{
    [GenerateAuthoringComponent]
    public struct BaseHearing : IComponentData
    {
        public float Range;
    }
}
