using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Grid
{
    /// <summary>
    /// Currently occupying a cell in the world grid.
    /// </summary>
    public struct OccupyingCell : ISystemStateComponentData
    {
        public int2 Value; // The cell that this entity occupies
    }
}
