
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Stats.Base;
using UnityEngine;
using System.IO;
using Ecosystem.Gameplay;
using Unity.Collections;

public class WriteToCSVSystem : SystemBase
{

    public static List<(int, float, float, float, Entity)> attributeList = new List<(int, float,float, float, Entity)>();
    private AnimalType chicken;
    private AnimalType lion;
    private float sampleTime = 5f;
    private float currentTime = 0f;
    private float systemAge = 0f;
    private int lionCount = 0;
    private int chickenCount = 0;
    private float lionStat = 0;
    private float chickenStat = 0;
    string speedPath = "/Scripts/Graphs/SpeedDoc.csv";
    string hearingPath = "/Scripts/Graphs/HearingDoc.csv";
    string visionPath = "/Scripts/Graphs/VisionDoc.csv";

    protected override void OnCreate()
    {
        clear();
    }


    protected override void OnUpdate()
    {
        /*   int i = 0;
           Entities.WithoutBurst().
               ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseSpeed baseSpeed, in BaseHearingRange baseHearingRange, in BaseVisionRange baseVisionRange
              ) =>
              {

                  if(i < 3) {
                      Debug.Log("#1: " + i + ": " +entity);
                      i++;
                       }


                  if (!attributeList.Contains((animalTypeData.AnimalTypeId, baseSpeed.Value, baseHearingRange.Value, baseVisionRange.Value, entity)))
                  {
                      attributeList.Add((animalTypeData.AnimalTypeId, baseSpeed.Value, baseHearingRange.Value, baseVisionRange.Value, entity));
                  }    

              }).Run();*/


        if (currentTime > sampleTime)
        {

            Entities.WithoutBurst().
                ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseSpeed baseSpeed
                ) =>
                {
                    if (animalTypeData.AnimalName.Equals("Fox"))
                    {
                        lionCount++;
                        lionStat += baseSpeed.Value;
                    }
                    if (animalTypeData.AnimalName.Equals("Rabbit"))
                    {
                        chickenCount++;
                        chickenStat += baseSpeed.Value;
                    }
                }).Run();

                    lionStat = lionStat / lionCount;
                    chickenStat = chickenStat / chickenCount;
            addRecord(Mathf.RoundToInt(systemAge).ToString(), lionCount.ToString(), lionStat.ToString(), chickenCount.ToString(), chickenStat.ToString(), speedPath);
            reset();

            Entities.WithoutBurst().
                ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseHearingRange baseHearingRange
                ) =>
                {
                    if (animalTypeData.AnimalName.Equals("Fox"))
                    {
                        lionCount++;
                        lionStat += baseHearingRange.Value;
                    }
                    if (animalTypeData.AnimalName.Equals("Rabbit"))
                    {
                        chickenCount++;
                        chickenStat += baseHearingRange.Value;
                    }
                }).Run();

            lionStat = lionStat / lionCount;
            chickenStat = chickenStat / chickenCount;
            addRecord(Mathf.RoundToInt(systemAge).ToString(), lionCount.ToString(), lionStat.ToString(), chickenCount.ToString(), chickenStat.ToString(), hearingPath);
            reset();

            Entities.WithoutBurst().
                ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseVisionRange baseVisionRange
                ) =>
                {
                    if (animalTypeData.AnimalName.Equals("Fox"))
                    {
                        lionCount++;
                        lionStat += baseVisionRange.Value;
                    }
                    if (animalTypeData.AnimalName.Equals("Rabbit"))
                    {
                        chickenCount++;
                        chickenStat += baseVisionRange.Value;
                    }
                }).Run();

            lionStat = lionStat / lionCount;
            chickenStat = chickenStat / chickenCount;
            addRecord(Mathf.RoundToInt(systemAge).ToString(), lionCount.ToString(), lionStat.ToString(), chickenCount.ToString(), chickenStat.ToString(), visionPath);
            reset();
            currentTime = 0;
        }
        systemAge += Time.DeltaTime;
        currentTime += Time.DeltaTime;


      /*  Entities.WithoutBurst().
            ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseSpeed basespeed
            ) =>
            {
                if (i < 6)
                {
                    Debug.Log("#2: " + i + ": " + entity);
                }

            }).Run();



        Entities.WithoutBurst().
            ForEach((Entity entity, in AnimalTypeData animalTypeData, in BaseSpeed basespeed
            ) =>
            {
                if (i < 6)
                {   
                    Debug.Log("#2: " + i + ": " + entity);
                }

            }).Run();*/

    }

    private void addRecord(string time, string lionCount, string lionStat, string chickenCount, string chickenStat, string path)
    {
        //string path = "/Scripts/Graphs/GraphValues.csv";

        StreamWriter file = new StreamWriter(Application.dataPath + path, true);
        string line;
        if (path == speedPath)
            line = String.Format("{0},{1},{2},{3},{4}", time, lionCount, lionStat, chickenCount, chickenStat);
        else { line = String.Format("{0},{1},{2}", time, lionStat, chickenStat); }
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

   /* private int getChickenID()
    {
        return chicken.GetInstanceID();
    }

    private int getLionID()
    {
        return lion.GetInstanceID();
    }
    */

    private void reset()
    {
        lionStat = 0;
        lionCount = 0;
        chickenCount = 0;
        chickenStat = 0;
    }
    


}


