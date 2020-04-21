
using Unity.Entities;
using Ecosystem.ECS.Animal;

namespace Ecosystem.ECS.Reproduction
{
    public class PregnancyVisualSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<BirthEvent>()
                .WithoutBurst()
                .ForEach((Entity entity,
                PregnancyParticleSystem particleSystem) =>
                {
                    if (particleSystem.ParticleSystem.IsAlive())
                        particleSystem.ParticleSystem.Stop();
                }).Run();
            Entities
                .WithAll<ReproductionEvent>()
                .WithoutBurst()
                .ForEach((Entity entity,
                PregnancyParticleSystem particleSystem,
                in SexData sexData) =>
                {
                    if(sexData.Sex == Sex.Female) 
                    {
                        if(!particleSystem.ParticleSystem.IsAlive())
                            particleSystem.ParticleSystem.Play();
                    }
                }).Run();
        }
    }
}
