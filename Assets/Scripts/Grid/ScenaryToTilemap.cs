﻿using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Grid 
{
    public class ScenaryToTilemap : MonoBehaviour
    {
        [SerializeField] private GameZone gameZone = default;

        //Matrix with all GameObjects in the Grid
        private Scenary [,] gameObjectsInGrid;
        private int [,] tiles;
        private List<Vector2> poissonPoints;

        [Header("Poisson Disc Sampling")]
        [Range(0.1f, 15f)]
        public float min_radius;
        [Range(1f, 15f)]
        public float max_radius;
        [Range(1, 50)]
        public int numSamplesBeforeRejection = 20;
        public int numToSpawn;

        public float greenScenaryNeigbourRate = 0.15f;
        public float greenScenarySpawnRate = 0.05f;
        public float desertScenarySpawnRate = 0.1f;

        public List<GameObject> bushes = new List<GameObject>();
        public List<GameObject> trees = new List<GameObject>();
        public List<GameObject> rocks = new List<GameObject>();
        public List<GameObject> cacti = new List<GameObject>();


        private SortedDictionary<Scenary, int> sortedScenary = new SortedDictionary<Scenary, int>();

        void Start()
        {
            this.tiles = GameZone.tiles;
            this.poissonPoints = PoissonDiscSampling.GeneratePoisson(GameZone.NoiseMap,
                                                                     new Vector2(tiles.GetLength(0), tiles.GetLength(1)),
                                                                     numSamplesBeforeRejection,
                                                                     min_radius,
                                                                     max_radius,
                                                                     numToSpawn,
                                                                     GameZone.WaterSurface);

            gameObjectsInGrid = new Scenary [tiles.GetLength(0), tiles.GetLength(1)];
            CreateEmptyScenary();
            SetupGameObjects();
        }

        private void SetupGameObjects()
        {
           if (poissonPoints != null)
            {
                foreach(Vector2 point in poissonPoints)
                {
                    int row = (int)point.x;
                    int col = (int)point.y;
                    if (tiles[row, col] == 34)
                    {
                        gameObjectsInGrid[row, col] = RandomizeDesertScenary(1, row, col);
                    }
                    else if (tiles[row, col] == 51)
                    {
                        gameObjectsInGrid[row, col] = RandomizeGreenScenary(1, row, col);
                    }
                }
            }
            
            /*for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col] == 34)
                    {
                        //Scenary in the desert does not spawn more often if there are other things closeby
                        gameObjectsInGrid[row, col] = RandomizeDesertScenary(desertScenarySpawnRate, row, col);
                    }
                    else if (tiles[row, col] == 51)
                    {
                        if (ScenaryAsNeighbour(row, col) != Scenary.Empty)
                        {
                            gameObjectsInGrid[row, col] = RandomizeGreenScenary(greenScenaryNeigbourRate, row, col);
                        }
                        else 
                        {
                            gameObjectsInGrid[row, col] = RandomizeGreenScenary(greenScenarySpawnRate, row, col);
                        }
                    }
                }
            }*/
        }

        private Scenary ScenaryAsNeighbour(int row, int col)
        {
            IncrementValue(CheckScenary(row-1, col-1));
            IncrementValue(CheckScenary(row-1, col));
            IncrementValue(CheckScenary(row-1, col+1));
            IncrementValue(CheckScenary(row, col-1));

            Scenary largestCount = Scenary.Empty;
            int count = 0;

            foreach (var v in sortedScenary)
            {
                if (v.Value > count && v.Key != Scenary.Empty)
                {
                    count = v.Value;
                    largestCount = v.Key;
                }
            }
            
            return largestCount;
        }

        private Scenary CheckScenary(int row, int col)
        {
            if (row < 0 || row >= tiles.GetLength(0) || col < 0 || col >= tiles.GetLength(1)){
                return Scenary.Empty;
            }
            return gameObjectsInGrid[row, col];
        }

        private Quaternion RandomQuaternion()
        {
            return Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }

        private Scenary SpawnRandomTree(int row, int col)
        {
            Vector3 spawnPos = gameZone.GetWorldPosition(row, col);
            Instantiate(GetRandomTree(), spawnPos, RandomQuaternion());
            return Scenary.Tree;
        }

        private Scenary RandomizeGreenScenary(float value, int row, int col)
        {
            float newBush = (value / 5) * 2;
            float newTree = (value / 5) * 4;
            float newRock = value;
            float rand = Random.value;

            Vector3 spawnPos = gameZone.GetWorldPosition(row, col);
            spawnPos.y = GameZone.GetGroundLevel(row, col);

            if (rand <= newBush)
            {
                Instantiate (GetRandomBush(), spawnPos, RandomQuaternion());
                return Scenary.Bush;
            }
            else if (rand <= newTree)
            {
                Instantiate (GetRandomTree(), spawnPos, RandomQuaternion());
                return Scenary.Tree;
            }
            else if (rand <= newRock) 
            {
                Instantiate (GetRandomRock(), spawnPos, RandomQuaternion());
                return Scenary.Rock;
            }
            
            return Scenary.Empty;
        }

        private Scenary RandomizeDesertScenary(float value, int row, int col) 
        {
            float newRock = value / 2;
            float rand = Random.value;

            Vector3 spawnPos = gameZone.GetWorldPosition(row, col);
            spawnPos.y = GameZone.NoiseMap[row, col];

            if (rand <= newRock)
            {
                Instantiate (GetRandomRock(), spawnPos, RandomQuaternion());
                return Scenary.Rock;
            }
            else if (rand <= value)
            {
                Instantiate (GetRandomCactus(), spawnPos, RandomQuaternion());
                return Scenary.Cactus;
            }
            
            return Scenary.Empty;
        }

        private GameObject GetRandomBush()
        {
            int rand = Random.Range(0, bushes.Count);
            return bushes[rand];
        }

        private GameObject GetRandomTree()
        {
            int rand = Random.Range(0, trees.Count);
            return trees[rand];
        }

        private GameObject GetRandomRock()
        {
            int rand = Random.Range(0, rocks.Count);
            return rocks[rand];
        }

        private GameObject GetRandomCactus()
        {
            int rand = Random.Range(0, cacti.Count);
            return cacti[rand];
        }

        private void CreateEmptyScenary()
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    gameObjectsInGrid[row, col] = Scenary.Empty;
                }
            }
        }

        private void IncrementValue(Scenary scenary)
        {
            if (!sortedScenary.ContainsKey(scenary))
            {
                sortedScenary.Add(scenary,1);
                return;
            }
            sortedScenary[scenary]++;
        }

    }
}

