using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Sex
{
    /// <summary>
    /// Marks an entity as Male
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MaleTag : IComponentData
    {

    }
}
