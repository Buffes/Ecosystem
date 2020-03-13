using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// A float value between 0 and 1 indicating the level of hunger. Hunger > 1 == death
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct HungerData : IComponentData
    {
        public float Hunger;
    }
}
