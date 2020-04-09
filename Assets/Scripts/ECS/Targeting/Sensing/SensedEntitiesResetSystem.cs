using Ecosystem.ECS.Grid.Buckets;
using Unity.Entities;
using Unity.Jobs;

namespace Ecosystem.ECS.Targeting.Sensing
{
    [UpdateInGroup(typeof(SensingResetSystemGroup))]
    public class SensedEntitiesResetSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref DynamicBuffer<BucketAnimalData> sensedAnimals) =>
            {
                sensedAnimals.Clear();
            }).ScheduleParallel();

            Entities.ForEach((ref DynamicBuffer<BucketFoodData> sensedFood) =>
            {
                sensedFood.Clear();
            }).ScheduleParallel();
        }
    }
}
