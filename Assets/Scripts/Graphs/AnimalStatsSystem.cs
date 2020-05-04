using Unity.Entities;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Animal.Needs;
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
        private AnimalStats<float> ageOfDeathStats = new AnimalStats<float>("ageOfDeathData.csv","Age of Death");
        private AverageAnimalStats ageStats = new AverageAnimalStats("AgeDoc.csv");
        private AverageAnimalStats hungerLimitStats = new AverageAnimalStats("HungerLimitDoc.csv");
        private AverageAnimalStats thirstLimitStats = new AverageAnimalStats("ThirstLimitDoc.csv");
        private AverageAnimalStats matingLimitStats = new AverageAnimalStats("MatingLimitDoc.csv");


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

            Entities
                .WithoutBurst()
                .ForEach((in AnimalTypeData animalTypeData,in DeathEvent deathEvent,in AgeData ageOfDeathData) => {
                    if (deathEvent.Cause.Equals(DeathCause.Age)) {
                        ageOfDeathStats.AddDataPoint(
                            time,
                            animalTypeData.AnimalName.ToString(),
                            ageOfDeathData.Age);
                    }
                }).Run();

            timeUntilDataPoint -= Time.DeltaTime;
            if (timeUntilDataPoint < 0f)
            {
                timeUntilDataPoint = dataPointInterval;

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData,in AgeData age) => {
                        ageStats.AddStatValue(animalTypeData.AnimalName.ToString(),age.Age);
                    }).Run();

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData,in HungerLimit hungerLimit) => {
                        hungerLimitStats.AddStatValue(animalTypeData.AnimalName.ToString(),hungerLimit.Value);
                    }).Run();

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData,in ThirstLimit thirstLimit) => {
                        thirstLimitStats.AddStatValue(animalTypeData.AnimalName.ToString(),thirstLimit.Value);
                    }).Run();

                Entities
                    .WithoutBurst()
                    .ForEach((in AnimalTypeData animalTypeData,in MatingLimit matingLimit) => {
                        matingLimitStats.AddStatValue(animalTypeData.AnimalName.ToString(),matingLimit.Value);
                    }).Run();

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
                ageStats.AddDataPoint(time);
                hungerLimitStats.AddDataPoint(time);
                thirstLimitStats.AddDataPoint(time);
                matingLimitStats.AddDataPoint(time);

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
            ageOfDeathStats.WriteToFile();
            ageStats.WriteToFile();
            hungerLimitStats.WriteToFile();
            thirstLimitStats.WriteToFile();
            matingLimitStats.WriteToFile();
        }
    }
}