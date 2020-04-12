using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Grid.Buckets;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.Sensing
{
    [UpdateInGroup(typeof(SensingAddSystemGroup))]
    public class VisionSystem : SystemBase
    {
        private EntityBucketSystem entityBucketSystem;

        protected override void OnCreate()
        {
            entityBucketSystem = World.GetOrCreateSystem<EntityBucketSystem>();
        }

        protected override void OnUpdate()
        {
            var grid = entityBucketSystem.Grid;
            var animalBuckets = entityBucketSystem.AnimalBuckets;
            var foodBuckets = entityBucketSystem.FoodBuckets;

            Dependency = JobHandle.CombineDependencies(Dependency, entityBucketSystem.Dependency);

            Entities
                .WithReadOnly(animalBuckets)
                .ForEach((
                    ref DynamicBuffer<BucketAnimalData> sensedAnimals,
                    in Vision vision,
                    in Translation position,
                    in Rotation rotation) =>
                {
                    AddWithinVisionRange(ref sensedAnimals, animalBuckets, grid, position.Value,
                        rotation.Value, vision);
                }).ScheduleParallel();


            Entities
                .WithReadOnly(foodBuckets)
                .ForEach((
                    ref DynamicBuffer<BucketFoodData> sensedFood,
                    in Vision vision,
                    in Translation position,
                    in Rotation rotation) =>
                {
                    AddWithinVisionRange(ref sensedFood, foodBuckets, grid, position.Value,
                        rotation.Value, vision);
                }).ScheduleParallel();
        }

        public static void AddWithinVisionRange<T>(ref DynamicBuffer<T> buffer,
            NativeMultiHashMap<int, T> buckets, GridData grid, float3 position, quaternion rotation,
            Vision vision) where T : struct, IBucketEntityData
        {
            var bucketKeys = EntityBucketSystem.GetNearbyCellsKeys(grid, position, vision.Range, Allocator.Temp);

            for (int i = 0; i < bucketKeys.Length; i++)
            {
                if (buckets.TryGetFirstValue(bucketKeys[i], out var data, out var it))
                {
                    do
                    {
                        if (!Utilities.IntersectsVision(data.GetPosition(), position, rotation, vision))
                            continue;

                        buffer.Add(data);
                    }
                    while (buckets.TryGetNextValue(out data, ref it));
                }
            }

            bucketKeys.Dispose();
        }
    }
}
