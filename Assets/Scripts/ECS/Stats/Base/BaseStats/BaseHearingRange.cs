using Ecosystem.ECS.Targeting.Sensors;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base
{
    [GenerateAuthoringComponent]
    public struct BaseHearingRange : IComponentData, BaseStat
    {
        public float Value;

        public float BaseValue => Value;
    }

    public class BaseHearingSystem : BaseStatSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Hearing hearing, in BaseHearingRange baseHearingRange)
                => ResetToBase(ref hearing.Range, baseHearingRange)).ScheduleParallel();
        }
    }
}
