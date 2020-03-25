using Ecosystem.ECS.Death;
using Ecosystem.ECS.Random;
using Unity.Entities;
using Unity.Mathematics;


namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Determines the age of death for an animal based on its average lifespan. Based on a logistic distribution.
    /// </summary>
    public class AgeOfDeathSystem : SystemBase
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
            .WithNone<AgeOfDeathData>()
            .ForEach((int nativeThreadIndex, Entity entity, int entityInQueryIndex,
            in LifespanData lifespan) =>
            {
                var random = randomArray[nativeThreadIndex];
                
                // Calculate the age of death by old age using inverse transform sampling on logistic distribution.
                float u = random.NextFloat(0.00001f, 1.0f);
                float scale = 0.05f * math.sqrt(lifespan.Value); // Smaller std deviation for shorter lifespans 
                float exactDeathAge = lifespan.Value + scale * math.log(u / (1f-u));
                // Clamp to within 3 standard deviations. scale is about 0.5 * std deviation.
                exactDeathAge = math.clamp(exactDeathAge, -6f*scale + lifespan.Value, 6f*scale + lifespan.Value);
                exactDeathAge = math.max(0f, exactDeathAge); // no negative ages
                commandBuffer.AddComponent<AgeOfDeathData>(entityInQueryIndex, entity, new AgeOfDeathData { Value = exactDeathAge});
                
                randomArray[nativeThreadIndex] = random; // Necessary to update the generator.
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}