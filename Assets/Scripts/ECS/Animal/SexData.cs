using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Specifies the sex of the animal. Male, Female
    /// </summary>
    [Serializable]
    public struct SexData : IComponentData
    {
        public Sex Sex;
    }
}