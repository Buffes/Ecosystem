using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct HungerData : IComponentData
    {
        public float Hunger;
    }
}
