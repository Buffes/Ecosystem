using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{
    public class FindTargetSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            
            Entities.WithNone<HasTarget>().WithAll<Unit>().ForEach((Entity entity, ref Translation unitTranslation) =>
            {
                float3 unitPosition = unitTranslation.Value;
                Entity closestTargetEntity = Entity.Null;
                float3 closestTargetPosition = float3.zero;
                Debug.Log(entity);

                Entities.WithAll<Unit>().ForEach((Entity targetEntity, ref Translation targetTranslation) =>
                {
                    //Check if the unit targets itself
                    if (!targetEntity.Equals(entity))
                    {

                        //If unit has not yet targeted something; target the first target in collection
                        if (closestTargetEntity == Entity.Null)
                        {
                            closestTargetEntity = targetEntity;
                            closestTargetPosition = targetTranslation.Value;
                            Debug.Log(targetEntity);
                        }
                        else
                        {
                            if (math.distance(unitPosition, targetTranslation.Value) > math.distance(unitPosition, closestTargetPosition))
                            {
                                closestTargetEntity = entity;
                                closestTargetPosition = targetTranslation.Value;
                            }
                        }
                    }

                });

                //Draw line between unit and closest target
                if(closestTargetEntity != Entity.Null)
                {
                    Debug.DrawLine(unitPosition, closestTargetPosition);
                    PostUpdateCommands.AddComponent(entity, new HasTarget { targetEntity = closestTargetEntity });
                }
            });
        }
    }
}