using Ecosystem.Gameplay;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using UnityEngine;

public class WriteToCSVFile : MonoBehaviour
{
    public AnimalType chicken;
    public AnimalType lion;
    public  float sampleTime = 5f;
    private float currentTime = 0f;
    private float systemAge = 0f;
    

    private void Awake()
    {
        clear();
    }

    private void Update()
    {
        int nrChickens = 0;
        int nrLions = 0;

        List<(int, Entity)> list = WriteToCSVSystem.list;

        if (currentTime > sampleTime)
        {
            for( int i = 0; i < list.Count; i++)
            {

                if(list[i].Item1 == getLionID())
                    nrLions++;
                
                if (list[i].Item1 == getChickenID())
                    nrChickens++;
            }

            addRecord(Mathf.RoundToInt(systemAge).ToString(), nrChickens.ToString(), nrLions.ToString());
            currentTime = 0f;
        }
        systemAge += Time.deltaTime;
        currentTime += Time.deltaTime;

    }

    public static void addRecord(string time, string nrChickens, string nrLions)
    {
        string path = "/Scripts/Graphs/GraphValues.csv";

        StreamWriter file = new StreamWriter(Application.dataPath + path, true);

        var line = String.Format("{0},{1},{2}", time, nrChickens, nrLions);
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
