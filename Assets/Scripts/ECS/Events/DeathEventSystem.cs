using System;
using UnityEngine;
using Unity.Jobs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

namespace Assets.Scripts.ECS.Events
{
    /// <summary>
    /// Kills desired entities
    /// </summary>
    public class DeathEventSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in DeathCommand death) =>
            {
                
            }).ScheduleParallel();
        }
    }
}
