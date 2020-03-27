using Unity.Entities;
using Ecosystem.Attributes;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    public class AnimalPrefab : IComponentData
    {
        public Attributes.Animal Prefab;
    }
}
