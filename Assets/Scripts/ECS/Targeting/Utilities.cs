
using Ecosystem.ECS.Movement.Pathfinding;
using Unity.Entities;
using Unity.Mathematics;

namespace Ecosystem.ECS.Targeting.Sensors
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

            if (math.length(relativePosition) > vision.Range) {
                return false; // Target outside range
            }
            relativePosition = math.normalize(relativePosition);
            float3 forward = math.normalize(math.forward(rotation));
            float forwardAngle = math.atan2(forward.z, forward.x);
            
            float targetAngle = math.atan2(relativePosition.z, relativePosition.x);
            bool intersects = math.abs(targetAngle - forwardAngle) < vision.Angle;
            return intersects;
        }


        public static bool IsUnreachable(DynamicBuffer<UnreachablePosition> buffer, float3 position)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].Position.Equals(GetGridCoords(position)))
                {
                    return true;
                }
            }
            
            return false;
        }

        private static int2 GetGridCoords(float3 worldPosition)
        {
            int x = (int)math.round(worldPosition.x);
            int z = (int)math.round(worldPosition.z);
            return new int2(x, z);
        }
    }
}