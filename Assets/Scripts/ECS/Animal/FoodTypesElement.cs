using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    [Serializable]
    public struct FoodTypesElement : IBufferElementData
    {
        public int FoodTypeId;

        public static implicit operator int(FoodTypesElement e) { return e.FoodTypeId; }
        public static implicit operator FoodTypesElement(int id) { return new FoodTypesElement { FoodTypeId = id }; }
    }
}
