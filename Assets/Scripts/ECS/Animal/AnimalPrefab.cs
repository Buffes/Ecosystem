using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    [GenerateAuthoringComponent]
    public struct AnimalPrefab : IComponentData
    {
        public GameObject Prefab;
    }
}
