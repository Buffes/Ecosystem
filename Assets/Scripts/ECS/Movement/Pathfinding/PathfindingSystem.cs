using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Finds a path to the target position from a move command. Currently just sets the path straight
    /// toward the target. Needs a reference to the grid world to actually pathfind.
    /// </summary>
    public class PathfindingSystem : JobComponentSystem
    {
        EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            var commandBuffer = m_EndSimulationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex,
                ref DynamicBuffer<PathElement> pathBuffer,
                in MoveCommand moveCommand,
                in Translation translation) =>
            {

                float3 target = moveCommand.Target;
                float reach = moveCommand.Reach;

                // Consume the command
                commandBuffer.RemoveComponent<MoveCommand>(entityInQueryIndex, entity);
                // Clear any existing path
                pathBuffer.Clear();

                // Offset the target by the reach
                target = target - reach * math.normalize(target - translation.Value);

                // Add path checkpoint
                pathBuffer.Add(new PathElement { Checkpoint = target });

            }).Schedule(inputDependencies);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(jobHandle);
            return jobHandle;
        }

        private static int2 GetGridCoords(float3 worldPosition)
        {
            int x = (int)worldPosition.x - (worldPosition.x < 0 ? 1 : 0);
            int z = (int)worldPosition.z - (worldPosition.z < 0 ? 1 : 0);
            return new int2(x, z);
        }

        private static float3 GetWorldPosition(int2 gridCoords)
        {
            float x = gridCoords.x + 0.5f;
            float z = gridCoords.y + 0.5f;
            return new float3(x, 0f, z);
        }
    }
}
