using Unity.Entities;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Movement;

namespace Ecosystem.ECS.Animal {
    /// <summary>
    /// System for increasing the energy of an animal.
    /// </summary>
    public class EnergySystem : SystemBase {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate() {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            float deltaTime = Time.DeltaTime;

            Entities.ForEach((Entity entity,int entityInQueryIndex,
                ref EnergyData energyData) => {
                    if (EntityManager.HasComponent<Sprinting>(entity)) {
                        energyData.Energy -= deltaTime / 100.0f;
                    }
                    
                    if (energyData.Energy <= 0.0f) {
                        commandBuffer.AddComponent<DeathEvent>(entityInQueryIndex,entity);
                    }

                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
