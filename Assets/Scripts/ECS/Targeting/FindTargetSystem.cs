using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{
    public class FindTargetSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<Unit>().ForEach((Entity entity) =>
            {
                Entity closestTargetEntity = Entity.Null;

                Entities.WithAll<Target>().ForEach((Entity targetEntity, ref Translation targetTranslation) =>
                {

                });
            });
        }
    }
}