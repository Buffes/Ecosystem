using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Ecosystem.ECS.Death;


namespace Ecosystem.ParticleSystems
{
    public class DeathParticleSystem : SystemBase
    {
        private ParticleSystem death;

        protected override void OnUpdate()
        {
            death = ParticleMono.death;

            Entities
                .WithoutBurst()
                .ForEach((in Translation translation, in DeathEvent deathEvent) =>
                {
                    if (deathEvent.Cause != DeathCause.Food)
                        ParticleMono.InstantiateParticles(death, translation.Value, 2f);
                }).Run();
        }
    }
}
