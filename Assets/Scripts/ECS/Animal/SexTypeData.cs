using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Specifies the sex of the animal
    /// </summary>
    [Serializable]
    public struct SexTypeData : IComponentData
    {
        public int SexTypeId;
    }
}