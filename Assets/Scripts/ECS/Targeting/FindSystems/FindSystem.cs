using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;

using Ecosystem.Grid;

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
namespace Assets.Scripts.ECS.Targeting.FindSystems
{
    public class FindSystem : SystemBase
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
            
            Entities
                .WithReadOnly(entities)
                .WithReadOnly(positions)
                .ForEach((Entity entity, int entityInQueryIndex, 
                in Translation position, 
                in Rotation rotation, 
                in Hearing hearing, 
                in Vision vision) =>
            {
                DynamicBuffer<DetectedEntityElement> detectedEntities = commandBuffer.AddBuffer<DetectedEntityElement>(entityInQueryIndex,entity);
                for (int i = 0; i < entities.Length; i++)
                {
                    //DynamicBuffer<PreyTypesElement> targetPreyTypes = preyTypeBuffers[entities[i]];
                    float3 targetPosition = positions[i].Value;
                    float targetDistance = math.distance(targetPosition, position.Value);

                    if (targetDistance > hearing.Range && !Utilities.IntersectsVision(targetPosition, position.Value, rotation.Value, vision))
                    {
                        continue; // Out of hearing and vision range    
                    }
                    detectedEntities.Add(new DetectedEntityElement { Entity = entities[i] });

                }
            }).ScheduleParallel();

            entities.Dispose(Dependency);
            positions.Dispose(Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
