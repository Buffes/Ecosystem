using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Grid
{
    /// <summary>
    /// Sets cells in the world as occupied at every position where there is a cell occupant entity,
    /// and restores the cell after the entity is destroyed.
    /// </summary>
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class CellOccupationSystem : SystemBase
    {
        private EndInitializationEntityCommandBufferSystem m_EndInitializationEcbSystem;
        private WorldGridSystem worldGridSystem;

        protected override void OnCreate()
        {
            m_EndInitializationEcbSystem = World
                .GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
            worldGridSystem = World.GetOrCreateSystem<WorldGridSystem>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = m_EndInitializationEcbSystem.CreateCommandBuffer().ToConcurrent();

            var grid = worldGridSystem.Grid;
            var occupiedCells = worldGridSystem.OccupiedCells;
            var blockedCells = worldGridSystem.BlockedCells;

            Entities
                .WithNone<OccupyingCell>()
                .ForEach((Entity entity, int entityInQueryIndex,
                in CellOccupant occupant, in Translation position) =>
                {
                    int2 gridPos = grid.GetGridPosition(position.Value);
                    int index = grid.GetCellIndex(gridPos);

                    if (occupiedCells[index]) return; // Already occupied

                    occupiedCells[index] = true;
                    blockedCells[index] = occupant.BlocksMovement;

                    commandBuffer.AddComponent(entityInQueryIndex, entity, new OccupyingCell
                    {
                        Value = gridPos
                    });
                }).ScheduleParallel();

            Entities
                .WithNone<CellOccupant>()
                .ForEach((Entity entity, int entityInQueryIndex,
                in OccupyingCell occupyingCell) =>
                {
                    int index = grid.GetCellIndex(occupyingCell.Value);
                    occupiedCells[index] = false;
                    blockedCells[index] = false;

                    commandBuffer.RemoveComponent<OccupyingCell>(entityInQueryIndex, entity);
                }).ScheduleParallel();

            m_EndInitializationEcbSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
