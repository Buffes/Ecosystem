using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Sex
{
    /// <summary>
    /// Marks an entity as a Hermaphrodite
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct HermTag : IComponentData
    {

    }
}
