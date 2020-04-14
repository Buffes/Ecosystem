using Ecosystem.ECS.Animal;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Grid.Buckets
{
    public struct BucketFoodData : IBufferElementData, IBucketEntityData
    {
        public Entity Entity;
        public float3 Position;
        public FoodTypeData FoodTypeData;

        public float3 GetPosition() => Position;
    }
}
