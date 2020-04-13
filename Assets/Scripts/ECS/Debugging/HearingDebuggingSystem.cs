using Ecosystem.ECS.Debugging.Graphics;
using Ecosystem.ECS.Debugging.Selection;
using Ecosystem.ECS.Targeting.Sensing;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Debugging
{
    /// <summary>
    /// Adds debugging visuals for hearing range.
    /// </summary>
    public class HearingDebuggingSystem : SystemBase
    {
        private static readonly string HEARING_DEBUG_ENTITY_NAME = "HearingRangeIndicator";

        public bool Show { get; set; }
        public Material Material { get; set; }

        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private Entity hearingDebugEntityPrefab;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnStartRunning()
        {
            CreateHearingDebugPrefab();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
            var prefab = this.hearingDebugEntityPrefab;

            if (Show)
            {
                // Add display on selection
                Entities
                    .WithAll<Selected, Hearing>()
                    .WithNone<DisplayingHearingDebug>()
                    .ForEach((Entity entity, int entityInQueryIndex) =>
                    {
                        commandBuffer.AddComponent<DisplayingHearingDebug>(entityInQueryIndex, entity);
                    }).ScheduleParallel();

                // Remove display on deselection
                Entities
                    .WithAll<DisplayingHearingDebug>()
                    .WithNone<Selected>()
                    .ForEach((Entity entity, int entityInQueryIndex) =>
                    {
                        commandBuffer.RemoveComponent<DisplayingHearingDebug>(entityInQueryIndex, entity);
                    }).ScheduleParallel();
            }
            else
            {
                // Remove all display
                Entities.WithAll<DisplayingHearingDebug>().ForEach((Entity entity, int entityInQueryIndex) =>
                {
                    commandBuffer.RemoveComponent<DisplayingHearingDebug>(entityInQueryIndex, entity);
                }).ScheduleParallel();
            }

            // Display hearing debug
            Entities
                .WithAll<DisplayingHearingDebug>()
                .WithNone<HearingDebugChild>()
                .ForEach((Entity entity, int entityInQueryIndex, in Hearing hearing) =>
                {
                    var instance = commandBuffer.Instantiate(entityInQueryIndex, prefab);
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new Parent { Value = entity });
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new HearingRangeTarget { Target = entity });
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new CircleMesh { Radius = hearing.Range });

                    commandBuffer.AddComponent(entityInQueryIndex, entity, new HearingDebugChild { Entity = instance });
                }).ScheduleParallel();

            // Remove hearing debug display
            Entities
                .WithNone<DisplayingHearingDebug>()
                .ForEach((Entity entity, int entityInQueryIndex, in HearingDebugChild hearingDebugChild) =>
                {
                    commandBuffer.DestroyEntity(entityInQueryIndex, hearingDebugChild.Entity);
                    commandBuffer.RemoveComponent<HearingDebugChild>(entityInQueryIndex, entity);
                }).ScheduleParallel();


            var hearingComponents = GetComponentDataFromEntity<Hearing>(true);

            // Update radius if hearing range changes
            Entities
                .WithReadOnly(hearingComponents)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref CircleMesh circleMesh,
                in HearingRangeTarget hearingRangeDisplay) =>
                {
                    Entity target = hearingRangeDisplay.Target;

                    if (!hearingComponents.Exists(target))
                    {
                        commandBuffer.RemoveComponent<DisplayingHearingDebug>(entityInQueryIndex, target);
                        return;
                    }

                    circleMesh.Radius = hearingComponents[target].Range;
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        private void CreateHearingDebugPrefab()
        {
            if (hearingDebugEntityPrefab != Entity.Null) return;

            this.hearingDebugEntityPrefab = EntityManager.CreateEntity(
                typeof(Prefab),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Parent),
                typeof(LocalToParent),
                typeof(HearingRangeTarget),
                typeof(CircleMesh),
                typeof(ShapeStyle));
            EntityManager.SetSharedComponentData(hearingDebugEntityPrefab, new ShapeStyle { Material = Material });
#if UNITY_EDITOR
            EntityManager.SetName(hearingDebugEntityPrefab, HEARING_DEBUG_ENTITY_NAME);
#endif
        }

        private struct DisplayingHearingDebug : ISystemStateComponentData
        {
        }

        private struct HearingDebugChild : ISystemStateComponentData
        {
            public Entity Entity; // The displaying child entity
        }

        private struct HearingRangeTarget : IComponentData
        {
            public Entity Target; // The entity with the hearing component
        }
    }
}
