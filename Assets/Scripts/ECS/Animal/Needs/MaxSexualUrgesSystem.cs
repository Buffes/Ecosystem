using Unity.Entities;
using Ecosystem.ECS.Death;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// System for clamping down sexual urge on an animal.
    /// </summary>
    public class MaxSexualUrgesSystem : SystemBase
    {

        protected override void OnUpdate()
        {

            Entities.ForEach((ref SexualUrgesData urgeData, in MaxSexualUrgesData maxUrgeData) =>
            {
                if (urgeData.Urge >= maxUrgeData.MaxUrge)
                {
                    urgeData.Urge = maxUrgeData.MaxUrge;
                }

            }).ScheduleParallel();
        }
    }
}
