using Ecosystem.ECS.Death;
using Ecosystem.ECS.Random;
using Unity.Entities;
using Unity.Mathematics;


namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Increases the age of all animals.
    /// </summary>
    public class AgingSystem : SystemBase
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

            float deltaTime = Time.DeltaTime;
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

            Entities
            .WithNativeDisableParallelForRestriction(randomArray)
            .ForEach((int nativeThreadIndex, Entity entity, int entityInQueryIndex,
            ref AgeData age,
            in LifespanData lifespan) =>
            {
                var random = randomArray[nativeThreadIndex];

                // Store age in seconds.
                age.Age += deltaTime;
                
                // Average of once per second.
                if (random.NextFloat() < 0.0167f)
                {
                    // Death by old age.
                    float deathProbability = math.exp(- math.exp(60f * lifespan.Value - age.Age));
                    if (random.NextFloat() < deathProbability)
                    {
                        commandBuffer.AddComponent<DeathEvent>(entityInQueryIndex, entity);
                    }
                }
                
                randomArray[nativeThreadIndex] = random; // Necessary to update the generator.
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}