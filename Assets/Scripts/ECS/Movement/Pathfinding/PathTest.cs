using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    
    /// <summary>
    /// Test class for pathfinding.
    /// </summary>
    public class PathTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 1000; i++)
            {
                CreateEntity();
            }
        }

        void CreateEntity()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = entityManager.CreateEntity(
                typeof(MoveCommand),
                typeof(Translation)
            );

            entityManager.SetComponentData(entity, 
                new MoveCommand { 
                    target = new float3(10, 0, 10),
                    reach = 0,
                    pathfind = true,
                    range = int.MaxValue
                }
            );
            entityManager.SetComponentData(entity, new Translation {Value = new float3(0,0,0)});
            entityManager.AddBuffer<PathElement>(entity);
            
        }
    }
}