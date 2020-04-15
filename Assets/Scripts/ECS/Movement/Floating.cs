using System;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// This entity is floating and should not be affected by gravity.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct Floating : IComponentData
    {
    }
}
