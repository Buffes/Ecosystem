using Ecosystem.ECS.Animal.Needs;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base {
    [GenerateAuthoringComponent]
    public struct BaseHungerLimit : IComponentData, BaseStat {
        public float Value;

        public float BaseValue => Value;
    }

    public class BaseHungerLimitSystem : BaseStatSystem {
        protected override void OnUpdate() {
            Entities.ForEach((ref HungerLimit hungerLimit,in BaseHungerLimit baseHungerLimit)
                => ResetToBase(ref hungerLimit.Value,baseHungerLimit)).ScheduleParallel();
        }
    }
}
