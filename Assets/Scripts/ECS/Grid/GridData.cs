using Unity.Mathematics;

namespace Ecosystem.ECS.Grid
{
    public struct GridData
    {
        private int width;
        private int height;
        private float cellSize;

        public int Length => width * height;

        public GridData(int width, int height, float cellSize = 1f)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
        }

        /// <summary>
        /// If not interested in the indexing, such as if used for a hash map.
        /// </summary>
        public GridData(float cellSize) : this(0, 0, cellSize) { }

        public int2 GetGridPosition(float3 worldPosition)
        {
            int x = (int)math.floor(worldPosition.x / cellSize);
            int z = (int)math.floor(worldPosition.z / cellSize);

            return new int2(x, z);
        }

        public bool IsInBounds(float3 worldPosition)
        {
            int2 gridPosition = GetGridPosition(worldPosition);
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
        /// Returns a unique key for the cell at the specified position.
        /// <para/>
        /// Useful as keys in a hash map. Works anywhere in the world without the need of
        /// a width/height in the grid.
        /// <para/>
        /// Uses the Cantor pairing function to generate a unique number for each pair of numbers.
        /// </summary>
        public int GetCellKey(float3 worldPosition) => GetCellKey(GetGridPosition(worldPosition));
        public int GetCellKey(int2 gridPosition)
        {
            int a = gridPosition.x;
            int b = gridPosition.y;
            return (a + b) * (a + b + 1) / 2 + b;
        }
    }
}
