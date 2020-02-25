using Ecosystem.ECS.Movement.Pathfinding;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    public class Movement : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Entity entity;
        private EntityManager entityManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entity = entity;
            entityManager = dstManager;
        }

        /// <summary>
        /// Sends a move command to move to the specified target position.
        /// Avoid calling this too often because for every frame that it is called in,
        /// a new path will be calculated.
        /// </summary>
        /// <param name="target">the target position to move to</param>
        /// <param name="reach">the distance away from the target to end the movement at</param>
        /// <param name="range">the max distance of the resulting path</param>
        public void Move(Vector3 target, float reach, float range)
        {
            entityManager.AddComponentData(entity, new MoveCommand
            {
                target = target,
                reach = reach,
                pathfind = true,
                range = range
            });
        }
    }
}
