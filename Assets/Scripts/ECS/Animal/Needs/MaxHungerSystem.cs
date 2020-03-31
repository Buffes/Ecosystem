using Unity.Entities;
using Ecosystem.ECS.Death;

namespace Ecosystem.ECS.Animal.Needs
{
    /// <summary>
    /// System for clamping down hunger on an animal.
    /// </summary>
    public class MaxHungerSystem : SystemBase
    {

        protected override void OnUpdate()
        {

            Entities.ForEach((ref HungerData hungerData,in MaxHungerData maxHungerData) =>
            {
                    if (hungerData.Hunger >= maxHungerData.MaxHunger)
                    {
                        hungerData.Hunger = maxHungerData.MaxHunger;
                    }

                }).ScheduleParallel();
        }
    }
}
