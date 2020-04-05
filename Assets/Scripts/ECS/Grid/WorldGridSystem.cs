using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Grid
{
    /// <summary>
    /// Holds info about the world grid.
    /// </summary>
    public class WorldGridSystem : SystemBase
    {
        public GridData Grid;

        public NativeArray<bool> OccupiedCells;
        public NativeArray<bool> BlockedCells;
        public NativeArray<bool> WaterCells;

        /// <summary>
        /// Sets a cell as occupied.
        /// </summary>
        public void SetOccupiedCell(int2 gridPos, bool blocksMovement = false)
        {
            OccupiedCells[Grid.GetCellIndex(gridPos)] = true;
            BlockedCells[Grid.GetCellIndex(gridPos)] = blocksMovement;
        }

        /// <summary>
        /// Sets a cell as water.
        /// </summary>
        public void SetWaterCell(int2 gridPos) => WaterCells[Grid.GetCellIndex(gridPos)] = true;

        protected override void OnCreate()
        {
            InitGrid(100, 100); // Default grid
        }

        /// <summary>
        /// Initializes the grid with amount of columns/rows equal to the specified width/height.
        /// </summary>
        public void InitGrid(int width, int height, float cellSize = 1f)
        {
            Grid = new GridData(width, height, cellSize);

            if (OccupiedCells.IsCreated) OccupiedCells.Dispose();
            if (BlockedCells.IsCreated) BlockedCells.Dispose();
            if (WaterCells.IsCreated) WaterCells.Dispose();
            OccupiedCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
            BlockedCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
            WaterCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            OccupiedCells.Dispose();
            BlockedCells.Dispose();
            WaterCells.Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}
