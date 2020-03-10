using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Grid 
{
    public class ScenaryToTilemap : MonoBehaviour
    {
        //Matrix with all GameObjects in the Grid
        private Scenary [,] gameObjectsInGrid;
        private int [,] tiles;

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

            gameObjectsInGrid = new Scenary [tiles.GetLength(0), tiles.GetLength(1)];
            CreateEmptyScenary();
            SetupGameObjects();
        }

        private void SetupGameObjects()
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
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
            }
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

        private Scenary RandomizeGreenScenary(float value, int row, int col)
        {
            float newBush = value / 3;
            float newTree = (value / 3) * 2;
            float newRock = value;
            float rand = Random.value;
            
            if (rand <= newBush)
            {
                GameObject ob = GetRandomBush();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Bush;
            }
            else if (rand <= newTree)
            {
                GameObject ob = GetRandomTree();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Tree;
            }
            else if (rand <= newRock) 
            {
                GameObject ob = GetRandomRock();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Rock;
            }
            
            return Scenary.Empty;
        }

        private Scenary RandomizeDesertScenary(float value, int row, int col) 
        {
            float newRock = value / 2;
            float rand = Random.value;
            
            if (rand <= newRock)
            {
                GameObject ob = GetRandomRock();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Rock;
            }
            else if (rand <= value)
            {
                GameObject ob = GetRandomCactus();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Cactus;
            }
            
            return Scenary.Empty;
        }

        private GameObject GetRandomBush()
        {
            int rand = Random.Range(0, bushes.Count-1);
            return bushes[rand];
        }

        private GameObject GetRandomTree()
        {
            int rand = Random.Range(0, trees.Count-1);
            return trees[rand];
        }

        private GameObject GetRandomRock()
        {
            int rand = Random.Range(0, rocks.Count-1);
            return rocks[rand];
        }

        private GameObject GetRandomCactus()
        {
            int rand = Random.Range(0, cacti.Count-1);
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

