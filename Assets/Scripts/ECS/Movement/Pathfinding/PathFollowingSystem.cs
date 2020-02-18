using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Moves entities along their current path.
    /// </summary>
    public class PathFollowingSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return Entities.ForEach((Entity entity, int entityInQueryIndex,
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
                    if (math.length(difference) > 0.1)
                    {
                        movementInput.Direction = math.normalize(difference);
                        return;
                    }
                    pathBuffer.RemoveAt(i);
                }

                movementInput.Direction = float3.zero;

            }).Schedule(inputDependencies);
        }
    }
}
