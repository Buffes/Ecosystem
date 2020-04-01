using Unity.Entities;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Movement;

namespace Ecosystem.ECS.Animal {
    /// <summary>
    /// System for updating the energy of an animal.
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

            Entities.WithAll<Sprinting>().ForEach((Entity entity,int entityInQueryIndex,
                ref EnergyData energyData, ref ExhaustedData exhaustedData, ref RecoveryData recoveryData) => {
                    
                    energyData.Energy -= deltaTime / 100.0f;
                    if (energyData.Energy <= 0.0f) {
                        energyData.Energy = 0f;
                        exhaustedData.TimeUntilSprintPossible = recoveryData.RecoveryLimit;
                        recoveryData.RecoveryTime = 0f;
                        commandBuffer.RemoveComponent<Sprinting>(entityInQueryIndex,entity);
                    }
                }).ScheduleParallel();

            Entities.WithNone<Sprinting>().ForEach((Entity entity,int entityInQueryIndex,
                ref EnergyData energyData,ref RecoveryData recoveryData, ref ExhaustedData exhaustedData) => {
                    
                    if (recoveryData.RecoveryTime < recoveryData.RecoveryLimit) {
                        recoveryData.RecoveryTime += deltaTime/100.0f;
                        exhaustedData.TimeUntilSprintPossible = recoveryData.RecoveryLimit-recoveryData.RecoveryTime;
                    }
                    else if (exhaustedData.TimeUntilSprintPossible != 0f) {
                        exhaustedData.TimeUntilSprintPossible = 0f;
                    }

                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
