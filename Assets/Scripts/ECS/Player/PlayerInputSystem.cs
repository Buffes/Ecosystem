using Ecosystem.ECS.Movement;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.ECS.Player
{
    /// <summary>
    /// Movement input controls for player-controlled entities.
    /// </summary>
    public class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            bool sprint = Input.GetKey(KeyCode.LeftShift);
            float3 direction = new float3(x, 0f, z);

            if (math.length(direction) != 0)
            {
                direction = math.normalize(direction);
            }

            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref MovementInput movementInput) =>
                {
                    movementInput.Direction = direction;
                }).ScheduleParallel();

            if (sprint)
                Entities.WithStructuralChanges().WithoutBurst().WithAll<PlayerTag>().WithNone<Sprinting>().ForEach((Entity entity)
                    => EntityManager.AddComponentData(entity, new Sprinting())).Run();
            else
                Entities.WithStructuralChanges().WithoutBurst().WithAll<PlayerTag, Sprinting>().ForEach((Entity entity)
                    => EntityManager.RemoveComponent<Sprinting>(entity)).Run();
        }
    }
}
