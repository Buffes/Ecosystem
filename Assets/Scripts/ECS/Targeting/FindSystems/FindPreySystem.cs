﻿using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid.Buckets;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Looks for nearby prey and stores info about the closest prey that was found.
    /// </summary>
    public class FindPreySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, int entityInQueryIndex,
                ref LookingForPrey lookingForPrey,
                in Translation position,
                in DynamicBuffer<PreyTypesElement> preyTypeBuffer,
                in DynamicBuffer<BucketAnimalData> sensedAnimals,
                in DynamicBuffer<UnreachablePosition> unreachablePositions) =>
            {

                int closestPreyIndex = -1;
                float closestPreyDistance = 0f;

                // Check all animals that we can sense
                for (int i = 0; i < sensedAnimals.Length; i++)
                {
                    var sensedAnimalInfo = sensedAnimals[i];

                    AnimalTypeData targetAnimalType = sensedAnimalInfo.AnimalTypeData;
                    float3 targetPosition = sensedAnimalInfo.Position;
                    float targetDistance = math.distance(targetPosition, position.Value);

                    if (!IsPrey(targetAnimalType, preyTypeBuffer)) continue; // Not prey
                    if (closestPreyIndex != -1 && targetDistance >= closestPreyDistance) continue; // Not the closest
                    if (Utilities.IsUnreachable(unreachablePositions, targetPosition)) continue;

                    closestPreyIndex = i;
                    closestPreyDistance = targetDistance;
                }

                // Set result
                if (closestPreyIndex != -1)
                {
                    lookingForPrey.HasFound = true;
                    lookingForPrey.Entity = sensedAnimals[closestPreyIndex].Entity;
                    lookingForPrey.Position = sensedAnimals[closestPreyIndex].Position;
                }
                else
                {
                    lookingForPrey.HasFound = false;
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
