using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// A float value ranging from 0 to infinity indicating the level of hunger. Hunger <= 0 == death
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct HungerData : IComponentData
    {
        public float Hunger;
    }
}
