using Ecosystem.ECS.Random;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs.LowLevel.Unsafe;
using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Movement;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    /// <summary>
    /// Looks for a random walkable point somewhere around the animal and stores it for movement.
    /// </summary>
    [UpdateInGroup(typeof(FindSystemGroup))]
    public class FindRandomTargetSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private RandomSystem randomSystem;
        private WorldGridSystem worldGridSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            randomSystem = World.GetOrCreateSystem<RandomSystem>();
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate()
        {
            var blockedCells = worldGridSystem.BlockedCells;
            var waterCells = worldGridSystem.WaterCells;
            var grid = worldGridSystem.Grid;
            var randomArray = randomSystem.RandomArray;

            Entities
                .WithNativeDisableContainerSafetyRestriction(randomArray)
                .WithReadOnly(blockedCells)
                .WithReadOnly(waterCells)
                .ForEach((int nativeThreadIndex, Entity entity,
                    ref LookingForRandomTarget lookingForRandomTarget,
                    in Translation translation,
                    in MovementTerrain movementTerrain) =>
            {
                bool onLand = movementTerrain.MovesOnLand;
                bool inWater = movementTerrain.MovesOnWater;

                int randomIndex = nativeThreadIndex % JobsUtility.MaxJobThreadCount;
                var random = randomArray[randomIndex];
                
                float3 target = translation.Value;
                
                float cap = 6; // cap + min = maximum distance from entity to pick a point.
                float min = 2; // The minimum distance away from the entity to pick a point.
                int tries = 10; // To handle case of no possible targets.

                for (int i = 0; i < tries; i++)
                {
                    float2 tile = random.NextFloat2(new float2(-cap, -cap), new float2(cap, cap));
                    tile.x += tile.x > 0 ? min : -min;
                    tile.y += tile.y > 0 ? min : -min;
                    
                    float3 potentialtarget = new float3(translation.Value.x + tile.x, 0f, translation.Value.z + tile.y);
                    
                    if (WorldGridSystem.IsWalkable(grid, blockedCells, waterCells, onLand, inWater,
                        grid.GetGridPosition(potentialtarget)))
                    {
                        target = potentialtarget;
                        break;
                    }
                }

                // Set result
                if (math.distance(target, translation.Value) > 1f)
                {
                    lookingForRandomTarget.HasFound = true;
                    lookingForRandomTarget.Position = target;
                }
                else
                {
                    lookingForRandomTarget.HasFound = false;
                }

                randomArray[randomIndex] = random; // Necessary to update the generator.
            }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}