using System;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting.Sensors
{
    /// <summary>
    /// Vision cone described as a circular segment with a radius and an angle between the left and right edges.
    /// Use the transform rotation to get the forward direction.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Vision : IComponentData
    {
        public float Range; // radius of segment.
        public float Angle; // Radians specifying the angle between the forward direction and the sides.
    }
}