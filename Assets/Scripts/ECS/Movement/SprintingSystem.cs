using Ecosystem.ECS.Stats.Modifier;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    /// <summary>
    /// Increases movement speed if sprinting.
    /// </summary>
    public class SprintingSystem : SystemBase
    {
        private const float SPEED_INCREASE = 1f; // 100% increase

        protected override void OnUpdate()
        {
            Entities
                .WithAll<Sprinting>()
                .ForEach((DynamicBuffer<SpeedPercentModifier> speedPercentModifiers) =>
                {
                    speedPercentModifiers.Add(new SpeedPercentModifier { Value = SPEED_INCREASE });
                }).ScheduleParallel();
        }
    }
}
