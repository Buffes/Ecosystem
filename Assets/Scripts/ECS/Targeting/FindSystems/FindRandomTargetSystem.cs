using Ecosystem.ECS.Random;
using Ecosystem.ECS.Targeting.Targets;
using Ecosystem.Grid;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting
{
    /// <summary>
    /// Looks for a random walkable point somewhere around the animal and stores it for movement.
    /// </summary>
    public class FindRandomTargetSystem : SystemBase
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
            var walkableTiles = GameZone.walkableTiles;
            int2 gridSize = new int2(GameZone.tiles.GetLength(0), GameZone.tiles.GetLength(1));
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

            Entities
                .WithNativeDisableParallelForRestriction(randomArray)
                .WithReadOnly(walkableTiles)
                .ForEach((int nativeThreadIndex, Entity entity,
                    ref LookingForRandomTarget lookingForRandomTarget,
                    in Translation translation) =>
            {
                var random = randomArray[nativeThreadIndex];
                
                float3 target = translation.Value;
                
                float cap = 4; // cap + min = maximum distance from entity to pick a point.
                float min = 2; // The minimum distance away from the entity to pick a point.
                int tries = 10; // To handle case of no possible targets.

                for (int i = 0; i < tries; i++)
                {
                    float2 tile = random.NextFloat2(new float2(-cap, -cap), new float2(cap, cap));
                    tile.x += tile.x > 0 ? min : -min;
                    tile.y += tile.y > 0 ? min : -min;
                    
                    target = translation.Value + new float3(tile.x, 0f, tile.y);
                    
                    if (IsWalkable(walkableTiles, gridSize, target.x, target.y))
                    {
                        break;
                    }
                }

                // Set result
                if (!target.Equals(translation.Value))
                {
                    lookingForRandomTarget.HasFound = true;
                    lookingForRandomTarget.Position = target;
                }
                else
                {
                    lookingForRandomTarget.HasFound = false;
                }

                randomArray[nativeThreadIndex] = random; // Necessary to update the generator.
            }).ScheduleParallel(this.Dependency);

            m_EndSimulationEcbSystem.AddJobHandleForProducer(Dependency);
        }

        private static bool IsWalkable(NativeArray<bool> grid, int2 gridSize, float x, float y)
        {
            int xInt = (int)math.round(x);
            int yInt = (int)math.round(y);

            return grid[xInt + yInt * gridSize.x];
        }
    }
}