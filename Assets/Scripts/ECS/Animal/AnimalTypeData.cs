using System;
using Unity.Entities;
using Unity.Collections;

namespace Ecosystem.ECS.Animal
{
    [Serializable]
    public struct AnimalTypeData : IComponentData
    {
        public int AnimalTypeId;
        public NativeString64 AnimalName;
    }
}
