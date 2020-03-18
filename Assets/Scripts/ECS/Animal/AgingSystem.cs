using Unity.Entities;


namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Increases the age of all animals.
    /// </summary>
    public class AgingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            Entities.ForEach((Entity entity, int entityInQueryIndex,
            ref AgeData age) =>
            {
                // Store age in seconds.
                age.Age += deltaTime;

            }).ScheduleParallel();
        }
    }
}