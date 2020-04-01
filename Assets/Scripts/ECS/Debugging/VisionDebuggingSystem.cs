using Ecosystem.ECS.Debugging.Graphics;
using Ecosystem.ECS.Debugging.Selection;
using Ecosystem.ECS.Targeting.Sensors;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Debugging {
    public class VisionDebuggingSystem : SystemBase {

        private static readonly string VISION_DEBUG_ENTITY_NAME = "VisionRangeIndicator";

        public bool Show { get; set; }
        public Material Material { get; set; }

        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private Entity visionDebugEntityPrefab;

        protected override void OnCreate() {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnStartRunning() {
            CreateVisionDebugPrefab();
        }

        protected override void OnUpdate() {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
            var prefab = this.visionDebugEntityPrefab;

            if (Show) {
                // Add display on selection
                Entities
                    .WithAll<Selected, Vision>()
                    .WithNone<DisplayingVisionDebug>()
                    .ForEach((Entity entity, int entityInQueryIndex) => {
                        commandBuffer.AddComponent<DisplayingVisionDebug>(entityInQueryIndex, entity);
                    }).ScheduleParallel();

                // Remove display on deselection
                Entities
                    .WithAll<DisplayingVisionDebug>()
                    .WithNone<Selected>()
                    .ForEach((Entity entity, int entityInQueryIndex) => {
                        commandBuffer.RemoveComponent<DisplayingVisionDebug>(entityInQueryIndex, entity);
                    }).ScheduleParallel();
            } else {
                // Remove all display
                Entities.WithAll<DisplayingVisionDebug>().ForEach((Entity entity, int entityInQueryIndex) => {
                    commandBuffer.RemoveComponent<DisplayingVisionDebug>(entityInQueryIndex, entity);
                }).ScheduleParallel();
            }

            // Display vision debug
            Entities
                .WithAll<DisplayingVisionDebug>()
                .WithNone<VisionDebugChild>()
                .ForEach((Entity entity, int entityInQueryIndex, in Vision vision) => {
                    var instance = commandBuffer.Instantiate(entityInQueryIndex, prefab);
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new Parent { Value = entity });
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new VisionRangeTarget { Target = entity });
                    commandBuffer.SetComponent(entityInQueryIndex, instance, new CircleMesh { Radius = vision.Range });

                    commandBuffer.AddComponent(entityInQueryIndex, entity, new VisionDebugChild { Entity = instance });
                }).ScheduleParallel();

            // Remove vision debug display
            Entities
                .WithNone<DisplayingVisionDebug>()
                .ForEach((Entity entity, int entityInQueryIndex, in VisionDebugChild visionDebugChild) => {
                    commandBuffer.DestroyEntity(entityInQueryIndex, visionDebugChild.Entity);
                    commandBuffer.RemoveComponent<VisionDebugChild>(entityInQueryIndex, entity);
                }).ScheduleParallel();


            var visionComponents = GetComponentDataFromEntity<Vision>(true);

            // Update radius if vision range changes
            Entities
                .WithReadOnly(visionComponents)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref CircleMesh circleMesh,
                in VisionRangeTarget visionRangeDisplay) => {
                    Entity target = visionRangeDisplay.Target;

                    if (!visionComponents.Exists(target)) {
                        commandBuffer.RemoveComponent<DisplayingVisionDebug>(entityInQueryIndex, target);
                        return;
                    }

                    circleMesh.Radius = visionComponents[target].Range;
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        private void CreateVisionDebugPrefab() {
            if (visionDebugEntityPrefab != Entity.Null) return;

            this.visionDebugEntityPrefab = EntityManager.CreateEntity(
                typeof(Prefab),
                typeof(LocalToWorld),
                typeof(Translation),
                typeof(Parent),
                typeof(LocalToParent),
                typeof(VisionRangeTarget),
                typeof(CircleMesh),
                typeof(ShapeStyle));
            EntityManager.SetSharedComponentData(visionDebugEntityPrefab, new ShapeStyle { Material = Material });
            EntityManager.SetName(visionDebugEntityPrefab, VISION_DEBUG_ENTITY_NAME);
        }

        private struct DisplayingVisionDebug : ISystemStateComponentData {
        }

        private struct VisionDebugChild : ISystemStateComponentData {
            public Entity Entity; // The displaying child entity
        }

        private struct VisionRangeTarget : IComponentData {
            public Entity Target; // The entity with the vision component
        }

    }
}


