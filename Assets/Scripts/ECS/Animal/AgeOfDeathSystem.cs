using Ecosystem.ECS.Death;
using Ecosystem.ECS.Random;
using Unity.Entities;
using Unity.Mathematics;


namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Determines the age of death for an animal based on its expected lifespan. Based on a normal distribution.
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
                
                // Calculate the age of death by old age using inverse transform sampling on normal distribution.
                float u = random.NextFloat();
                float gompertzTransform = math.log(1f - 10f * math.log(1f-u));
                float exactDeathAge = lifespan.Value + 10f * gompertzTransform - 5f; //TODO: CHange to normal dist
                commandBuffer.AddComponent<AgeOfDeathData>(entityInQueryIndex, entity, new AgeOfDeathData { Value = exactDeathAge});
                
                randomArray[nativeThreadIndex] = random; // Necessary to update the generator.
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}