using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Movement.Pathfinding
{
    /// <summary>
    /// Attaches to game objects to allow pathfinding.
    /// </summary>
    public class PathBufferAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<PathElement>(entity);
        }
    }
}
