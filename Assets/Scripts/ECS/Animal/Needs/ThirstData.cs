using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    [Serializable]
    [GenerateAuthoringComponent]
    public struct ThirstData : IComponentData
    {
        public float Thirst;
    }
}
