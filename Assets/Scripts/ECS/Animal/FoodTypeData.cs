using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal
{
    [Serializable]
    public struct FoodTypeData : IComponentData
    {
        public int FoodTypeId;
        public int FoodPoints;
    }
}
