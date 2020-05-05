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
        public NativeArray<bool> DrinkableCells;
        public NativeArray<float> HeightMap;

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
        public void SetWaterCell(int2 gridPosition) => WaterCells[Grid.GetCellIndex(gridPosition)] = true;

        public void setDrinkableCell(int2 gridPosition) => DrinkableCells[Grid.GetCellIndex(gridPosition)] = true;

        public void SetHeight(int2 gridPosition, float height) => HeightMap[Grid.GetCellIndex(gridPosition)] = height;

        /// <summary>
        /// Returns if the specified position is walkable.
        /// </summary>
        public bool IsWalkable(bool land, bool water, int2 gridPosition)
            => IsWalkable(Grid, BlockedCells, WaterCells, land, water, gridPosition);

        /// <summary>
        /// Returns if the specified position is walkable. Safe to use in systems.
        /// </summary>
        public static bool IsWalkable(GridData grid, NativeArray<bool> blockedCells, NativeArray<bool> waterCells,
            bool land, bool water, int2 gridPosition)
        {
            int i = grid.GetCellIndex(gridPosition);

            if (!grid.IsInBounds(gridPosition)) return false;
            if (blockedCells[i]) return false;
            if (waterCells[i] && !water) return false;
            if (!waterCells[i] && !land) return false;
            return true;
        }

        protected override void OnCreate()
        {
            InitGrid(100, 100); // Default grid
        }

        /// <summary>
        /// Initializes the grid shape.
        /// </summary>
        public void InitGrid(int width, int height, float cellSize = 1f)
            => InitGrid(new GridData(width, height, cellSize));
        public void InitGrid(GridData grid)
        {
            Grid = grid;

            if (OccupiedCells.IsCreated) OccupiedCells.Dispose();
            if (BlockedCells.IsCreated) BlockedCells.Dispose();
            if (WaterCells.IsCreated) WaterCells.Dispose();
            if (DrinkableCells.IsCreated) DrinkableCells.Dispose();
            if (HeightMap.IsCreated) HeightMap.Dispose();
            OccupiedCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
            BlockedCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
            WaterCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
            DrinkableCells = new NativeArray<bool>(Grid.Length, Allocator.Persistent);
            HeightMap = new NativeArray<float>(Grid.Length, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            OccupiedCells.Dispose();
            BlockedCells.Dispose();
            WaterCells.Dispose();
            DrinkableCells.Dispose();
            HeightMap.Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}
