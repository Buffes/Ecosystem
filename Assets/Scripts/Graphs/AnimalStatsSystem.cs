using Unity.Entities;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Stats.Base;

namespace Ecosystem.Graphs
{
    /// <summary>
    /// Collects animal data over time and saves it in CSV files.
    /// </summary>
    public class AnimalStatsSystem : SystemBase
    {
        private AnimalStats<DeathCause> deathStats = new AnimalStats<DeathCause>("DeathCauses.csv", "Death Cause");
        private AverageAnimalStats speedStats = new AverageAnimalStats("SpeedDoc.csv");

        private float dataPointInterval = 5f;
        private float timeUntilDataPoint = 0f;

        protected override void OnUpdate()
        {
            var time = Time.ElapsedTime;

            Entities
                .WithoutBurst()
                .ForEach((in AnimalTypeData animalTypeData, in DeathEvent deathEvent) =>
                {
                    deathStats.AddDataPoint(
                        time,
                        animalTypeData.AnimalName.ToString(),
                        deathEvent.Cause);
                }).Run();

            timeUntilDataPoint -= Time.DeltaTime;
            if (timeUntilDataPoint < 0f)
            {
                timeUntilDataPoint = dataPointInterval;

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData, in BaseSpeed baseSpeed) =>
                    {
                        speedStats.AddStatValue(animalTypeData.AnimalName.ToString(), baseSpeed.Value);
                    }).Run();

                speedStats.AddDataPoint(time);
            }
        }

        protected override void OnDestroy()
        {
            Save();
        }

        private void Save()
        {
            deathStats.WriteToFile();
            speedStats.WriteToFile();
        }
    }
}
