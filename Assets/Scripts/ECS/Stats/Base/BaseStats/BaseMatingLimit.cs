using Ecosystem.ECS.Animal.Needs;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Base {
    [GenerateAuthoringComponent]
    public struct BaseMatingLimit : IComponentData, BaseStat {
        public float Value;

        public float BaseValue => Value;
    }

    public class BaseMatingLimitSystem : BaseStatSystem {
        protected override void OnUpdate() {
            Entities.ForEach((ref MatingLimit matingLimit,in BaseMatingLimit baseMatingLimit)
                => ResetToBase(ref matingLimit.Value,baseMatingLimit)).ScheduleParallel();
        }
    }
}
