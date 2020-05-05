using Ecosystem.ECS.Animal;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.ECS.Grid.Buckets
{
    public struct BucketAnimalData : IBufferElementData, IBucketEntityData
    {
        public Entity Entity;
        public float3 Position;
        public AnimalTypeData AnimalTypeData;
        public Quaternion Rotation;

        public float3 GetPosition() => Position;
    }
}
