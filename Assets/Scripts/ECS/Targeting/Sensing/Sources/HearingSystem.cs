using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Grid.Buckets;
using Ecosystem.ECS.Movement.Pathfinding;
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
        private  WorldGridSystem worldGridSystem;

        protected override void OnCreate()
        {
            entityBucketSystem = World.GetOrCreateSystem<EntityBucketSystem>();
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate()
        {
            Dependency = JobHandle.CombineDependencies(Dependency, entityBucketSystem.Dependency);

            var worldGrid = worldGridSystem.Grid;
            var grid = entityBucketSystem.Grid;
            var animalBuckets = entityBucketSystem.AnimalBuckets;
            var foodBuckets = entityBucketSystem.FoodBuckets;

            Entities
                .WithReadOnly(animalBuckets)
                .ForEach((
                    ref DynamicBuffer<BucketAnimalData> sensedAnimals,
                    in DynamicBuffer<UnreachablePosition> unreachablePositions,
                    in Hearing hearing,
                    in Translation position) =>
                {
                    AddWithinHearingRange(ref sensedAnimals, unreachablePositions, worldGrid, animalBuckets, grid, position.Value, hearing);
                }).ScheduleParallel();

            Entities
                .WithReadOnly(foodBuckets)
                .ForEach((
                    ref DynamicBuffer<BucketFoodData> sensedFood,
                    in DynamicBuffer<UnreachablePosition> unreachablePositions,
                    in Hearing hearing,
                    in Translation position) =>
                {
                    AddWithinHearingRange(ref sensedFood, unreachablePositions, worldGrid, foodBuckets, grid, position.Value, hearing);
                }).ScheduleParallel();
        }

        public static void AddWithinHearingRange<T>(ref DynamicBuffer<T> buffer, DynamicBuffer<UnreachablePosition> unreachablePositions, GridData worldGrid,
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
                        if (Utilities.IsUnreachable(unreachablePositions, data.GetPosition(), worldGrid)) continue;

                        buffer.Add(data);
                    }
                    while (buckets.TryGetNextValue(out data, ref it));
                }
            }

            bucketKeys.Dispose();
        }
    }
}
