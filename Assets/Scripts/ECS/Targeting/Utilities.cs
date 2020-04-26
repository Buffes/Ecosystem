using Ecosystem.ECS.Grid;
using Ecosystem.ECS.Movement.Pathfinding;
using Ecosystem.ECS.Targeting.Sensing;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.ECS.Targeting
{
    public static class Utilities
    {
        /// <summary>
        /// Checks intersection between a point at targetPosition and a circle sector defined by the vision parameter.
        /// </summary>
        /// <param name="targetPosition"> The position of the target. </param>
        /// <param name="position"> The position value of the entity. </param>
        /// <param name="rotation"> The rotation value of the entity. </param>
        /// <param name="vision"> The field of vision of the entity, defined by a range and an angle. </param>
        /// <returns> True if targetPosition intersects the circle sector defined by vision, otherwise false. </returns>
        public static bool IntersectsVision(float3 targetPosition, float3 position, quaternion rotation, Vision vision)
        {
            float3 relativePosition = targetPosition - position;

            if (math.length(relativePosition) > vision.Range)
            {
                return false; // Target outside range
            }
            relativePosition = math.normalize(relativePosition);
            float3 forward = math.normalize(math.forward(rotation));
            float forwardAngle = math.atan2(forward.z, forward.x);

            float targetAngle = math.atan2(relativePosition.z, relativePosition.x);
            bool intersects = math.abs(targetAngle - forwardAngle) < vision.Angle;
            return intersects;
        }


        public static bool IsUnreachable(DynamicBuffer<UnreachablePosition> buffer, float3 position, GridData grid)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                Debug.Log(buffer[i].Position + " in unreachables...");
                Debug.Log("testing with " + grid.GetGridPosition(position));
                if (buffer[i].Position.Equals(grid.GetGridPosition(position)))
                {
                    Debug.Log(position + " is unreachable!");
                    return true;
                }
            }

            return false;
        }
    }
}