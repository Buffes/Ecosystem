using Unity.Entities;
using Ecosystem.ECS.Death;

namespace Ecosystem.ECS.Animal.Needs {
    /// <summary>
    /// System for clamping down thirst on an animal.
    /// </summary>
    public class MaxThirstSystem : SystemBase {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate() {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate() {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.ForEach((Entity entity,int entityInQueryIndex,MaxThirstData maxThirstData,
                ref ThirstData thirstData) => {

                    if (thirstData.Thirst >= maxThirstData.MaxThirst) {
                        thirstData.Thirst = maxThirstData.MaxThirst;
                    }

                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
