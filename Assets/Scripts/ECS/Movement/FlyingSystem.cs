

using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Moves entities with the Flying component up to the flying height.
    /// </summary>
    public class FlyingSystem : SystemBase
    {
        private const float FLYING_HEIGHT = 10f;

        protected override void OnUpdate()
        {
            Entities
            .WithAll<Flying>()
            .ForEach((
                ref PhysicsVelocity velocity,
                in Translation translation,
                in MovementSpeed movementSpeed) =>
            {
                if (translation.Value.y < FLYING_HEIGHT)
                {
                    velocity.Linear.y = movementSpeed.Value;
                }
            }).ScheduleParallel();
        }
    }
}