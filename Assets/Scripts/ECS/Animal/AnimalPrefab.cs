using Unity.Entities;
using Ecosystem.Attributes;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    [GenerateAuthoringComponent]
    public struct AnimalPrefab : IComponentData
    {
        public Attributes.Animal Prefab;
    }
}
