using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    [Serializable]
    public struct PreyTypesElement : IBufferElementData
    {
        public int AnimalTypeId;

        public static implicit operator int(PreyTypesElement e) { return e.AnimalTypeId; }
        public static implicit operator PreyTypesElement(int id) { return new PreyTypesElement { AnimalTypeId = id }; }
    }
}
