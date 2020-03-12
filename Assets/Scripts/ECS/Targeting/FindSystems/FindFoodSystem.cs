using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Looks for nearby food and stores info about the closest food that was found.
    /// </summary>
    public class FindFoodSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<FoodTypeData>());
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var entities = query.ToEntityArray(Allocator.TempJob);
            var positions = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var foodTypes = query.ToComponentDataArray<FoodTypeData>(Allocator.TempJob);

            Entities
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .WithReadOnly(foodTypes)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref LookingForFood lookingForFood,
                in Translation position,
                in Hearing hearing,
                in DynamicBuffer<FoodTypesElement> foodTypeBuffer) =>
            {

                int closestFoodIndex = -1;
                float closestFoodDistance = 0f;

                // Check all food that we can sense
                for (int i = 0; i < entities.Length; i++)
                {
                    FoodTypeData targetFoodType = foodTypes[i];
                    float3 targetPosition = positions[i].Value;
                    float targetDistance = math.distance(targetPosition, position.Value);

                    if (targetDistance > hearing.Range) continue; // Out of range
                    if (!IsWantedFood(targetFoodType, foodTypeBuffer)) continue; // Not wanted food type
                    if (closestFoodIndex != -1 && targetDistance >= closestFoodDistance) continue; // Not the closest

                    closestFoodIndex = i;
                    closestFoodDistance = targetDistance;
                }

                // Set result
                if (closestFoodIndex != -1)
                {
                    lookingForFood.HasFound = true;
                    lookingForFood.Entity = entities[closestFoodIndex];
                    lookingForFood.Position = positions[closestFoodIndex].Value;
                }
                else
                {
                    lookingForFood.HasFound = false;
                }

            }).ScheduleParallel();

            entities.Dispose(Dependency);
            positions.Dispose(Dependency);
            foodTypes.Dispose(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
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
