using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid.Buckets;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Sensing;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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

                    if (!IsPrey(animalType, targetPreyTypes)) continue; // Not prey to the target
                    if (closestPredatorIndex != -1 && targetDistance >= closestPredatorDistance) continue; // Not the closest
                    if (Utilities.IsUnreachable(unreachablePositions, targetPosition)) continue;

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
