using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Targeting.Sensors;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public struct SensedAnimalElement : IBufferElementData
{
    public Entity entity;
    public float3 position;
    public AnimalTypeData animalTypeData;
}

public struct SensedFoodElement : IBufferElementData
{
    public Entity entity;
    public float3 position;
}

public class SensingSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(SensingSystemGroup))]
public class SensingResetSystemGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(SensingSystemGroup))]
[UpdateAfter(typeof(SensingResetSystemGroup))]
public class SensingAddSystemGroup : ComponentSystemGroup { }

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

        Entities
            .WithReadOnly(animalBuckets)
            .ForEach((
                ref DynamicBuffer<BucketAnimalData> sensedAnimals,
                in Vision vision,
                in Translation position,
                in Rotation rotation) =>
            {
                //var a = animalBuckets.TryGetFirstValue(0, out _, out _);
                //Debug.Log(animalBuckets.Length);
                Test(ref sensedAnimals, animalBuckets, grid, position.Value, rotation.Value, vision);
            }).ScheduleParallel();
        

        /*Entities
            .WithReadOnly(foodBuckets)
            .ForEach((
                ref DynamicBuffer<BucketFoodData> sensedFood,
                in Vision vision,
                in Translation position,
                in Rotation rotation) =>
            {
                Test2(ref sensedFood, foodBuckets, grid, position.Value, rotation.Value, vision);
            }).ScheduleParallel();*/

        Entities
            .WithReadOnly(animalBuckets)
            .ForEach((
                ref DynamicBuffer<SensedAnimalElement> sensedAnimals,
                in Vision vision,
                in Translation position,
                in Rotation rotation) =>
            {
                NativeArray<BucketAnimalData> entityData = EntityBucketSystem.GetNearbyEntities(
                    animalBuckets, grid, position.Value, vision.Range, Allocator.Temp);

                for (int i = 0; i < entityData.Length; i++)
                {
                    var data = entityData[i];

                    if (!Utilities.IntersectsVision(data.Position, position.Value, rotation.Value, vision))
                        continue;

                    sensedAnimals.Add(new SensedAnimalElement
                    {
                        entity = data.Entity,
                        position = data.Position,
                        animalTypeData = data.AnimalTypeData
                    });
                }

                entityData.Dispose();
            }).ScheduleParallel();

        /*Entities
            .WithReadOnly(foodBuckets)
            .ForEach((
                ref DynamicBuffer<SensedFoodElement> sensedAnimals,
                in Vision vision,
                in Translation position,
                in Rotation rotation) =>
            {
                NativeArray<BucketFoodData> entityData = EntityBucketSystem.GetNearbyEntities(
                    foodBuckets, grid, position.Value, vision.Range);

                for (int i = 0; i < entityData.Length; i++)
                {
                    var data = entityData[i];

                    if (!Utilities.IntersectsVision(data.Position, position.Value, rotation.Value, vision))
                        continue;

                    sensedAnimals.Add(new SensedFoodElement
                    {
                        entity = data.Entity,
                        position = data.Position
                    });
                }

                entityData.Dispose();
            }).ScheduleParallel();*/
    }

    private static void Test<T>(ref DynamicBuffer<T> buffer, NativeMultiHashMap<int, T> buckets, GridData grid,
            float3 position, quaternion rotation, Vision vision)
        where T : struct, IBucketEntityData
    {
        NativeArray<T> entityData = EntityBucketSystem.GetNearbyEntities(
                    buckets, grid, position, vision.Range, Allocator.Temp);

        for (int i = 0; i < entityData.Length; i++)
        {
            var data = entityData[i];

            if (!Utilities.IntersectsVision(data.GetPosition(), position, rotation, vision))
                continue;

            buffer.Add(data);
        }

        entityData.Dispose();
    }

    public static void Test2<T>(ref DynamicBuffer<T> buffer, NativeMultiHashMap<int, T> buckets, GridData grid,
            float3 position, quaternion rotation, Vision vision)
        where T : struct, IBucketEntityData
    {
        NativeArray<int> bucketKeys = EntityBucketSystem.GetNearbyCellsKeys(grid, position, vision.Range, Allocator.Temp);

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

[UpdateInGroup(typeof(SensingResetSystemGroup))]
public class SensedResetSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref DynamicBuffer<BucketAnimalData> sensedAnimals) =>
        {
            sensedAnimals.Clear();
        }).ScheduleParallel();
    }
}
