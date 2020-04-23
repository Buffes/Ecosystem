using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// System for increasing the sexual urges of an animal
    /// </summary>
    public class SexualUrgesSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            float deltaTime = Time.DeltaTime/60f;

            Entities
                .WithNone<Reproduction.Pregnant>()
                .WithAll<Growth.Adult>()
                .ForEach((Entity entity, int entityInQueryIndex,
                ref SexualUrgesData sexualUrgesData) =>
            {
                sexualUrgesData.Urge -= deltaTime;

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
