using Ecosystem.ECS.Random;
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
            var randomArray = World.GetExistingSystem<RandomSystem>().RandomArray;

            Entities
            .WithNativeDisableParallelForRestriction(randomArray)
            .ForEach((int nativeThreadIndex, Entity entity, int entityInQueryIndex,
            ref AgeData age) =>
            {
                var random = randomArray[nativeThreadIndex];

                // Store age in seconds.
                age.Age += deltaTime;
                
                // Average of once per second
                if (random.NextFloat() < 0.0167f)
                {
                    // TODO: chance to die based on age.
                }
                
                randomArray[nativeThreadIndex] = random; // Necessary to update the generator.
            }).ScheduleParallel();
        }
    }
}