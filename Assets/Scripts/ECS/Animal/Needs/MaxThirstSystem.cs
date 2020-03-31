using Unity.Entities;
using Ecosystem.ECS.Death;

namespace Ecosystem.ECS.Animal.Needs {
    /// <summary>
    /// System for clamping down thirst on an animal.
    /// </summary>
    public class MaxThirstSystem : SystemBase {

        protected override void OnUpdate() {

            Entities.ForEach((ref ThirstData thirstData,in MaxThirstData maxThirstData) => {

                    if (thirstData.Thirst >= maxThirstData.MaxThirst) {
                        thirstData.Thirst = maxThirstData.MaxThirst;
                    }

                }).ScheduleParallel();
        }
    }
}
