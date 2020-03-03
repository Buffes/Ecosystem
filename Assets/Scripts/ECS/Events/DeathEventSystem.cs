using System;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Ecosystem.ECS.Hybrid;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

namespace Ecosystem.ECS.Events
{
    /// <summary>
    /// Kills desired entities
    /// </summary>
    public class DeathEventSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {

            Entities.WithAll<DeathCommand>().ForEach((Entity entity, /*int entityInQueryIndex,*/
                ref DeathCommand deathCmd) =>
            {
                PostUpdateCommands.DestroyEntity(deathCmd.target);
                PostUpdateCommands.DestroyEntity(entity);

            });
        }
    }
}
