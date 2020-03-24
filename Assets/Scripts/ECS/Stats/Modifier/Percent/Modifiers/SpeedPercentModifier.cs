using Ecosystem.ECS.Movement;
using Unity.Entities;

namespace Ecosystem.ECS.Stats.Modifier
{
    [GenerateAuthoringComponent]
    public struct SpeedPercentModifier : IBufferElementData
    {
        public float Value;
    }

    public class SpeedPercentModifierSystem : PercentModifierSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((
                ref DynamicBuffer<SpeedPercentModifier> speedPercentModifiers,
                ref MovementSpeed movementSpeed)
                => ApplyModifiers(ref movementSpeed.Value, speedPercentModifiers)).ScheduleParallel();
        }
    }
}
