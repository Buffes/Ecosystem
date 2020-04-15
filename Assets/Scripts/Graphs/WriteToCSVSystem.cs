
using System;   
using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Stats.Base;
using UnityEngine;
using System.IO;

public class WriteToCSVSystem : SystemBase
{
    private float sampleTime = 5f;
    private float currentTime = 0f;
    private float systemAge = 0f;
    string speedPath = "/Stats/SpeedDoc.csv";
    string hearingPath = "/Stats/HearingDoc.csv";
    string visionPath = "/Stats/VisionDoc.csv";

    protected override void OnCreate()
    {
        clear();
    }


    protected override void OnUpdate()
    {
        if (currentTime > sampleTime)
        {

            Entities.WithoutBurst().
                ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseSpeed baseSpeed
                ) =>
                {
                    if (animalTypeData.AnimalName.Equals("Fox"))
                    {
                        addRecord(Mathf.RoundToInt(systemAge).ToString(), 1.ToString(), baseSpeed.Value.ToString(), 0.ToString(), 0.ToString(), speedPath);
                    }
                    if (animalTypeData.AnimalName.Equals("Rabbit"))
                    {
                        addRecord(Mathf.RoundToInt(systemAge).ToString(), 0.ToString(), 0.ToString(), 1.ToString(), baseSpeed.Value.ToString(), speedPath);
                    }
                }).Run();

            Entities.WithoutBurst().
                ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseHearingRange baseHearingRange
                ) =>
                {
                    if (animalTypeData.AnimalName.Equals("Fox"))
                    {
                        addRecord(Mathf.RoundToInt(systemAge).ToString(), 1.ToString(), baseHearingRange.Value.ToString(), 0.ToString(), 0.ToString(), hearingPath);
                    }
                    if (animalTypeData.AnimalName.Equals("Rabbit"))
                    {
                        addRecord(Mathf.RoundToInt(systemAge).ToString(), 0.ToString(), 0.ToString(), 1.ToString(), baseHearingRange.Value.ToString(), hearingPath);
                    }
                }).Run();

            Entities.WithoutBurst().
                ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseVisionRange baseVisionRange
                ) =>
                {
                    if (animalTypeData.AnimalName.Equals("Fox"))
                    {
                        addRecord(Mathf.RoundToInt(systemAge).ToString(), 1.ToString(), baseVisionRange.Value.ToString(), 0.ToString(), 0.ToString(), visionPath);
                    }
                    if (animalTypeData.AnimalName.Equals("Rabbit"))
                    {
                        addRecord(Mathf.RoundToInt(systemAge).ToString(), 0.ToString(), 0.ToString(), 1.ToString(), baseVisionRange.Value.ToString(), visionPath);
                    }
                }).Run();
            currentTime = 0;
        }
        systemAge += Time.DeltaTime;
        currentTime += Time.DeltaTime;
    }

    private void addRecord(string time, string lionCount, string lionStat, string chickenCount, string chickenStat, string path)
    {
        StreamWriter file = new StreamWriter(Application.dataPath + path, true);
        var line = String.Format("{0},{1},{2},{3},{4}", time, lionCount, lionStat, chickenCount, chickenStat);
        file.WriteLine(line);
        file.Close();
    }

    private void clear()
    {
        StreamWriter speedFile = new StreamWriter(Application.dataPath + speedPath, false);
        speedFile.Close();
        StreamWriter hearingFile = new StreamWriter(Application.dataPath + hearingPath, false);
        hearingFile.Close();
        StreamWriter visionFile = new StreamWriter(Application.dataPath + visionPath, false);
        visionFile.Close();
    }
}


