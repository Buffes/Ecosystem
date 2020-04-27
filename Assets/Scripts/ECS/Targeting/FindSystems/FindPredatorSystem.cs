using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid.Buckets;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Sensing;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    /// <summary>
    /// Looks for nearby predators and stores info about the closest predator that was found.
    /// </summary>
    [UpdateInGroup(typeof(FindSystemGroup))]
    public class FindPredatorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var preyTypeBuffers = GetBufferFromEntity<PreyTypesElement>();
            
            Entities
                .WithReadOnly(preyTypeBuffers)
                .ForEach((
                ref LookingForPredator lookingForPredator,
                in Translation position,
                in AnimalTypeData animalType,
                in DynamicBuffer<BucketAnimalData> sensedAnimals,
                in DynamicBuffer<UnreachablePosition> unreachablePositions) =>
            {

                int closestPredatorIndex = -1;
                float closestPredatorDistance = 0f;

                // Check all food that we can sense
                for (int i = 0; i < sensedAnimals.Length; i++)
                {
                    var sensedAnimalInfo = sensedAnimals[i];

                    DynamicBuffer<PreyTypesElement> targetPreyTypes = preyTypeBuffers[sensedAnimalInfo.Entity];
                    float3 targetPosition = sensedAnimalInfo.Position;
                    float targetDistance = math.distance(targetPosition, position.Value);
                    Quaternion targetRotation = sensedAnimalInfo.Rotation;

                    float3 relativePosition = position.Value - targetPosition;

                    relativePosition = math.normalize(relativePosition);
                    float3 forward = math.normalize(math.forward(targetRotation));
                    float forwardAngle = math.atan2(forward.z,forward.x);

                    float targetAngle = math.atan2(relativePosition.z,relativePosition.x);

                    if (math.abs(targetAngle - forwardAngle) > math.PI / 2) continue; // Target not walking towards prey
                    if (!IsPrey(animalType, targetPreyTypes)) continue; // Not prey to the target
                    if (closestPredatorIndex != -1 && targetDistance >= closestPredatorDistance) continue; // Not the closest


                    closestPredatorIndex = i;
                    closestPredatorDistance = targetDistance;
                }

                // Set result
                if (closestPredatorIndex != -1)
                {
                    lookingForPredator.HasFound = true;
                    lookingForPredator.Entity = sensedAnimals[closestPredatorIndex].Entity;
                    lookingForPredator.Position = sensedAnimals[closestPredatorIndex].Position;
                }
                else
                {
                    lookingForPredator.HasFound = false;
                }

            }).ScheduleParallel();
        }

        private static bool IsPrey(AnimalTypeData animalType, DynamicBuffer<PreyTypesElement> preyBuffer)
        {
            for (int i = 0; i < preyBuffer.Length; i++)
            {
                int prey = preyBuffer[i].AnimalTypeId;

                if (animalType.AnimalTypeId == prey)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
