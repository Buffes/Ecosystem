using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
                in Rotation rotation,
                in Hearing hearing,
                in Vision vision,
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
                    
                    if (targetDistance > hearing.Range && !IntersectsVision(targetPosition, position.Value, rotation.Value, vision))
                    {
                        continue; // Out of hearing and vision range    
                    } 
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

        /// <summary>
        /// Checks intersection between a point at targetPosition and a circle sector defined by the vision parameter.
        /// </summary>
        /// <param name="targetPosition"> The position of the target. </param>
        /// <param name="position"> The position value of the entity. </param>
        /// <param name="rotation"> The rotation value of the entity. </param>
        /// <param name="vision"> The field of vision of the entity, defined by a range and an angle. </param>
        /// <returns> True if targetPosition intersects the circle sector defined by vision, otherwise false. </returns>
        private static bool IntersectsVision(float3 targetPosition, float3 position, quaternion rotation, Vision vision)
        {
            float3 relativePosition = targetPosition - position;

            if (math.length(relativePosition) > vision.Range) {
                return false; // Target outside range
            }
            relativePosition = math.normalize(relativePosition);
            float3 forward = math.normalize(math.forward(rotation));
            float forwardAngle = math.atan2(forward.z, forward.x);
            
            float targetAngle = math.atan2(relativePosition.z, relativePosition.x);
            bool intersects = math.abs(targetAngle - forwardAngle) < vision.Angle;
            return intersects;
        }
    }
}
