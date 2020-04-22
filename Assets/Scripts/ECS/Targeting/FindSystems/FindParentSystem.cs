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
    /// Looks for the parents of non-adult entities.
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
                .ForEach((Entity entity, int entityInQueryIndex,
                in Translation position,
                in DynamicBuffer<BucketAnimalData> sensedAnimals,
                in DynamicBuffer<UnreachablePosition> unreachablePositions,
                in MovementTerrain movementTerrain) =>
            {
                bool onLand = movementTerrain.MovesOnLand;
                bool inWater = movementTerrain.MovesOnWater;
                int closestPreyIndex = -1;
                float closestPreyDistance = 0f;

                // Check all animals that we can sense
                for (int i = 0; i < sensedAnimals.Length; i++)
                {
                    var sensedAnimalInfo = sensedAnimals[i];

                    AnimalTypeData targetAnimalType = sensedAnimalInfo.AnimalTypeData;
                    float3 targetPosition = sensedAnimalInfo.Position;
                    float targetDistance = math.distance(targetPosition, position.Value);


                }

                // Set result
                if (closestPreyIndex != -1)
                {
                    float3 preyPosition = sensedAnimals[closestPreyIndex].Position;
                    lookingForPrey.HasFound = true;
                    lookingForPrey.Entity = sensedAnimals[closestPreyIndex].Entity;
                    lookingForPrey.Position = preyPosition;

                    int length = 3; // Might need adjusting
                    float3 predictedPosition;
                    do
                    {
                        predictedPosition = preyPosition + length * math.normalizesafe(directions[lookingForPrey.Entity].Direction);
                        length--;
                    }
                    while (length >= 0 && !WorldGridSystem.IsWalkable(grid, blockedCells, waterCells, onLand, inWater,
                                                                    grid.GetGridPosition(predictedPosition)));

                    lookingForPrey.PredictedPosition = predictedPosition;

                }
                else
                {
                    lookingForPrey.HasFound = false;
                }

            }).ScheduleParallel();

        }

        private static bool IsPrey(AnimalTypeData animalType, DynamicBuffer<PreyTypesElement> preyBuffer)
        {
            for (int i = 0; i < preyBuffer.Length; i++)
            {
                int prey = preyBuffer[i].AnimalTypeId;

                if (animalType.AnimalTypeId == prey)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
