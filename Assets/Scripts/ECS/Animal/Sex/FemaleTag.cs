using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Sex
{
    /// <summary>
    /// Marks an entity as Female
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct FemaleTag : IComponentData
    {

    }
}
