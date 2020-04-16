using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// A float value ranging from 0 to infinity indicating the maximum value of the entitys SexualUrgesData
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct MaxSexualUrgesData : IComponentData
    {
        public float MaxUrge;
    }
}
