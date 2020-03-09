using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Specifies the sex of the animal. Male, Female, Hermaphrodite
    /// </summary>
    [Serializable]
    public struct SexTypeData : IComponentData
    {
        public int SexTypeId;
    }
}