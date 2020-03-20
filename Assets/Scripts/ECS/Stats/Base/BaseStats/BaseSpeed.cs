using Ecosystem.ECS.Movement;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base
{
    [GenerateAuthoringComponent]
    public struct BaseSpeed : IComponentData, BaseStat
    {
        public float Value;

        public float BaseValue => Value;
    }

    public class BaseSpeedSystem : BaseStatSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref MovementSpeed movementspeed, in BaseSpeed baseSpeed)
                => ResetToBase(ref movementspeed.Value, baseSpeed)).ScheduleParallel();
        }
    }
}
