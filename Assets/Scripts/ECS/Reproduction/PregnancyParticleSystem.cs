using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Reproduction
{
    [GenerateAuthoringComponent]
    public class PregnancyParticleSystem : IComponentData
    {
        public ParticleSystem ParticleSystem;
    }
}
