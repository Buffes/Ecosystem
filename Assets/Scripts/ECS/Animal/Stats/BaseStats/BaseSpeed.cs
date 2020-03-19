using Unity.Entities;

namespace Ecosystem.ECS.Animal.Stats
{
    [GenerateAuthoringComponent]
    public struct BaseSpeed : IComponentData
    {
        public float Value;
    }
}
