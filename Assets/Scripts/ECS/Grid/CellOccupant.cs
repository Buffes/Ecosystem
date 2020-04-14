using Unity.Entities;

namespace Ecosystem.ECS.Grid
{
    /// <summary>
    /// An entity that is occupying a cell in the world grid. A cell can only have one occupant.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct CellOccupant : IComponentData
    {
        public bool BlocksMovement; // If this entity blocks movement in its cell
    }
}
