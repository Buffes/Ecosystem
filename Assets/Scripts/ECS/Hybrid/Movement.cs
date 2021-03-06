﻿using Ecosystem.ECS.Movement;
using Ecosystem.ECS.Movement.Pathfinding;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    public class Movement : HybridBehaviour
    {
        /// <summary>
        /// Sends a move command to move to the specified target position.
        /// Avoid calling this too often because for every frame that it is called in,
        /// a new path will be calculated.
        /// </summary>
        /// <param name="target">the target position to move to</param>
        /// <param name="reach">the distance away from the target to end the movement at</param>
        /// <param name="range">the max distance of the resulting path</param>
        public void Move(Vector3 target, float reach, int maxTiles)
        {
            EntityManager.AddComponentData(Entity, new MoveCommand
            {
                Target = target,
                Reach = reach,
                Pathfind = true,
                MaxTiles = maxTiles
            });
        }

        /// <summary>
        /// Starts/stops sprinting.
        /// </summary>
        public void Sprint(bool enabled)
        {
            if (enabled == EntityManager.HasComponent<SprintInput>(Entity)) return;
            if (enabled)
            {
                EntityManager.AddComponentData(Entity, new SprintInput());
            }
            else
            {
                EntityManager.RemoveComponent<SprintInput>(Entity);
            }
        }
        /// <summary>
        /// Starts/stops flying. Only has an effect if entity has a FlightData component.
        /// </summary>
        public void Fly(bool enabled)
        {
            if (!EntityManager.HasComponent<FlightData>(Entity)) return;
            
            if (enabled == EntityManager.HasComponent<Flying>(Entity)) 
            {
                EntityManager.RemoveComponent<LandCommand>(Entity);
            }
            else if (enabled)
            {
                EntityManager.AddComponentData(Entity, new Flying());
            }
            else
            {
                EntityManager.AddComponentData(Entity, new LandCommand());
            }
        }

        /// <summary>
        /// If this entity is still flying. It can still fly for a while after flying has been turned off, since it needs time to land.
        /// </summary>
        /// <returns>If this entity is still flying.</returns>
        public bool IsFlying()
        {
            return EntityManager.HasComponent<Flying>(Entity);
        }

        public Vector3 GetPosition()
        {
            return EntityManager.GetComponentData<Translation>(Entity).Value;
        }

        public Quaternion GetRotation()
        {
            return EntityManager.GetComponentData<Rotation>(Entity).Value;
        }

        public Vector3 GetScale()
        {
            // Scale factors are along the diagonal of this matrix.
            float4x4 scale = EntityManager.GetComponentData<CompositeScale>(Entity).Value;
            
            return new Vector3(scale.c0.x, scale.c1.y, scale.c2.z);
        }

        public float3 GetMovementInput() {
            return EntityManager.GetComponentData<MovementInput>(Entity).Direction;
        }
    }
}
