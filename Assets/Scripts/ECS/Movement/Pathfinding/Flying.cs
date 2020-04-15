using System;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// This entity is flying.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Flying : IComponentData
    {
    }
}
