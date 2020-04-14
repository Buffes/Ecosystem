using Ecosystem.Gameplay;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using UnityEngine;

public class WriteToCSVFile : MonoBehaviour
{
    public static AnimalType chicken;
    public static AnimalType lion;
    public  float sampleTime = 5f;
    private float currentTime = 0f;
    private float systemAge = 0f;
    

    private void Awake()
    {
        clear();
    }

   /* private void Update()
    {
        int nrChickens = 0;
        int nrLions = 0;
        float lionHearingRange = 0f;
        float lionVisionRange = 0f;
        float lionSpeed = 0f;
        float chickenHearingRange = 0f;
        float chickenVisionRange = 0f;
        float chickenSpeed = 0f;

        List<(int, float, float, float, Entity)> attributeList = WriteToCSVSystem.attributeList;

        if (currentTime > sampleTime)
        {
            for( int i = 0; i < attributeList.Count; i++)
            {

                if (attributeList[i].Item1 == getLionID())
                {
                    nrLions++;
                    lionSpeed += attributeList[i].Item2;
                    lionHearingRange += attributeList[i].Item3;
                    lionVisionRange += attributeList[i].Item4;
                }

                if (attributeList[i].Item1 == getChickenID())
                {
                    nrChickens++;
                    chickenSpeed += attributeList[i].Item2;
                    chickenHearingRange += attributeList[i].Item3;
                    chickenVisionRange += attributeList[i].Item4;
                }
            }

            chickenSpeed = chickenSpeed / nrChickens;
            chickenHearingRange = chickenHearingRange / nrChickens;
            chickenVisionRange = chickenVisionRange / nrChickens;
            lionSpeed = lionSpeed / nrLions;
            lionHearingRange = lionHearingRange / nrLions;
            lionVisionRange = lionVisionRange / nrLions;

            addRecord(Mathf.RoundToInt(systemAge).ToString(), nrChickens.ToString(), nrLions.ToString(), Mathf.RoundToInt(chickenSpeed).ToString(),
                Mathf.RoundToInt(chickenHearingRange).ToString(), Mathf.RoundToInt(chickenVisionRange).ToString(), Mathf.RoundToInt(lionSpeed).ToString(),
                Mathf.RoundToInt(lionHearingRange).ToString(), Mathf.RoundToInt(lionVisionRange).ToString());
            currentTime = 0f;
        }
        systemAge += Time.deltaTime;
        currentTime += Time.deltaTime;

    }*/

    public static void addRecord(string time, string nrChickens, string nrLions, string chickenSpeed, string chickenHearingRange,
        string chickenVisionRange, string lionSpeed, string lionHearingRange, string lionVisionRange)
    {
        string path = "/Scripts/Graphs/GraphValues.csv";

        StreamWriter file = new StreamWriter(Application.dataPath + path, true);

        var line = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", time, nrChickens, nrLions, chickenSpeed, chickenHearingRange, chickenVisionRange, lionSpeed, lionHearingRange, lionVisionRange);
        file.WriteLine(line);
        file.Close();
    }

    void clear()
    {
        string valuesPath = "/Scripts/Graphs/GraphValues.csv";
        StreamWriter valuesFile = new StreamWriter(Application.dataPath + valuesPath, false);
        valuesFile.Close();

    }

    private int getChickenID()
    {
        return chicken.GetInstanceID();
    }

    private int getLionID()
    {
        return lion.GetInstanceID();
    }

}
