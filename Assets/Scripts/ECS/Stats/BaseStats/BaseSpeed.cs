using Unity.Entities;

namespace Ecosystem.ECS.Stats
{
    [GenerateAuthoringComponent]
    public struct BaseSpeed : IComponentData
    {
        public float Value;
    }
}
