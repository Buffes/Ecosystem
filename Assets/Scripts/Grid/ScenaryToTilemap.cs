using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.Grid 
{
    public class ScenaryToTilemap
    {
        //Matrix with all GameObjects in the Grid
        private Scenary [,] gameObjectsInGrid;
        private int [,] tiles;

        private float greenScenaryNeigbourRate = 0.15f;
        private float greenScenarySpawnRate = 0.05f;
        private float desertScenarySpawnRate = 0.1f;

        private GameObject bush1;
        private GameObject bush2;
        private GameObject bush3;
        private GameObject bush4;
        private GameObject bush5;
        private GameObject bush6;
        private GameObject tree1;
        private GameObject tree2;
        private GameObject tree3;
        private GameObject tree4;
        private GameObject tree5;
        private GameObject tree6;
        private GameObject tree7;
        private GameObject tree8;
        private GameObject tree9;
        private GameObject tree10;
        private GameObject tree11;
        private GameObject tree12;
        private GameObject tree13;
        private GameObject tree14;
        private GameObject tree15;
        private GameObject tree16;
        private GameObject tree17;
        private GameObject tree18;
        private GameObject tree19;
        private GameObject tree20;
        private GameObject tree21;
        private GameObject tree22;
        private GameObject tree23;
        private GameObject tree24;
        private GameObject tree25;
        private GameObject tree26;
        private GameObject tree27;
        private GameObject tree28;
        private GameObject tree29;
        private GameObject tree30;
        private GameObject rock1;
        private GameObject rock2;
        private GameObject rock3;
        private GameObject rock4;
        private GameObject rock5;
        private GameObject rock6;
        private GameObject rock7;
        private GameObject rock8;
        private GameObject rock9;
        private GameObject rock10;
        private GameObject rock11;
        private GameObject cactus1;

        private SortedDictionary<Scenary, int> sortedScenary = new SortedDictionary<Scenary, int>();


        public ScenaryToTilemap()
        {
            this.tiles = GameZone.tiles;

            InitObjects();
            CreateEmptyScenary();
            SetupGameObjects();
        }

        private void InitObjects()
        {
            gameObjectsInGrid = new Scenary [tiles.GetLength(0), tiles.GetLength(1)];

            bush1 = Resources.Load("bush1") as GameObject;
            bush2 = Resources.Load("bush2") as GameObject;
            bush3 = Resources.Load("bush3") as GameObject;
            bush4 = Resources.Load("bush4") as GameObject;
            bush5 = Resources.Load("bush5") as GameObject;
            bush6 = Resources.Load("bush6") as GameObject;
            tree1 = Resources.Load("tree1") as GameObject;
            tree2 = Resources.Load("tree2") as GameObject;
            tree3 = Resources.Load("tree3") as GameObject;
            tree4 = Resources.Load("tree4") as GameObject;
            tree5 = Resources.Load("tree5") as GameObject;
            tree6 = Resources.Load("tree6") as GameObject;
            tree7 = Resources.Load("tree7") as GameObject;
            tree8 = Resources.Load("tree8") as GameObject;
            tree9 = Resources.Load("tree9") as GameObject;
            tree10 = Resources.Load("tree10") as GameObject;
            tree11 = Resources.Load("tree11") as GameObject;
            tree12 = Resources.Load("tree12") as GameObject;
            tree13 = Resources.Load("tree13") as GameObject;
            tree14 = Resources.Load("tree14") as GameObject;
            tree15 = Resources.Load("tree15") as GameObject;
            tree16 = Resources.Load("tree16") as GameObject;
            tree17 = Resources.Load("tree17") as GameObject;
            tree18 = Resources.Load("tree18") as GameObject;
            tree19 = Resources.Load("tree19") as GameObject;
            tree20 = Resources.Load("tree20") as GameObject;
            tree21 = Resources.Load("tree21") as GameObject;
            tree22 = Resources.Load("tree22") as GameObject;
            tree23 = Resources.Load("tree23") as GameObject;
            tree24 = Resources.Load("tree24") as GameObject;
            tree25 = Resources.Load("tree25") as GameObject;
            tree26 = Resources.Load("tree26") as GameObject;
            tree27 = Resources.Load("tree27") as GameObject;
            tree28 = Resources.Load("tree28") as GameObject;
            tree29 = Resources.Load("tree29") as GameObject;
            tree30 = Resources.Load("tree30") as GameObject;
            rock1 = Resources.Load("rock1") as GameObject;
            rock2 = Resources.Load("rock2") as GameObject;
            rock3 = Resources.Load("rock3") as GameObject;
            rock4 = Resources.Load("rock4") as GameObject;
            rock5 = Resources.Load("rock5") as GameObject;
            rock6 = Resources.Load("rock6") as GameObject;
            rock7 = Resources.Load("rock7") as GameObject;
            rock8 = Resources.Load("rock8") as GameObject;
            rock9 = Resources.Load("rock9") as GameObject;
            rock10 = Resources.Load("rock10") as GameObject;
            rock11 = Resources.Load("rock11") as GameObject;
            cactus1 = Resources.Load("cactus1") as GameObject;
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
                GameObject.Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Bush;
            }
            else if (rand <= newTree)
            {
                GameObject ob = GetRandomTree();
                GameObject.Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Tree;
            }
            else if (rand <= newRock) 
            {
                GameObject ob = GetRandomRock();
                GameObject.Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
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
                GameObject.Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Rock;
            }
            else if (rand <= value)
            {
                GameObject.Instantiate (cactus1, new Vector3(row, 0, col), Quaternion.identity);
                GameZone.walkableTiles[row, col] = false;
                return Scenary.Cactus;
            }
            
            return Scenary.Empty;
        }

        private GameObject GetRandomBush()
        {
            float rand = Random.value;
        
            if (rand <= 1f/6)
            {
                return bush1;
            }
            else if (rand <= 2f/6) 
            {
                return bush2;
            }
            else if (rand <= 3f/6) 
            {
                return bush3;
            }
            else if (rand <= 4f/6) 
            {
                return bush4;
            }
            else if (rand <= 5f/6) 
            {
                return bush5;
            }
            else 
            {
                return bush6;
            }
        }

        private GameObject GetRandomTree()
        {
            float rand = Random.value;

            if (rand <= 1f/30)
            {
                return tree1;
            }
            else if (rand <= 2f/30) 
            {
                return tree2;
            }
            else if (rand <= 3f/30) 
            {
                return tree3;
            }
            else if (rand <= 4f/30) 
            {
                return tree4;
            }
            else if (rand <= 5f/30) 
            {
                return tree5;
            }
            else if (rand <= 6f/30) 
            {
                return tree6;
            }
            else if (rand <= 7f/30) 
            {
                return tree7;
            }
            else if (rand <= 8f/30) 
            {
                return tree8;
            }
            else if (rand <= 9f/30) 
            {
                return tree9;
            }
            else if (rand <= 10f/30) 
            {
                return tree10;
            }
            else if (rand <= 11f/30) 
            {
                return tree11;
            }
            else if (rand <= 12f/30) 
            {
                return tree12;
            }
            else if (rand <= 13f/30)
            {
                return tree13;
            }
            else if (rand <= 14f/30) 
            {
                return tree14;
            }
            else if (rand <= 15f/30) 
            {
                return tree15;
            }
            else if (rand <= 16f/30) 
            {
                return tree16;
            }
            else if (rand <= 17f/30) 
            {
                return tree17;
            }
            else if (rand <= 18f/30) 
            {
                return tree18;
            }
            else if (rand <= 19f/30) 
            {
                return tree19;
            }
            else if (rand <= 20f/30) 
            {
                return tree20;
            }
            else if (rand <= 21f/30) 
            {
                return tree21;
            }
            else if (rand <= 22f/30) 
            {
                return tree22;
            }
            else if (rand <= 23f/30) 
            {
                return tree23;
            }
            else if (rand <= 24f/30)
            {
                return tree24;
            }
            else if (rand <= 25f/30) 
            {
                return tree25;
            }
            else if (rand <= 26f/30) 
            {
                return tree26;
            }
            else if (rand <= 27f/30) 
            {
                return tree27;
            }
            else if (rand <= 28f/30) 
            {
                return tree28;
            }
            else if (rand <= 29f/30) 
            {
                return tree29;
            }
            else
            {
                return tree30;
            }
           
        }

        private GameObject GetRandomRock()
        {
           float rand = Random.value;
        
            if (rand <= 1f/11)
            {
                return rock1;
            }
            else if (rand <= 2f/11) 
            {
                return rock2;
            }
            else if (rand <= 3f/11) 
            {
                return rock3;
            }
            else if (rand <= 4f/11) 
            {
                return rock4;
            }
            else if (rand <= 5f/11) 
            {
                return rock5;
            }
            else if (rand <= 6f/11) 
            {
                return rock6;
            }
            else if (rand <= 7f/11) 
            {
                return rock7;
            }
            else if (rand <= 8f/11) 
            {
                return rock8;
            }
            else if (rand <= 9f/11)
            {
                return rock9;
            }
            else if (rand <= 10f/11) 
            {
                return rock10;
            }
            else  
            {
                return rock11;
            }
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

