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

            float deltaTime = Time.DeltaTime/60f;

            Entities.WithAll<Sprinting>().ForEach((Entity entity,int entityInQueryIndex,
                ref EnergyData energyData) => {
                    
                    energyData.Energy -= deltaTime;
                    if (energyData.Energy <= 0.0f) {
                        energyData.Energy = 0f;
                        commandBuffer.AddComponent<ExhaustedData>(entityInQueryIndex,entity,new ExhaustedData(1f));
                    }
                }).ScheduleParallel();

            Entities.WithNone<Sprinting>().ForEach((Entity entity,int entityInQueryIndex,
                ref EnergyData energyData) => {

                    energyData.Energy += deltaTime;

                }).ScheduleParallel();

            Entities.ForEach((Entity entity,int entityInQueryIndex,
                ref ExhaustedData exhaustedData) => {

                    exhaustedData.TimeUntilSprintPossible -= deltaTime;
                    if (exhaustedData.TimeUntilSprintPossible <= 0f) {
                        commandBuffer.RemoveComponent<ExhaustedData>(entityInQueryIndex,entity);
                    }
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
