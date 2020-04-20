using Ecosystem.ECS.Animal;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

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
                .WithoutBurst()
                .WithNone<Adult>()
                .ForEach((Entity entity, int entityInQueryIndex,
                ref Scale scale,
                in AgeData age,
                in LifespanData lifespan) =>
                {
                    float ageOfMaturity = lifespan.Value * 0.15f; // TODO: Maybe make multiplier dependent on a component. 
                    Debug.Log("age: " + age.Age);
                    Debug.Log("age of maturity: " + ageOfMaturity);
                    if (age.Age < ageOfMaturity)
                    {
                        scale.Value = 0.5f;
                        Debug.Log("Setting scale to .5");
                    }
                    else
                    {
                        //commandBuffer.AddComponent<Adult>(entityInQueryIndex, entity);
                    }
                }).Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}