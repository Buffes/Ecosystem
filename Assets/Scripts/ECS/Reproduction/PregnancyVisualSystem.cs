using Unity.Entities;
using Ecosystem.ECS.Animal;

namespace Ecosystem.ECS.Reproduction
{
    [UpdateBefore(typeof(BirthSystem))]
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
                    particleSystem.ParticleSystem.Stop();
                    particleSystem.ParticleSystem.Clear();
                }).Run();
            Entities
                .WithAll<ReproductionEvent>()
                .WithoutBurst()
                .ForEach((Entity entity,
                PregnancyParticleSystem particleSystem,
                in SexData sexData) =>
                {
                    if (sexData.Sex == Sex.Female)
                    {
                        particleSystem.ParticleSystem.Play();
                    }
                }).Run();
        }
    }
}
