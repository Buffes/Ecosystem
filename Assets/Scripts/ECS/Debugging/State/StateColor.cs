using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Debugging
{
    /// <summary>
    /// Display color of the "current state" indicator.
    /// </summary>
    [GenerateAuthoringComponent]
    public struct StateColor : IComponentData
    {
        public Color Value;
    }
}
