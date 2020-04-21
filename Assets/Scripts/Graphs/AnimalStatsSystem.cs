using Unity.Entities;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Stats.Base;
using UnityEngine;

namespace Ecosystem.Graphs
{
    /// <summary>
    /// Collects animal data over time and saves it in CSV files.
    /// </summary>
    public class AnimalStatsSystem : SystemBase
    {
        private AnimalStats<DeathCause> deathStats = new AnimalStats<DeathCause>("DeathCauses.csv", "Death Cause");
        private AverageAnimalStats speedStats = new AverageAnimalStats("SpeedDoc.csv");
        private AverageAnimalStats hearingStats = new AverageAnimalStats("HearingDoc.csv");
        private AverageAnimalStats visionStats = new AverageAnimalStats("VisionDoc.csv");
        private AverageAnimalStats animalCount = new AverageAnimalStats("AnimalCountDoc.csv");

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

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData, in BaseHearingRange baseHearingRange) =>
                    {
                        hearingStats.AddStatValue(animalTypeData.AnimalName.ToString(), baseHearingRange.Value);
                    }).Run();

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData, in BaseVisionRange baseVisionRange) =>
                    {
                        visionStats.AddStatValue(animalTypeData.AnimalName.ToString(), baseVisionRange.Value);
                    }).Run();

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData) =>
                    {
                        animalCount.AddStatValue(animalTypeData.AnimalName.ToString(), 1);
                    }).Run();

                speedStats.AddDataPoint(time);
                hearingStats.AddDataPoint(time);
                visionStats.AddDataPoint(time);
                animalCount.AddDataPointCount(time);


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
            hearingStats.WriteToFile();
            visionStats.WriteToFile();
            animalCount.WriteToFile();
        }
    }
}