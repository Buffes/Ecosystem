using Unity.Entities;
using System.Threading;
using UnityEngine;

namespace Ecosystem.ECS.Death
{
    /// <summary>
    /// Kills desired entities
    /// </summary>
    public class DeathEventSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        public int hunger;
        public int thirst;
        public int age;
        public int predator;
        public int other;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            hunger = 0;
            thirst = 0;
            age = 0;
            predator = 0;
            other = 0;
        }
        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            Entities.WithoutBurst().ForEach((Entity entity, int entityInQueryIndex, in DeathEvent deathEvent) =>
            {
                switch (deathEvent.Cause)
                {
                    case DeathCause.Hunger: Interlocked.Increment(ref hunger); break;
                    case DeathCause.Thirst: Interlocked.Increment(ref thirst); break;
                    case DeathCause.Age: Interlocked.Increment(ref age); break;
                    case DeathCause.Predators: Interlocked.Increment(ref predator); break;
                    default: Interlocked.Increment(ref other); break;
                }
                commandBuffer.DestroyEntity(entityInQueryIndex,entity);
            }).Run();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
