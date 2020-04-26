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
    /// Looks for nearby mates and stores info about the closest mate that was found.
    /// </summary>
    [UpdateInGroup(typeof(FindSystemGroup))]
    public class FindMateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var sexTypes = GetComponentDataFromEntity<SexData>(true);

            Entities
                .WithReadOnly(sexTypes)
                .ForEach((
                ref LookingForMate lookingForMate,
                in Translation position,
                in AnimalTypeData animalType,
                in SexData sexType,
                in DynamicBuffer<BucketAnimalData> sensedAnimals) =>
                {
                    
                    int closestMateIndex = -1;
                    float closestMateDistance = 0f;

                    for (int i = 0; i < sensedAnimals.Length; i++)
                    {
                        var sensedAnimalInfo = sensedAnimals[i];

                        AnimalTypeData targetAnimalType = sensedAnimalInfo.AnimalTypeData;
                        SexData targetSexType = sexTypes[sensedAnimalInfo.Entity];
                        float3 targetPosition = sensedAnimalInfo.Position;
                        float targetDistance = math.distance(targetPosition, position.Value);

                        if (animalType.AnimalTypeId != targetAnimalType.AnimalTypeId) continue; // Not the same type of animal
                        if (closestMateIndex != -1 && targetDistance >= closestMateDistance) continue; // Not the closest
                        if (sexType.Sex == targetSexType.Sex) continue; // Not the opposite sex

                        closestMateIndex = i;
                        closestMateDistance = targetDistance;
                    }

                    // Set result
                    if (closestMateIndex != -1)
                    {
                        lookingForMate.HasFound = true;
                        lookingForMate.Entity = sensedAnimals[closestMateIndex].Entity;
                        lookingForMate.Position = sensedAnimals[closestMateIndex].Position;
                    }
                    else
                    {
                        lookingForMate.HasFound = false;
                    }

                }).ScheduleParallel();
        }
    }
}
