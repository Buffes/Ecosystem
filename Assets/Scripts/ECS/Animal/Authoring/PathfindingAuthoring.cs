using Ecosystem.ECS.Movement.Pathfinding;
using Unity.Entities;
using UnityEngine;


public class PathfindingAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{

    private Entity entity;
    private EntityManager entityManager;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        this.entity = entity;
        this.entityManager = dstManager;

        dstManager.AddBuffer<UnreachablePosition>(entity);
        dstManager.AddBuffer<PathElement>(entity);
    }
}