using Ecosystem.ECS.Animal;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Grid.Buckets
{
    public struct BucketAnimalData : IBufferElementData, IBucketEntityData
    {
        public Entity Entity;
        public float3 Position;
        public AnimalTypeData AnimalTypeData;

        public float3 GetPosition() => Position;
    }
}
