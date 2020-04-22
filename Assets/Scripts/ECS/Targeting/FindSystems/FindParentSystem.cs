using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Grid.Buckets;
using Ecosystem.ECS.Movement;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Targets;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Targeting.FindSystems
{
    /// <summary>
    /// Looks for the parents of non-adult entities. So that baby animals only know where their parent is if they can sense them.
    /// </summary>
    [UpdateInGroup(typeof(FindSystemGroup))]
    public class FindParentSystem : SystemBase
    {
        WorldGridSystem worldGridSystem;

        protected override void OnCreate()
        {
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate()
        {
            var blockedCells = worldGridSystem.BlockedCells;
            var waterCells = worldGridSystem.WaterCells;
            var grid = worldGridSystem.Grid;
            var directions = GetComponentDataFromEntity<MovementInput>();

            Entities
                .WithReadOnly(blockedCells)
                .WithReadOnly(waterCells)
                .WithReadOnly(directions)
                .WithNone<Adult>()
                .ForEach((ref LookingForParent lookingForParent,
                in DynamicBuffer<BucketAnimalData> sensedAnimals,
                in DynamicBuffer<UnreachablePosition> unreachablePositions,
                in ParentData parent) =>
            {
                int parentIndex = -1;
                float3 parentPosition = new float3();

                // Check all animals that we can sense
                for (int i = 0; i < sensedAnimals.Length; i++)
                {
                    var sensedAnimalInfo = sensedAnimals[i];

                    if (sensedAnimalInfo.Entity == parent.Entity)
                    {
                        parentIndex = i;
                        parentPosition = sensedAnimals[i].Position;
                    }
                }

                // Set result
                if (parentIndex != -1)
                {
                    float3 ParentPosition = sensedAnimals[parentIndex].Position;
                    lookingForParent.HasFound = true;
                    lookingForParent.Entity = sensedAnimals[parentIndex].Entity;
                    lookingForParent.Position = parentPosition;
                }
                else
                {
                    lookingForParent.HasFound = false;
                }

            }).ScheduleParallel();
        }
    }
}
