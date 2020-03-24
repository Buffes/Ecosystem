using System;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// A float value representing the sexual urges of an animal.
    /// </summary>
    [Serializable]
    [GenerateAuthoringComponent]
    public struct SexualUrgesData : IComponentData
    {
        public float Urge;
    }
}
