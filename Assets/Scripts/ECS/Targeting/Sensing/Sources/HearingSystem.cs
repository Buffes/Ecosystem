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
    public class HearingSystem : SystemBase
    {
        private EntityBucketSystem entityBucketSystem;

        protected override void OnCreate()
        {
            entityBucketSystem = World.GetOrCreateSystem<EntityBucketSystem>();
        }

        protected override void OnUpdate()
        {
            Dependency = JobHandle.CombineDependencies(Dependency, entityBucketSystem.Dependency);

            var grid = entityBucketSystem.Grid;
            var animalBuckets = entityBucketSystem.AnimalBuckets;
            var foodBuckets = entityBucketSystem.FoodBuckets;

            Entities
                .WithReadOnly(animalBuckets)
                .ForEach((
                    ref DynamicBuffer<BucketAnimalData> sensedAnimals,
                    in Hearing hearing,
                    in Translation position) =>
                {
                    AddWithinHearingRange(ref sensedAnimals, animalBuckets, grid, position.Value, hearing);
                }).ScheduleParallel();

            Entities
                .WithReadOnly(foodBuckets)
                .ForEach((
                    ref DynamicBuffer<BucketFoodData> sensedFood,
                    in Hearing hearing,
                    in Translation position) =>
                {
                    AddWithinHearingRange(ref sensedFood, foodBuckets, grid, position.Value, hearing);
                }).ScheduleParallel();
        }

        public static void AddWithinHearingRange<T>(ref DynamicBuffer<T> buffer,
            NativeMultiHashMap<int, T> buckets, GridData grid, float3 position, Hearing hearing)
            where T : struct, IBucketEntityData
        {
            var bucketKeys = EntityBucketSystem.GetNearbyCellsKeys(grid, position, hearing.Range, Allocator.Temp);

            for (int i = 0; i < bucketKeys.Length; i++)
            {
                if (buckets.TryGetFirstValue(bucketKeys[i], out var data, out var it))
                {
                    do
                    {
                        if (math.distance(position, data.GetPosition()) > hearing.Range) continue;

                        buffer.Add(data);
                    }
                    while (buckets.TryGetNextValue(out data, ref it));
                }
            }

            bucketKeys.Dispose();
        }
    }
}
