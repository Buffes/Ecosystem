using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelsData
{
    public LevelData[] levels;
    [Serializable]
    public class LevelData {
        public int number;
        public int[] path;
    }
}
