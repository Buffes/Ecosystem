using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Reproduction
{
    [GenerateAuthoringComponent]
    public class PregnancyParticleSystemPrefab : IComponentData
    {
        public ParticleSystem ParticleSystem;
    }
}
