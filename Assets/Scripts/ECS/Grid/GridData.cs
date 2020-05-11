using Unity.Mathematics;

namespace Ecosystem.ECS.Grid
{
    public readonly struct GridData
    {
        private readonly int width;
        private readonly int height;
        public readonly float CellSize;

        public int Length => width * height;

        public GridData(int width, int height, float cellSize = 1f)
        {
            this.width = width;
            this.height = height;
            this.CellSize = cellSize;
        }

        /// <summary>
        /// If not interested in the indexing, such as if used for a hash map. The width and height
        /// are irrelevant.
        /// </summary>
        public GridData(float cellSize) : this(0, 0, cellSize) { }

        public int2 GetGridPosition(float3 worldPosition)
        {
            int x = (int)math.floor(worldPosition.x / CellSize);
            int z = (int)math.floor(worldPosition.z / CellSize);

            return new int2(x, z);
        }

        public float3 GetWorldPosition(int2 gridPosition)
        {
            float x = gridPosition.x + 0.5f;
            float z = gridPosition.y + 0.5f;

            return new float3(x, 0f, z);
        }

        /// <summary>
        /// Returns if the specified position is within the bounds of this grid.
        /// </summary>
        public bool IsInBounds(float3 worldPosition) => IsInBounds(GetGridPosition(worldPosition));
        public bool IsInBounds(int2 gridPosition)
        {
            int x = gridPosition.x;
            int z = gridPosition.y;
            return x >= 0 && z >= 0 && x < width && z < height;
        }

        /// <summary>
        /// Returns the converted one-dimensional index for the cell at the specified position.
        /// <para/>
        /// Works for every cell within the bounds of this grid.
        /// </summary>
        public int GetCellIndex(float3 worldPosition) => GetCellIndex(GetGridPosition(worldPosition));
        public int GetCellIndex(int2 gridPosition) => GetCellIndex(gridPosition.x, gridPosition.y);
        private int GetCellIndex(int x, int z) => z * width + x;

        /// <summary>
        /// Returns the grid position that has the specified cell index.
        /// </summary>
        public int2 GetGridPositionFromIndex(int index)
        {
            int x = index % width;
            int z = (index - x) / width;

            return new int2(x, z);
        }

        /// <summary>
        /// Returns a unique key for the cell at the specified position.
        /// <para/>
        /// Useful as keys in a hash map. Works anywhere in the world without the need of
        /// a width/height in the grid.
        /// </summary>
        public int GetCellKey(float3 worldPosition) => GetCellKey(GetGridPosition(worldPosition));
        public int GetCellKey(int2 gridPosition)
        {
            return (int)math.hash(gridPosition);
        }
    }
}
