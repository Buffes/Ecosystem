using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Reproduction.PregrancyVisualization
{
    [GenerateAuthoringComponent]
    public class PregnancyParticleSystemPrefab : IComponentData
    {
        public ParticleSystem Prefab;
    }
}
