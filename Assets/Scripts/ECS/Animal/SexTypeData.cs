using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Specifies the sex of the animal. Male, Female
    /// </summary>
    [Serializable]
    public struct SexTypeData : IComponentData
    {
        public Sexes Sex;
    }
}