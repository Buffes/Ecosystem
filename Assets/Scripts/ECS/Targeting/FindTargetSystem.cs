using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{
    public class FindTargetSystem : ComponentSystem
    {

        protected override void OnUpdate()
        {
            
            
            Entities.WithAll<SightTag>().ForEach((Entity entity, DynamicBuffer<Target> targetBuffer, ref Translation unitTranslation/*Entity entity,ref Translation unitTranslation*/) =>
            {
                float3 unitPosition = unitTranslation.Value;
                Entity closestTargetEntity = Entity.Null;
                float3 closestTargetPosition = float3.zero;
                List<Entity> targets = new List<Entity>();
                targetBuffer.Clear();
                NativeList<Entity> targetEntityList = new NativeList<Entity>(Allocator.Temp);
                //Debug.Log("Who is looking:" + entity);

                Entities.ForEach((Entity targetEntity, ref Translation targetTranslation) =>
                {
                    //Check if the unit targets itself
                    if (!targetEntity.Equals(entity))
                    {

                        targetEntityList.Add(targetEntity);
                        //If unit has not yet targeted something; target the first target in collection
                        if (closestTargetEntity == Entity.Null)
                        {
                            closestTargetEntity = targetEntity;
                            closestTargetPosition = targetTranslation.Value;
                           // Debug.Log("first seen:" + targetEntity);
                        }
                        else
                        {
                            if (math.distance(unitPosition, targetTranslation.Value) < math.distance(unitPosition, closestTargetPosition))
                            {
                                closestTargetEntity = targetEntity;
                                closestTargetPosition = targetTranslation.Value;

                               // Debug.Log("Nearer than before:" + closestTargetEntity);
                            }
                        }
                    }

                });

                foreach (Entity target in targetEntityList)
                {
                    targetBuffer.Add(new Target { e = target });
                }

                targetEntityList.Dispose();

                //PostUpdateCommands.AddComponent(entity, new Targets { t = targets });
                //Draw line between unit and closest target
                if(closestTargetEntity != Entity.Null)
                {
                    Debug.DrawLine(unitPosition, closestTargetPosition, Color.green);
                    PostUpdateCommands.AddComponent(entity, new ClosestTarget { targetEntity = closestTargetEntity });
                }
            });
        }
    }
}