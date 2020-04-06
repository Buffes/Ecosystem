using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Sensors;
using Ecosystem.ECS.Targeting.Targets;
using Ecosystem.Grid;

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Looks for nearby water and stores info about the closest water that was found.
    /// </summary>
    public class FindWaterSystem : SystemBase
    {
        private WorldGridSystem worldGridSystem;

        protected override void OnCreate()
        {
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate()
        {
            var grid = worldGridSystem.Grid;
            var waterTiles = worldGridSystem.WaterCells;
           
            // Get buffers here since ForEach lambda has max 9 parameters. Should be unnecessary once the Separate concerns in find-systems task is done
            var unreachableBuffers = GetBufferFromEntity<UnreachablePosition>(true);

            Entities
                .WithReadOnly(waterTiles)
                .WithReadOnly(unreachableBuffers)
                .ForEach((Entity entity, int entityInQueryIndex,
                ref LookingForWater lookingForWater,
                in Translation position,
                in Rotation rotation,
                in Hearing hearing,
                in Vision vision) =>
            {

                int closestWaterIndex = -1;
                float closestWaterDistance = 0f;

                for (int i = 0; i < waterTiles.Length; i++)
                {
                    if (!waterTiles[i]) continue;

                    float3 targetPosition = grid.GetWorldPosition(grid.GetGridPositionFromIndex(i));

                    float targetDistance = math.distance(targetPosition, position.Value);

                    if (targetDistance > hearing.Range && !Utilities.IntersectsVision(targetPosition, position.Value, rotation.Value, vision))
                    {
                        continue; // Out of hearing and vision range    
                    } 

                    if (closestWaterIndex != -1 && targetDistance >= closestWaterDistance) continue; // Not the closest
                    if (Utilities.IsUnreachable(unreachableBuffers[entity], targetPosition)) continue;
                    
                    closestWaterIndex = i;
                    closestWaterDistance = targetDistance;
                }

                // Set result
                if (closestWaterIndex != -1)
                {
                    lookingForWater.HasFound = true;
                    lookingForWater.Position = grid.GetWorldPosition(grid.GetGridPositionFromIndex(closestWaterIndex));
                }
                else
                {
                    lookingForWater.HasFound = false;
                }

            }).ScheduleParallel();

        }
    }

}