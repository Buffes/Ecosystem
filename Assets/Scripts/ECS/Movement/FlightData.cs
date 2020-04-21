using System;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Data about the entity's flying behaviour.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct FlightData : IComponentData
    {
        public float Altitude;
    }
}
