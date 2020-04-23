using Ecosystem.ECS.Animal;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Growth
{
    /// <summary>
    /// Gives young animals lower stats and stops them from mating until maturity.
    /// </summary>
    public class MaturingSystem : SystemBase
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

            float deltaTime = Time.DeltaTime/60;
            
            Entities
                .WithNone<Adult>()
                .ForEach((Entity entity, int entityInQueryIndex,
                ref CompositeScale scale,
                ref Translation translation,
                in AgeData age,
                in LifespanData lifespan) =>
                {
                    float ageOfMaturity = lifespan.Value * 0.01f; // TODO: Maybe make multiplier dependent on a component. 
                    if (age.Age < ageOfMaturity)
                    {
                        float small = 0.5f;
                        float ageNorm = age.Age / ageOfMaturity;
                        float blend = small * (1f - ageNorm) + ageNorm;
                        scale.Value = float4x4.Scale(blend);
                    }
                    else
                    {
                        scale.Value = float4x4.Scale(1f);
                        commandBuffer.AddComponent<Adult>(entityInQueryIndex, entity);
                    }
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}