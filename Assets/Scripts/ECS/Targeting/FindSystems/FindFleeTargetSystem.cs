using Ecosystem.ECS.Random;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs.LowLevel.Unsafe;
using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Movement;
using UnityEngine;

namespace Ecosystem.ECS.Targeting.FindSystems {
    /// <summary>
    /// Looks for a random walkable point somewhere around the animal and stores it for movement.
    /// </summary>
    [UpdateInGroup(typeof(FindSystemGroup))]
    public class FindFleeTargetSystem : SystemBase {
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;
        private RandomSystem randomSystem;
        private WorldGridSystem worldGridSystem;

        protected override void OnCreate() {
            m_EndSimulationEcbSystem = World
                .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            randomSystem = World.GetOrCreateSystem<RandomSystem>();
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate() {
            var blockedCells = worldGridSystem.BlockedCells;
            var waterCells = worldGridSystem.WaterCells;
            var grid = worldGridSystem.Grid;
            var randomArray = randomSystem.RandomArray;

            Entities
                .WithNativeDisableContainerSafetyRestriction(randomArray)
                .WithReadOnly(blockedCells)
                .WithReadOnly(waterCells)
                .ForEach((int nativeThreadIndex,Entity entity,
                    ref LookingForFleeTarget lookingForFleeTarget,
                    in LookingForPredator lookingForPredator,
                    in Translation translation,
                    in MovementTerrain movementTerrain) =>
                {
                    if (!lookingForPredator.HasFound) return; // No predator to flee from

                    bool onLand = movementTerrain.MovesOnLand;
                    bool inWater = movementTerrain.MovesOnWater;

                    int randomIndex = nativeThreadIndex % JobsUtility.MaxJobThreadCount;
                    var random = randomArray[randomIndex];

                    float3 target = translation.Value;
                    float3 diff = target - lookingForPredator.Position;
                    float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                    float3 startingPoint = target + 3f * diff / diffLength;

                    float max = 2; //  maximum distance from startingPoint to pick a point.
                    int tries = 10; // To handle case of no possible targets.

                    for (int i = 0; i < tries; i++) {
                        float2 tile = random.NextFloat2(new float2(-max,-max),new float2(max,max));
                        //tile.x += tile.x > 0 ? min : -min;
                        //tile.y += tile.y > 0 ? min : -min;

                        float3 potentialtarget = new float3(startingPoint.x + tile.x,0f,startingPoint.z + tile.y);

                        if (WorldGridSystem.IsWalkable(grid,blockedCells,waterCells,onLand,inWater,
                            grid.GetGridPosition(potentialtarget))) {
                            target = potentialtarget;
                            break;
                        }
                    }

                    // Set result
                    if (math.distance(target,translation.Value) > 1f) {
                        lookingForFleeTarget.HasFound = true;
                        lookingForFleeTarget.Position = target;
                    } else {
                        lookingForFleeTarget.HasFound = false;
                    }

                    randomArray[randomIndex] = random; // Necessary to update the generator.
                }).ScheduleParallel();

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
