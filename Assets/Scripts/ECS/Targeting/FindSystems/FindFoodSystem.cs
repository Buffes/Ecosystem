using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid.Buckets;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    /// <summary>
    /// Looks for nearby food and stores info about the closest food that was found.
    /// </summary>
    [UpdateInGroup(typeof(FindSystemGroup))]
    public class FindFoodSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((
                ref LookingForFood lookingForFood,
                in Translation position,
                in DynamicBuffer<FoodTypesElement> foodTypeBuffer,
                in DynamicBuffer<BucketFoodData> sensedFood) =>
                {

                    int closestFoodIndex = -1;
                    float closestFoodDistance = 0f;

                    // Check all food that we can sense
                    for (int i = 0; i < sensedFood.Length; i++)
                    {
                        var sensedFoodInfo = sensedFood[i];

                        FoodTypeData targetFoodType = sensedFoodInfo.FoodTypeData;
                        float3 targetPosition = sensedFoodInfo.Position;
                        float targetDistance = math.distance(targetPosition, position.Value);

                        if (!IsWantedFood(targetFoodType, foodTypeBuffer)) continue; // Not wanted food type
                        if (closestFoodIndex != -1 && targetDistance >= closestFoodDistance) continue; // Not the closest

                        closestFoodIndex = i;
                        closestFoodDistance = targetDistance;
                    }

                    // Set result
                    if (closestFoodIndex != -1)
                    {
                        lookingForFood.HasFound = true;
                        lookingForFood.Entity = sensedFood[closestFoodIndex].Entity;
                        lookingForFood.Position = sensedFood[closestFoodIndex].Position;
                    }
                    else
                    {
                        lookingForFood.HasFound = false;
                    }

                }).ScheduleParallel();
        }

        private static bool IsWantedFood(FoodTypeData foodType, DynamicBuffer<FoodTypesElement> wantedFoodBuffer)
        {
            for (int i = 0; i < wantedFoodBuffer.Length; i++)
            {
                int wantedFoodType = wantedFoodBuffer[i].FoodTypeId;

                if (foodType.FoodTypeId == wantedFoodType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
