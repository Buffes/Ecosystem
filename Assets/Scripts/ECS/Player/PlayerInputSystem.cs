using Ecosystem.ECS.Movement;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.ECS.Player
{
    /// <summary>
    /// Movement input controls for entities with a PlayerTag component.
    /// </summary>
    public class PlayerInputSystem : ComponentSystem
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

            Entities.ForEach((
                ref MovementInput movementInput,
                ref PlayerTag playerTag) =>
            {

                movementInput.Direction = direction;
                movementInput.Sprint = sprint;

            });
        }
    }
}
