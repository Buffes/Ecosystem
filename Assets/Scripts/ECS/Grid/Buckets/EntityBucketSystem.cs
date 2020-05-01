using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Death;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Grid.Buckets
{
    /// <summary>
    /// Sorts entities into buckets based on position.
    /// <para/>
    /// Buckets are used to optimize search for nearby entities by only looking at entities in nearby
    /// buckets.
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class EntityBucketSystem : SystemBase
    {
        private const float CELL_SIZE = 20f; // Bucket area = CELL_SIZE ^ 2

        public NativeMultiHashMap<int, BucketAnimalData> AnimalBuckets;
        public NativeMultiHashMap<int, BucketFoodData> FoodBuckets;

        public GridData Grid;
        public new JobHandle Dependency => base.Dependency;

        private EntityQuery animalQuery;
        private EntityQuery foodQuery;

        /// <summary>
        /// Returns keys to all nearby buckets.
        /// <para/>
        /// Guaranteed to cover all buckets within the specified radius of the specified world position.
        /// </summary>
        public static NativeArray<int> GetNearbyCellsKeys(GridData grid, float3 worldPosition, float radius, Allocator allocator)
        {
            float3 startPos = worldPosition + new float3(-radius, 0, -radius);
            float3 endPos = worldPosition + new float3(radius, 0, radius);
            int2 start = grid.GetGridPosition(startPos);
            int2 end = grid.GetGridPosition(endPos);
            int length = (1 + end.x - start.x) * (1 + end.y - start.y);

            NativeArray<int> nearbyCellKeys = new NativeArray<int>(length, allocator);

            int index = 0;
            for (int i = start.x; i <= end.x; i++)
            {
                for (int j = start.y; j <= end.y; j++)
                {
                    nearbyCellKeys[index] = grid.GetCellKey(new int2(i, j));
                    index++;
                }
            }

            return nearbyCellKeys;
        }

        protected override void OnCreate()
        {
            Grid = new GridData(CELL_SIZE);
            AnimalBuckets = new NativeMultiHashMap<int, BucketAnimalData>(0, Allocator.Persistent);
            FoodBuckets = new NativeMultiHashMap<int, BucketFoodData>(0, Allocator.Persistent);

            animalQuery = GetEntityQuery(typeof(AnimalTypeData));
            foodQuery = GetEntityQuery(typeof(FoodTypeData));
        }

        protected override void OnDestroy()
        {
            AnimalBuckets.Dispose();
            FoodBuckets.Dispose();
        }

        protected override void OnUpdate()
        {
            GridData grid = Grid;

            var animalBuckets = AnimalBuckets;
            var foodBuckets = FoodBuckets;

            int animalCount = animalQuery.CalculateEntityCount();
            int foodCount = foodQuery.CalculateEntityCount();

            // Clear in a job so that it is included in the dependency job handle
            Job.WithCode(() =>
               {
                   animalBuckets.Clear();
                   foodBuckets.Clear();

                   if (animalCount > animalBuckets.Capacity) animalBuckets.Capacity = animalCount;
                   if (foodCount > foodBuckets.Capacity) foodBuckets.Capacity = foodCount;
               }).Schedule();

            var animalBucketsWriter = AnimalBuckets.AsParallelWriter();
            Entities
                .WithNone<DeathEvent>()
                .ForEach((Entity entity, in Translation position, in AnimalTypeData animalTypeData, in Rotation rotation) =>
                {
                    var data = new BucketAnimalData
                    {
                        Entity = entity,
                        Position = position.Value,
                        AnimalTypeData = animalTypeData,
                        Rotation = rotation.Value
                    };
                    animalBucketsWriter.Add(grid.GetCellKey(position.Value), data);
                }).ScheduleParallel();

            var foodBucketsWriter = FoodBuckets.AsParallelWriter();
            Entities
                .WithNone<DeathEvent>()
                .ForEach((Entity entity, in Translation position, in FoodTypeData foodTypeData) =>
                {
                    var data = new BucketFoodData
                    {
                        Entity = entity,
                        Position = position.Value,
                        FoodTypeData = foodTypeData
                    };
                    foodBucketsWriter.Add(grid.GetCellKey(position.Value), data);
                }).ScheduleParallel();
        }
    }
}
