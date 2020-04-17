

using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Moves entities with the Flying component up to the flying height.
    /// </summary>
    public class FlyingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
            .WithAll<Flying>()
            .WithNone<LandCommand>()
            .ForEach((
                ref PhysicsVelocity velocity,
                in Translation translation,
                in MovementSpeed movementSpeed,
                in FlightData flightData) =>
            {
                if (translation.Value.y < flightData.Altitude)
                {
                    velocity.Linear.y = movementSpeed.Value;
                }
                else
                {
                    velocity.Linear.y = 0f;
                }
            }).ScheduleParallel();
        }
    }
}