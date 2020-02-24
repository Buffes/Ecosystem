using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelsDataLoader : MonoBehaviour
{

    public Dictionary<int, LevelsData.LevelData> ReadLevelsData() {
        var file = Resources.Load("Levels.json", typeof(TextAsset)) as TextAsset;


        if (file == null) {
            throw new ApplicationException("Levels file is not accessable");
        }

        var levelsData = JsonUtility.FromJson<LevelsData>(file.text);

        return levelsData.levels.ToDictionary(level => level.number, level => level);

    }



}
