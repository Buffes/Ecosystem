using Ecosystem.ECS.Death;
using Unity.Entities;

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

            float deltaTime = Time.DeltaTime/60;

            Entities
                .WithNone<DeathEvent>()
                .ForEach((int nativeThreadIndex, Entity entity, int entityInQueryIndex,
                ref AgeData age,
                in AgeOfDeathData ageOfDeath) =>
                {

                    // Store age in seconds.
                    age.Age += deltaTime;


                    if (age.Age >= ageOfDeath.Value)
                    {
                        // Death by old age.
                        commandBuffer.AddComponent<DeathEvent>(entityInQueryIndex, entity,new DeathEvent(DeathCause.Age));
                    }
                
                }).ScheduleParallel();

            Entities
                .WithNone<Infertile>()
                .ForEach((Entity entity,int entityInQueryIndex,
                in AgeData age,
                in InfertilityData ageOfInfertilty) =>
                {
                    if (age.Age >= ageOfInfertilty.InfertilityAge)
                    {
                        commandBuffer.AddComponent<Infertile>(entityInQueryIndex,entity);
                    }
                }).ScheduleParallel();


            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}