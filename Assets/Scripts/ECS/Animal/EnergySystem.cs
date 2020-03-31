using Unity.Entities;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Movement;

namespace Ecosystem.ECS.Animal {
    /// <summary>
    /// System for updating the energy of an animal.
    /// </summary>
    public class EnergySystem : SystemBase {
        private float recoveryTime;
        private float recoveryLimit;
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate() {
            recoveryTime = 0f;
            recoveryLimit = 0.5f;
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            float deltaTime = Time.DeltaTime;

            Entities.ForEach((Entity entity,EntityManager entityManager,int entityInQueryIndex,
                ref EnergyData energyData) => {
                    if (entityManager.HasComponent<Sprinting>(entity)) {
                        energyData.Energy -= deltaTime / 100.0f;
                        if (energyData.Energy <= 0.0f) {
                            commandBuffer.RemoveComponent<Sprinting>(entityInQueryIndex,entity);
                        }
                    }
                    else if (recoveryTime < recoveryLimit) {
                        recoveryTime += deltaTime/100f;
                        if (entityManager.HasComponent<Sprinting>(entity)) {
                            commandBuffer.RemoveComponent<Sprinting>(entityInQueryIndex,entity);
                        }
                    }
                    
                    

                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
