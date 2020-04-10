﻿using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    public class HearingSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        private EntityQuery query;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

            query = GetEntityQuery(
                ComponentType.ReadOnly<Translation>()
                );

        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var entities = query.ToEntityArray(Allocator.TempJob);
            var positions = query.ToComponentDataArray<Translation>(Allocator.TempJob);
            var detectedEntitiesBuffers = GetBufferFromEntity<DetectedEntityElement>();

            Entities
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .WithAll<DynamicBuffer<DetectedEntityElement>>()
                .ForEach((Entity entity, int entityInQueryIndex,
                in Translation position,
                in Hearing hearing) =>
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        DynamicBuffer<DetectedEntityElement> detecteds = detectedEntitiesBuffers[entities[i]];
                        float3 targetPosition = positions[i].Value;
                        float targetDistance = math.distance(targetPosition, position.Value);

                        if (targetDistance > hearing.Range)
                        {
                            continue; // Out of hearing range  
                        }
                        detecteds.Add(new DetectedEntityElement { Entity = entities[i] });

                    }
                }).ScheduleParallel();

            entities.Dispose(Dependency);
            positions.Dispose(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
