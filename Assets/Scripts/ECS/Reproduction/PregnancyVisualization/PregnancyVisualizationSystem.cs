using Ecosystem.ECS.Animal.Needs;
using Ecosystem.ECS.Debugging.Selection;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

namespace Ecosystem.ECS.Reproduction.PregrancyVisualization
{
    public class PregnancyVisualizationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithoutBurst()
                .WithAll<PregnancyData>()
                .ForEach((Entity entity,
                PregnancyParticleSystemPrefab prefab,
                in Translation position,
                in Rotation rotation) =>
            {
                if(!prefab.Prefab.IsAlive())
                    prefab.Prefab.Play();
            }).Run();

            Entities
                .WithoutBurst()
                .WithNone<PregnancyData>()
                .ForEach((Entity entity,
                PregnancyParticleSystemPrefab prefab,
                in Translation position,
                in Rotation rotation) =>
                {
                    if (prefab.Prefab.IsAlive())
                        prefab.Prefab.Stop();
                }).Run();
        }
    }
}
