using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Moves entities along their current path.
    /// </summary>
    public class PathFollowingSystem : SystemBase
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

            Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref MovementInput movementInput,
                ref DynamicBuffer<PathElement> pathBuffer,
                in Translation translation) =>
            {
                if (pathBuffer.Length == 0) return;

                for (int i = pathBuffer.Length - 1; i >= 0; i--)
                {
                    float3 checkpoint = pathBuffer[i].Checkpoint;
                    float3 difference = checkpoint - translation.Value;
                    difference.y = 0;
                    if (math.length(difference) > 0.1f)
                    {
                        float3 oldDir = movementInput.Direction;
                        float3 newDir = math.normalize(difference);

                        if (math.length(newDir + oldDir) > 0.1f)
                        {
                            movementInput.Direction = newDir;
                            return;
                        }
                    }
                    pathBuffer.RemoveAt(i);
                    if (pathBuffer.Length == 0)
                    {
                        commandBuffer.AddComponent<FinishedPathTag>(entityInQueryIndex, entity);
                    }
                    else
                    {
                        commandBuffer.RemoveComponent<FinishedPathTag>(entityInQueryIndex, entity);
                    }
                }

                movementInput.Direction = float3.zero;

            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
