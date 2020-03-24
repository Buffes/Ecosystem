using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Reproduction
{
    public class ReproductionEventSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();
            Entities.WithAll<ReproductionEvent>().ForEach((Entity entity, int entityInQueryIndex
                ,ref LookingForMate lookingForMate
                ,in SexData sexData) =>
            {
                Entity target = lookingForMate.Entity;
                //TODO if female, become pregnant
                commandBuffer.RemoveComponent<ReproductionEvent>(target.Index, target);
                commandBuffer.RemoveComponent<ReproductionEvent>(entityInQueryIndex, entity);

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
