using Ecosystem.ECS.Animal;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Grid
{
    /// <summary>
    /// Sorts entities into buckets based on position.
    /// <para/>
    /// Buckets are used to optimize search for nearby entities by only looking at entities in nearby
    /// buckets.
    /// </summary>
    public class EntityBucketSystem : SystemBase
    {
        private const int CELL_SIZE = 20; // Bucket area = CELL_SIZE ^ 2

        public NativeMultiHashMap<int, Entity> AnimalBuckets;
        public NativeMultiHashMap<int, Entity> FoodBuckets;

        private GridData grid;

        private EntityQuery animalQuery;
        private EntityQuery foodQuery;

        /// <summary>
        /// Returns keys to all nearby buckets.
        /// <para/>
        /// Guaranteed to cover all buckets within the specified radius of the specified world position.
        /// </summary>
        public int[] GetNearbyCellsKeys(float3 worldPosition, float radius)
        {
            float3 startPos = worldPosition + new float3(-radius, 0, -radius);
            float3 endPos = worldPosition + new float3(radius, 0, radius);
            int2 start = grid.GetGridPosition(startPos);
            int2 end = grid.GetGridPosition(endPos);

            int[] nearbyCellKeys = new int[(1 + end.x - start.x) * (1 + end.y - start.y)];

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
            grid = new GridData(CELL_SIZE);
            AnimalBuckets = new NativeMultiHashMap<int, Entity>(0, Allocator.Persistent);
            FoodBuckets = new NativeMultiHashMap<int, Entity>(0, Allocator.Persistent);

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
            GridData grid = this.grid;

            ResetBuckets();

            var animalBucketsWriter = AnimalBuckets.AsParallelWriter();
            Entities
                .WithAll<AnimalTypeData>()
                .ForEach((Entity entity, in Translation position) =>
                {
                    animalBucketsWriter.Add(grid.GetCellKey(position.Value), entity);
                }).ScheduleParallel();

            var foodBucketsWriter = FoodBuckets.AsParallelWriter();
            Entities
                .WithAll<FoodTypeData>()
                .ForEach((Entity entity, in Translation position) =>
                {
                    foodBucketsWriter.Add(grid.GetCellKey(position.Value), entity);
                }).ScheduleParallel();
        }

        private void ResetBuckets()
        {
            AnimalBuckets.Clear();
            FoodBuckets.Clear();

            int animalCount = animalQuery.CalculateEntityCount();
            int foodCount = foodQuery.CalculateEntityCount();

            if (animalCount > AnimalBuckets.Capacity) AnimalBuckets.Capacity = animalCount;
            if (foodCount > FoodBuckets.Capacity) FoodBuckets.Capacity = foodCount;
        }
    }
}
