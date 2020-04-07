using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    [Serializable]
    public struct AnimalTypeData : IComponentData
    {
        public int AnimalTypeId;
        public AnimalTypeNames AnimalName;
    }
}
