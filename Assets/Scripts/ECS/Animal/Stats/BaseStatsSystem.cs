using Ecosystem.ECS.Movement;
using Ecosystem.ECS.Targeting.Sensors;
using Unity.Entities;

namespace Ecosystem.ECS.Animal.Stats
{
    /// <summary>
    /// Recalculates practical stats from base stats.
    /// <para/>
    /// If the stats are affected by other factors, such as an animal being temporarily crippled,
    /// that would be an extra calculation step that runs after this system.
    /// </summary>
    public class BaseStatsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref MovementStats movementStats, in BaseSpeed baseSpeed) =>
            {
                movementStats.Speed = baseSpeed.Value;
            }).ScheduleParallel();

            Entities.ForEach((ref Hearing hearing, in BaseHearing baseHearing) =>
            {
                hearing.Range = baseHearing.Range;
            }).ScheduleParallel();

            Entities.ForEach((ref Vision vision, in BaseVision baseVision) =>
            {
                vision.Range = baseVision.Range;
                vision.Angle = baseVision.Angle;
            }).ScheduleParallel();
        }
    }
}
