using System;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting.Sensing
{
    /// <summary>
    /// Vision cone described as a circle sector with a radius and an angle between the forward direction and the edges.
    /// Use the entity's rotation to get the forward direction.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Vision : IComponentData
    {
        public float Range; // radius of the circle sector.
        
        /// <summary>
        /// The angle between the forward direction and the edges of the sector, in radians.
        /// </summary>
        public float Angle;
    }
}