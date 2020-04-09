﻿using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Sensing;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    /// <summary>
    /// Looks for nearby mates and stores info about the closest mate that was found.
    /// </summary>
    public class FindMateSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Translation>(),
                ComponentType.ReadOnly<AnimalTypeData>(),
                ComponentType.ReadOnly<SexData>()
                );
            
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var entities = query.ToEntityArray(Allocator.TempJob);
            var positions = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var animalTypes = query.ToComponentDataArray<AnimalTypeData>(Allocator.TempJob);
            var sexTypes = query.ToComponentDataArray<SexData>(Allocator.TempJob);
            
            // Get buffers here since ForEach lambda has max 9 parameters. Should be unnecessary once the Separate concerns in find-systems task is done
            var unreachableBuffers = GetBufferFromEntity<UnreachablePosition>(true);

            Entities
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .WithReadOnly(animalTypes)
                .WithReadOnly(sexTypes)
                .WithReadOnly(unreachableBuffers)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref LookingForMate lookingForMate,
                in Translation position,
                in Hearing hearing,
                in AnimalTypeData animalType,
                in SexData sexType) =>
                {
                    
                    int closestMateIndex = -1;
                    float closestMateDistance = 0f;

                    for (int i = 0; i < entities.Length; i++)
                    {
                        AnimalTypeData targetAnimalType = animalTypes[i];
                        SexData targetSexType = sexTypes[i];
                        float3 targetPosition = positions[i].Value;
                        float targetDistance = math.distance(targetPosition, position.Value);

                        if (targetDistance > hearing.Range) continue; // Out of range
                        if (animalType.AnimalTypeId != targetAnimalType.AnimalTypeId) continue; //If not the same type of animal
                        if (closestMateIndex != -1 && targetDistance >= closestMateDistance) continue; // Not the closest
                        if (sexType.Sex != targetSexType.Sex)   continue; // If the same sex
                        if (Utilities.IsUnreachable(unreachableBuffers[entity], targetPosition)) continue;

                        closestMateIndex = i;
                        closestMateDistance = targetDistance;
                    }

                    // Set result
                    if (closestMateIndex != -1)
                    {
                        lookingForMate.HasFound = true;
                        lookingForMate.Entity = entities[closestMateIndex];
                        lookingForMate.Position = positions[closestMateIndex].Value;
                    }
                    else
                    {
                        lookingForMate.HasFound = false;
                    }


                }).ScheduleParallel();

            entities.Dispose(Dependency);
            positions.Dispose(Dependency);
            animalTypes.Dispose(Dependency);
            sexTypes.Dispose(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
