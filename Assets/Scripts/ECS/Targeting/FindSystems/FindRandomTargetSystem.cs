using Ecosystem.ECS.Random;
using Ecosystem.ECS.Targeting.Targets;
using Ecosystem.Grid;
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

        protected override void OnUpdate()
        {
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

            Entities
                .WithNativeDisableParallelForRestriction(randomArray)
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
                    
                    if (GameZone.IsWalkable(target.x, target.y))
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
            }).ScheduleParallel();
        }
    }
}