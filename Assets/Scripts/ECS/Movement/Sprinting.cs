using Ecosystem.ECS.Stats;
using Unity.Entities;

namespace Ecosystem.ECS.Movement
{
    [GenerateAuthoringComponent]
    public struct Sprinting : IComponentData
    {
    }

    public class BuffSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Sprinting>()
                .ForEach((DynamicBuffer<SpeedBuff> speedBuffs) =>
                {
                    speedBuffs.Add(new SpeedBuff { Multiplier = 2 });
                }).ScheduleParallel();
        }
    }
}
