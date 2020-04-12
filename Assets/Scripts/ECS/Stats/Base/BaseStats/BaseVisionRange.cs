using Ecosystem.ECS.Targeting.Sensing;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base
{
    [GenerateAuthoringComponent]
    public struct BaseVisionRange : IComponentData, BaseStat
    {
        public float Value;

        public float BaseValue => Value;
    }

    public class BaseVisionSystem : BaseStatSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Vision vision, in BaseVisionRange baseVisionRange)
                => ResetToBase(ref vision.Range, baseVisionRange)).ScheduleParallel();
        }
    }
}
