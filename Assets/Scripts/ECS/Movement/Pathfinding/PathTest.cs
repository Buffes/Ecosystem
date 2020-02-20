using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Movement.Pathfinding
{
        
    public class PathTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = entityManager.CreateEntity(typeof(MoveCommand));

            entityManager.SetComponentData(entity, new MoveCommand { 
                                                                    target = new float3(10, 0, 10),
                                                                    reach = 0,
                                                                    pathfind = true,
                                                                    range = int.MaxValue
                                                                   });
        }
    }
}