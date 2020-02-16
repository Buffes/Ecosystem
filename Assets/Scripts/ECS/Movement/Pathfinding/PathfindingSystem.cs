using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    public class PathfindingSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return Entities.ForEach((
                ref MovementInput movementInput) =>
            {

                // Logic goes here

            }).Schedule(inputDependencies);
        }
    }
}
