using Ecosystem.ECS.Animal.Needs;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base {
    [GenerateAuthoringComponent]
    public struct BaseThirstLimit : IComponentData, BaseStat {
        public float Value;

        public float BaseValue => Value;
    }

    public class BaseThirstLimitSystem : BaseStatSystem {
        protected override void OnUpdate() {
            Entities.ForEach((ref ThirstLimit thirstLimit,in BaseThirstLimit baseThirstLimit)
                => ResetToBase(ref thirstLimit.Value,baseThirstLimit)).ScheduleParallel();
        }
    }
}
