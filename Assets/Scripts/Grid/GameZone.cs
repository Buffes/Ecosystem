using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

namespace Ecosystem.Grid
{
    public class GameZone : MonoBehaviour 
    {
        //Tiles with assets in the Grid, and the size of the Grid
        public int[,] tiles = new int[99,99];

        //The numbers of the shallow and beach tiles
        private int waterIndex = 17;
        private int landIndex = 34;
        
        //The difference between water and land
        private int diffWaterLand = 0;

        //The rate of chance for spawning an object if there is a similar object as neighbour
        private float waterNeighbourRate = 0.65f;
        private float greenScenaryNeigbourRate = 0.15f;

        //The rate of objects spawning
        private float waterSpawnRate = 0.005f;
        private float greenScenarySpawnRate = 0.05f;
        private float desertScenarySpawnRate = 0.1f;

        //Matrix of walkable tiles
        private bool [,] walkableTiles;

        public GameObject bush1;
        public GameObject bush2;
        public GameObject bush3;
        public GameObject bush4;
        public GameObject bush5;
        public GameObject bush6;
        public GameObject bush7;
        public GameObject bush8;
        public GameObject bush9;
        public GameObject bush10;
        public GameObject bush11;
        public GameObject bush12;
        public GameObject tree1;
        public GameObject tree2;
        public GameObject tree3;
        public GameObject tree4;
        public GameObject tree5;
        public GameObject tree6;
        public GameObject tree7;
        public GameObject tree8;
        public GameObject tree9;
        public GameObject tree10;
        public GameObject tree11;
        public GameObject tree12;
        public GameObject tree13;
        public GameObject tree14;
        public GameObject tree15;
        public GameObject tree16;
        public GameObject tree17;
        public GameObject tree18;
        public GameObject tree19;
        public GameObject tree20;
        public GameObject tree21;
        public GameObject tree22;
        public GameObject tree23;
        public GameObject tree24;
        public GameObject tree25;
        public GameObject tree26;
        public GameObject tree27;
        public GameObject tree28;
        public GameObject tree29;
        public GameObject tree30;
        public GameObject tree31;
        public GameObject tree32;
        public GameObject tree33;
        public GameObject tree34;
        public GameObject tree35;
        public GameObject tree36;
        public GameObject tree37;
        public GameObject tree38;
        public GameObject tree39;
        public GameObject tree40;
        public GameObject tree41;
        public GameObject tree42;
        public GameObject tree43;
        public GameObject tree44;
        public GameObject tree45;
        public GameObject tree46;
        public GameObject tree47;
        public GameObject tree48;
        public GameObject tree49;
        public GameObject tree50;
        public GameObject rock1;
        public GameObject rock2;
        public GameObject rock3;
        public GameObject rock4;
        public GameObject rock5;
        public GameObject rock6;
        public GameObject rock7;
        public GameObject rock8;
        public GameObject rock9;
        public GameObject rock10;
        public GameObject rock11;
        public GameObject rock12;
        public GameObject cactus1;
        public GameObject cactus4;
        public GameObject cactus5;

        //Matrix with all GameObjects in the Grid
        private Scenary [,] gameObjectsInGrid;

        // Start is called before the first frame update
        void Start() 
        {
            InitObjects();
            RandomizeStartGrid();
            CheckCorners();
            CheckEdges();
            CheckMiddle();
            SetupTilemap();
            SetupWalkableTiles();
            SetupGameObjects();
            PassWalkableTilesToSystems();
        }

        private void InitObjects()
        {
            diffWaterLand = landIndex - waterIndex;
            walkableTiles = new bool [tiles.GetLength(0), tiles.GetLength(1)];
            gameObjectsInGrid = new Scenary [tiles.GetLength(0), tiles.GetLength(1)];

            bush1 = Resources.Load("bush1") as GameObject;
            bush2 = Resources.Load("bush2") as GameObject;
            bush3 = Resources.Load("bush3") as GameObject;
            bush4 = Resources.Load("bush4") as GameObject;
            bush5 = Resources.Load("bush5") as GameObject;
            bush6 = Resources.Load("bush6") as GameObject;
            bush7 = Resources.Load("bush7") as GameObject;
            bush8 = Resources.Load("bush8") as GameObject;
            bush9 = Resources.Load("bush9") as GameObject;
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
            rock12 = Resources.Load("rock12") as GameObject;
            cactus1 = Resources.Load("cactus1") as GameObject;
        }

        private void RandomizeStartGrid()
        {
            for (int i = 0; i < tiles.GetLength(0); i += 2 )
            {
                for (int j = 0; j < tiles.GetLength(1); j += 2 )
                {
                    if ((i - 2 >= 0 && tiles[i - 2,j] == waterIndex) || (j - 2 >= 0 && tiles[i, j - 2] == waterIndex))
                    {
                        tiles[i,j] = RandomizeWater(waterNeighbourRate);
                    } else 
                    {
                        tiles[i,j] = RandomizeWater(waterSpawnRate);
                    }
                }
            }
        }

        //The probability of creating water, otherwise create grass. 
        private int RandomizeWater(float water)
        {
            float rand = UnityEngine.Random.value;
            if (rand <= water)
            {
                return waterIndex;
            }
            return landIndex;
        }

        private void CheckCorners()
        {
            if (tiles[0,0] >= landIndex && tiles[0,2] >= landIndex && 
                tiles[2,0] >= landIndex && tiles[2,2] >= landIndex)
            {
                tiles[0,0] = 51;
            } 
            else if (tiles[0,0] <= waterIndex && tiles[0,2] <= waterIndex && 
                    tiles[2,0] <= waterIndex && tiles[2,2] <= waterIndex)
            {
                tiles[0,0] = 0;
            }
            
            if (tiles[0,tiles.GetLength(1)-3] >= landIndex && tiles[0,tiles.GetLength(1)-1] >= landIndex && 
                tiles[2,tiles.GetLength(1)-3] >= landIndex && tiles[2,tiles.GetLength(1)-1] >= landIndex)
            {
                tiles[0,tiles.GetLength(1)-1] = 51;
            }
            else if (tiles[0,tiles.GetLength(1)-3] <= waterIndex && tiles[0,tiles.GetLength(1)-1] <= waterIndex && 
                    tiles[2,tiles.GetLength(1)-3] <= waterIndex && tiles[2,tiles.GetLength(1)-1] <= waterIndex)
            {
                tiles[0,tiles.GetLength(1)-1] = 0;
            }

            if (tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-3] >= landIndex && tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-1] >= landIndex && 
                tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-3] >= landIndex && tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] >= landIndex)
            {
                tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] = 51;
            }
            else if(tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-3] <= waterIndex && tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-1] <= waterIndex && 
                    tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-3] <= waterIndex && tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] <= waterIndex)
            {
                tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] = 0;
            }

            if (tiles[tiles.GetLength(0)-1, 0] >= landIndex && tiles[tiles.GetLength(0)-3, 0] >= landIndex && 
                tiles[tiles.GetLength(0)-1, 2] >= landIndex && tiles[tiles.GetLength(0)-3, 2] >= landIndex)
            {
                tiles[tiles.GetLength(0)-1, 0] = 51;
            }
            else if (tiles[tiles.GetLength(0)-1, 0] <= waterIndex && tiles[tiles.GetLength(0)-3, 0] <= waterIndex && 
                    tiles[tiles.GetLength(0)-1, 2] <= waterIndex && tiles[tiles.GetLength(0)-3, 2] <= waterIndex) 
            {
                tiles[tiles.GetLength(0)-1, 0] = 0;
            }
        }

        private void CheckEdges()
        {
            CheckUpperEdges();
            CheckLowerEdges();
            CheckRightEdges();
            CheckLeftEdges();
        }

        private void CheckUpperEdges()
        {
            for(int i = 2; i < tiles.GetLength(1)-2; i += 2)
            {
                if(tiles[0,i] >= landIndex && tiles[0,i-2] >= landIndex && tiles[0,i+2] >= landIndex && 
                tiles[2,i] >= landIndex && tiles[2,i-2] >= landIndex && tiles[2,i+2] >= landIndex)
                {
                    tiles[0,i] = 51;
                }

                else if(tiles[0,i] <= waterIndex && tiles[0,i-2] <= waterIndex && tiles[0,i+2] <= waterIndex && 
                        tiles[2,i] <= waterIndex && tiles[2,i-2] <= waterIndex && tiles[2,i+2] <= waterIndex)
                {
                    tiles[0,i] = 0;
                }
            }

            for (int i = 1; i < tiles.GetLength(0)-1; i += 2)
            {
                FillHorizontalTiles(0, i);
            }
        }

        private void CheckLowerEdges()
        {
            for(int i = 2; i < tiles.GetLength(1)-2; i += 2)
            {
                if(tiles[tiles.GetLength(0)-1,i] >= landIndex && tiles[tiles.GetLength(0)-1,i-2] >= landIndex && tiles[tiles.GetLength(0)-1,i+2] >= landIndex && 
                tiles[tiles.GetLength(0)-3,i] >= landIndex && tiles[tiles.GetLength(0)-3,i-2] >= landIndex && tiles[tiles.GetLength(0)-3,i+2] >= landIndex)
                {
                    tiles[tiles.GetLength(0)-1,i] = 51;
                }
                else if(tiles[tiles.GetLength(0)-1,i] <= waterIndex && tiles[tiles.GetLength(0)-1,i-2] <= waterIndex && tiles[tiles.GetLength(0)-1,i+2] <= waterIndex && 
                        tiles[tiles.GetLength(0)-3,i] <= waterIndex && tiles[tiles.GetLength(0)-3,i-2] <= waterIndex && tiles[tiles.GetLength(0)-3,i+2] <= waterIndex)
                {
                    tiles[tiles.GetLength(0)-1,i] = 0;
                }
            }

            for (int i = 1; i < tiles.GetLength(0)-1; i += 2)
            {
                FillHorizontalTiles(tiles.GetLength(0)-1, i);
            }
        }

        private void CheckRightEdges()
        {
            for(int i = 2; i < tiles.GetLength(0)-2; i += 2)
            {
                if(tiles[i-2,tiles.GetLength(1)-1] >= landIndex && tiles[i,tiles.GetLength(1)-1] >= landIndex && tiles[i+2,tiles.GetLength(1)-1] >= landIndex && 
                tiles[i-2,tiles.GetLength(1)-3] >= landIndex && tiles[i,tiles.GetLength(1)-3] >= landIndex && tiles[i+2,tiles.GetLength(1)-3] >= landIndex)
                {
                    tiles[i, tiles.GetLength(1)-1] = 51;
                }
                else if(tiles[i-2,tiles.GetLength(1)-1] <= waterIndex && tiles[i,tiles.GetLength(1)-1] <= waterIndex && tiles[i+2,tiles.GetLength(1)-1] <= waterIndex && 
                        tiles[i-2,tiles.GetLength(1)-3] <= waterIndex && tiles[i,tiles.GetLength(1)-3] <= waterIndex && tiles[i+2,tiles.GetLength(1)-3] <= waterIndex)
                {
                    tiles[i, tiles.GetLength(1)-1] = 0;
                }
            }

            for (int i = 1; i < tiles.GetLength(0)-1; i += 2)
            {
                FillVerticalTiles(i, tiles.GetLength(0)-1);
            }
        }

        private void CheckLeftEdges()
        {
            for(int i = 2; i < tiles.GetLength(0)-2; i += 2)
            {
                if(tiles[i-2,0] >= landIndex && tiles[i,0] >= landIndex && tiles[i+2,0] >= landIndex && 
                tiles[i-2,2] >= landIndex && tiles[i,2] >= landIndex && tiles[i+2,2] >= landIndex)
                {
                    tiles[i,0] = 51;
                }
                else if(tiles[i-2,0] <= waterIndex && tiles[i,0] <= waterIndex && tiles[i+2,0] <= waterIndex && 
                        tiles[i-2,2] <= waterIndex && tiles[i,2] <= waterIndex && tiles[i+2,2] <= waterIndex)
                {
                    tiles[i,0] = 0;
                }
            }

            for (int i = 1; i < tiles.GetLength(0)-1; i += 2)
            {
                FillVerticalTiles(i, 0);
            }
        }

        private void CheckMiddle()
        {
            for (int i = 2; i < tiles.GetLength(0) - 1; i += 2)
            {
                for (int j = 2; j < tiles.GetLength(1) - 1; j += 2) 
                { 
                    if (tiles[i-2,j-2] >= landIndex && tiles[i-2,j] >= landIndex && tiles[i-2,j+2] >= landIndex && 
                        tiles[i,  j-2] >= landIndex && tiles[i,  j] >= landIndex && tiles[i,  j+2] >= landIndex && 
                        tiles[i+2,j-2] >= landIndex && tiles[i+2,j] >= landIndex && tiles[i+2,j+2] >= landIndex) 
                    {
                        tiles[i,j] = 51;
                    }
                    else if (tiles[i-2,j-2] <= waterIndex && tiles[i-2,j] <= waterIndex && tiles[i-2,j+2] <= waterIndex && 
                        tiles[i,  j-2] <= waterIndex && tiles[i,  j] <= waterIndex && tiles[i,  j+2] <= waterIndex && 
                        tiles[i+2,j-2] <= waterIndex && tiles[i+2,j] <= waterIndex && tiles[i+2,j+2] <= waterIndex)
                    {
                        tiles[i,j] = 0;
                    }
                }
            }

            for (int i = 2; i < tiles.GetLength(0) - 1; i += 2)
            {
                for (int j = 1; j < tiles.GetLength(1) - 1; j +=2 )
                {
                    FillHorizontalTiles(i,j);
                }
            }

            for (int i = 2; i < tiles.GetLength(0) - 1; i += 2)
            {
                for (int j = 1; j < tiles.GetLength(1) - 1; j +=2 )
                {
                    FillVerticalTiles(j,i);
                }
            }

            for (int i = 1; i < tiles.GetLength(0) - 1; i += 2)
            {
                for (int j = 1; j < tiles.GetLength(1) - 1; j +=2 )
                {
                    FillMiddleTiles(i,j);
                }
            }
        }

        private void SetupTilemap() 
        {
            var tilemap = GetComponentInChildren<Tilemap>();

            var localTilePositions = new List<Vector3Int>();

            foreach(var pos in tilemap.cellBounds.allPositionsWithin) 
            {
                Vector3Int localPosition = new Vector3Int(pos.x, pos.y, pos.x);
                localTilePositions.Add(localPosition);
            }
            SetupTiles(localTilePositions, tilemap);
        }

        private void SetupTiles(List<Vector3Int> positions, Tilemap tilemap) 
        {
            var grassTile = TilesResourceLoader.GetGrassTile();
            var deepTile = TilesResourceLoader.GetDeepTile();
            var shallowTile = TilesResourceLoader.GetShallowTile();
            var beachTile = TilesResourceLoader.GetBeachTile();
            
            for(int i = 0; i < tiles.GetLength(0); i++) 
            {
                for(int j = 0; j < tiles.GetLength(1); j++) 
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);
                    int currentpos = tiles[i,j];
                    SetTileToTilemap(tilemap, pos, currentpos);
                }
            }
        }

        private void SetTileToTilemap (Tilemap tilemap, Vector3Int pos, int currentpos)
        {
            //Water: 0-16
            //Land: 17-51

            var deepTile = TilesResourceLoader.GetDeepTile();
            var shallowDeepCurvedOut0 = TilesResourceLoader.GetShallowDeepCurveOut0Tile();
            var shallowDeepCurvedOut1 = TilesResourceLoader.GetShallowDeepCurveOut1Tile();
            var shallowDeepCurvedOut2 = TilesResourceLoader.GetShallowDeepCurveOut2Tile();
            var shallowDeepCurvedOut3 = TilesResourceLoader.GetShallowDeepCurveOut3Tile();
            var shallowDeepCurvedIn0 = TilesResourceLoader.GetShallowDeepCurveIn0Tile();
            var shallowDeepCurvedIn1 = TilesResourceLoader.GetShallowDeepCurveIn1Tile();
            var shallowDeepCurvedIn2 = TilesResourceLoader.GetShallowDeepCurveIn2Tile();
            var shallowDeepCurvedIn3 = TilesResourceLoader.GetShallowDeepCurveIn3Tile();
            var shallowDeepHorizontal0 = TilesResourceLoader.GetShallowDeepHorizontal0Tile();
            var shallowDeepHorizontal1 = TilesResourceLoader.GetShallowDeepHorizontal1Tile();
            var shallowDeepHorizontal2 = TilesResourceLoader.GetShallowDeepHorizontal2Tile();
            var shallowDeepHorizontal3 = TilesResourceLoader.GetShallowDeepHorizontal3Tile();
            var shallowDeepStraight0 = TilesResourceLoader.GetShallowDeepStraight0Tile();
            var shallowDeepStraight1 = TilesResourceLoader.GetShallowDeepStraight1Tile();
            var shallowDeepStraight2 = TilesResourceLoader.GetShallowDeepStraight2Tile();
            var shallowDeepStraight3 = TilesResourceLoader.GetShallowDeepStraight3Tile();
            var shallowTile = TilesResourceLoader.GetShallowTile();
            var beachShallowCurvedOut0 = TilesResourceLoader.GetBeachShallowCurveOut0Tile();
            var beachShallowCurvedOut1 = TilesResourceLoader.GetBeachShallowCurveOut1Tile();
            var beachShallowCurvedOut2 = TilesResourceLoader.GetBeachShallowCurveOut2Tile();
            var beachShallowCurvedOut3 = TilesResourceLoader.GetBeachShallowCurveOut3Tile();
            var beachShallowCurvedIn0 = TilesResourceLoader.GetBeachShallowCurveIn0Tile();
            var beachShallowCurvedIn1 = TilesResourceLoader.GetBeachShallowCurveIn1Tile();
            var beachShallowCurvedIn2 = TilesResourceLoader.GetBeachShallowCurveIn2Tile();
            var beachShallowCurvedIn3 = TilesResourceLoader.GetBeachShallowCurveIn3Tile();
            var beachShallowHorizontal0 = TilesResourceLoader.GetBeachShallowHorizontal0Tile();
            var beachShallowHorizontal1 = TilesResourceLoader.GetBeachShallowHorizontal1Tile();
            var beachShallowHorizontal2 = TilesResourceLoader.GetBeachShallowHorizontal2Tile();
            var beachShallowHorizontal3 = TilesResourceLoader.GetBeachShallowHorizontal3Tile();
            var beachShallowStraight0 = TilesResourceLoader.GetBeachShallowStraight0Tile();
            var beachShallowStraight1 = TilesResourceLoader.GetBeachShallowStraight1Tile();
            var beachShallowStraight2 = TilesResourceLoader.GetBeachShallowStraight2Tile();
            var beachShallowStraight3 = TilesResourceLoader.GetBeachShallowStraight3Tile();
            var beachTile = TilesResourceLoader.GetBeachTile();
            var grassBeachCurvedOut0 = TilesResourceLoader.GetGrassBeachCurveOut0Tile();
            var grassBeachCurvedOut1 = TilesResourceLoader.GetGrassBeachCurveOut1Tile();
            var grassBeachCurvedOut2 = TilesResourceLoader.GetGrassBeachCurveOut2Tile();
            var grassBeachCurvedOut3 = TilesResourceLoader.GetGrassBeachCurveOut3Tile();
            var grassBeachCurvedIn0 = TilesResourceLoader.GetGrassBeachCurveIn0Tile();
            var grassBeachCurvedIn1 = TilesResourceLoader.GetGrassBeachCurveIn1Tile();
            var grassBeachCurvedIn2 = TilesResourceLoader.GetGrassBeachCurveIn2Tile();
            var grassBeachCurvedIn3 = TilesResourceLoader.GetGrassBeachCurveIn3Tile();
            var grassBeachHorizontal0 = TilesResourceLoader.GetGrassBeachHorizontal0Tile();
            var grassBeachHorizontal1 = TilesResourceLoader.GetGrassBeachHorizontal1Tile();
            var grassBeachHorizontal2 = TilesResourceLoader.GetGrassBeachHorizontal2Tile();
            var grassBeachHorizontal3 = TilesResourceLoader.GetGrassBeachHorizontal3Tile();
            var grassBeachStraight0 = TilesResourceLoader.GetGrassBeachStraight0Tile();
            var grassBeachStraight1 = TilesResourceLoader.GetGrassBeachStraight1Tile();
            var grassBeachStraight2 = TilesResourceLoader.GetGrassBeachStraight2Tile();
            var grassBeachStraight3 = TilesResourceLoader.GetGrassBeachStraight3Tile();
            var grassTile = TilesResourceLoader.GetGrassTile();

            switch (currentpos) 
            {
                case 0:
                    tilemap.SetTile(pos, deepTile);
                    break;
                case 1:
                    tilemap.SetTile(pos, shallowDeepCurvedOut0);
                    break;
                case 2:
                    tilemap.SetTile(pos, shallowDeepCurvedOut1);
                    break;
                case 3:
                    tilemap.SetTile(pos, shallowDeepCurvedOut2);
                    break;
                case 4:
                    tilemap.SetTile(pos, shallowDeepCurvedOut3);
                    break;
                case 5:
                    tilemap.SetTile(pos, shallowDeepCurvedIn0);
                    break;
                case 6:
                    tilemap.SetTile(pos, shallowDeepCurvedIn1);
                    break;
                case 7:
                    tilemap.SetTile(pos, shallowDeepCurvedIn2);
                    break;
                case 8:
                    tilemap.SetTile(pos, shallowDeepCurvedIn3);
                    break;
                case 9:
                    tilemap.SetTile(pos, shallowDeepStraight0);
                    break;
                case 10:
                    tilemap.SetTile(pos, shallowDeepStraight1);
                    break;
                case 11:
                    tilemap.SetTile(pos, shallowDeepStraight2);
                    break;
                case 12:
                    tilemap.SetTile(pos, shallowDeepStraight3);
                    break;
                case 13:
                    tilemap.SetTile(pos, shallowDeepHorizontal0);
                    break;
                case 14:
                    tilemap.SetTile(pos, shallowDeepHorizontal1);
                    break;
                case 15:
                    tilemap.SetTile(pos, shallowDeepHorizontal2);
                    break;
                case 16:
                    tilemap.SetTile(pos, shallowDeepHorizontal3);
                    break;
                case 17:
                    tilemap.SetTile(pos,shallowTile);
                    break;
                case 18:
                    tilemap.SetTile(pos, beachShallowCurvedOut0);
                    break;
                case 19:
                    tilemap.SetTile(pos, beachShallowCurvedOut1);
                    break;
                case 20:
                    tilemap.SetTile(pos, beachShallowCurvedOut2);
                    break;
                case 21:
                    tilemap.SetTile(pos, beachShallowCurvedOut3);
                    break;
                case 22:
                    tilemap.SetTile(pos, beachShallowCurvedIn0);
                    break;
                case 23:
                    tilemap.SetTile(pos, beachShallowCurvedIn1);
                    break;
                case 24:
                    tilemap.SetTile(pos, beachShallowCurvedIn2);
                    break;
                case 25:
                    tilemap.SetTile(pos, beachShallowCurvedIn3);
                    break;
                case 26:
                    tilemap.SetTile(pos, beachShallowStraight0);
                    break;
                case 27:
                    tilemap.SetTile(pos, beachShallowStraight1);
                    break;
                case 28:
                    tilemap.SetTile(pos, beachShallowStraight2);
                    break;
                case 29:
                    tilemap.SetTile(pos, beachShallowStraight3);
                    break;
                case 30:
                    tilemap.SetTile(pos, beachShallowHorizontal0);
                    break;
                case 31:
                    tilemap.SetTile(pos, beachShallowHorizontal1);
                    break;
                case 32:
                    tilemap.SetTile(pos, beachShallowHorizontal2);
                    break;
                case 33:
                    tilemap.SetTile(pos, beachShallowHorizontal3);
                    break;
                case 34:
                    tilemap.SetTile(pos, beachTile);
                    break;
                case 35:
                    tilemap.SetTile(pos, grassBeachCurvedOut0);
                    break;
                case 36:
                    tilemap.SetTile(pos, grassBeachCurvedOut1);
                    break;
                case 37:
                    tilemap.SetTile(pos, grassBeachCurvedOut2);
                    break;
                case 38:
                    tilemap.SetTile(pos, grassBeachCurvedOut3);
                    break;
                case 39:
                    tilemap.SetTile(pos, grassBeachCurvedIn0);
                    break;
                case 40:
                    tilemap.SetTile(pos, grassBeachCurvedIn1);
                    break;
                case 41:
                    tilemap.SetTile(pos, grassBeachCurvedIn2);
                    break;
                case 42:
                    tilemap.SetTile(pos, grassBeachCurvedIn3);
                    break;
                case 43:
                    tilemap.SetTile(pos, grassBeachStraight0);
                    break;
                case 44:
                    tilemap.SetTile(pos, grassBeachStraight1);
                    break;
                case 45:
                    tilemap.SetTile(pos, grassBeachStraight2);
                    break;
                case 46:
                    tilemap.SetTile(pos, grassBeachStraight3);
                    break;
                case 47:
                    tilemap.SetTile(pos, grassBeachHorizontal0);
                    break;
                case 48:
                    tilemap.SetTile(pos, grassBeachHorizontal1);
                    break;
                case 49:
                    tilemap.SetTile(pos, grassBeachHorizontal2);
                    break;
                case 50:
                    tilemap.SetTile(pos, grassBeachHorizontal3);
                    break;
                case 51:
                    tilemap.SetTile(pos, grassTile);
                    break;
            }
        }

        private void FillHorizontalTiles(int row, int col)
        {
            int prev = tiles[row,col-1];
            int next = tiles[row,col+1];
            if (prev == next)
            {
                tiles[row,col] = prev;
            } 

            else if (prev == 0 || next == 0)
            {
                if (prev - next == diffWaterLand){
                    tiles[row,col] = 12;
                } 
                else 
                {
                    tiles[row,col] = 10;
                }
            }

            else if (prev == waterIndex || next == waterIndex)
            {
                if (prev - next == diffWaterLand){
                    tiles[row,col] = 29;
                } 
                else 
                {
                    tiles[row,col] = 27;
                }
            }

            else if (prev == landIndex || next == landIndex)
            {
                if (prev - next == diffWaterLand){
                    tiles[row,col] = 46;
                } 
                else 
                {
                    tiles[row,col] = 44;
                }
            }

        }

        private void FillVerticalTiles(int row, int col)
        {
            int prev = tiles[row-1,col];
            int next = tiles[row+1,col];
            if (prev == next)
            {
                tiles[row,col] = prev;
            } 

            else if (prev == 0 || next == 0)
            {
                if (prev - next == diffWaterLand){
                    tiles[row,col] = 11;
                } 
                else 
                {
                    tiles[row,col] = 9;
                }
            }

            else if (prev == waterIndex || next == waterIndex)
            {
                if (prev - next == diffWaterLand){
                    tiles[row,col] = 28;
                } 
                else 
                {
                    tiles[row,col] = 26;
                }
            }

            else if (prev == landIndex || next == landIndex)
            {
                if (prev - next == diffWaterLand){
                    tiles[row,col] = 45;
                } 
                else 
                {
                    tiles[row,col] = 43;
                }
            }
        }

        private void FillAcrossTiles(int row, int col, int leftUp, int leftDown)
        {
            
            if (tiles[row-1,col] == 12)
            {
                tiles[row, col] = 16;
            }

            else if (tiles[row-1,col] == 10)
            {
                tiles[row, col] = 15;
            }
            
            else if(tiles[row-1,col] == 29){
                tiles[row, col] = 33;

            }

            else if (tiles[row-1,col] == 27)
            {
                tiles[row, col] = 32;
            }

            else if(tiles[row-1,col] == 46){
                tiles[row, col] = 50;

            }

            else if (tiles[row-1,col] == 44)
            {
                tiles[row, col] = 49;
            }
        }

        private void FillMiddleTiles(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int up = tiles[row-1, col];
            int rightUp = tiles[row-1, col+1];

            int left = tiles[row, col-1];
            int right =tiles[row, col+1];

            int leftDown = tiles[row+1, col-1];
            int down = tiles[row+1, col];
            int rightDown = tiles[row+1, col+1];

            int corners = leftUp + rightUp + leftDown + rightDown;

            if(new[] { leftUp, up, rightUp, left, right, leftDown, down, rightDown }.All(x => x == left))
            {
                tiles[row, col] = left;
            }

            else if (Mathf.Abs(left-right) == diffWaterLand) 
            {
                FillVerticalTiles(row, col);
            }

            else if (Mathf.Abs(up-down) == diffWaterLand)
            {
                FillHorizontalTiles(row, col);
            }

            else if (leftUp == rightDown && leftDown == rightUp)
            {
                FillAcrossTiles(row, col, leftUp, leftDown);
            }

            else 
            {
                switch (corners)
                {
                    case 187:
                        FillGrassCorner(row, col);
                        break;
                    case 153:
                        FillBeachGrassCorner(row, col);
                        break;
                    case 119:
                        FillBeachShallowCorner(row, col);
                        break;
                    case 85:
                        FillShallowBeachCorner(row, col);
                        break;
                    case 51:
                        FillShallowDeepCorner(row, col);
                        break;
                    case 17:
                        FillDeepCorner(row, col);
                        break;
                }
            }
        }

        private void FillDeepCorner(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int rightUp = tiles[row-1, col+1];
            int leftDown = tiles[row+1, col-1];
            int rightDown = tiles[row+1, col+1];

            if(leftUp > rightDown)
            {
                tiles[row, col] = 4;
            }

            else if(rightUp > leftDown) 
            {
                tiles[row, col] = 3;
            }

            else if(leftDown > rightUp) 
            {
                tiles[row, col] = 1;
            }

            else 
            {
                tiles[row, col] = 2;
            }
        }

        private void FillShallowDeepCorner(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int rightUp = tiles[row-1, col+1];
            int leftDown = tiles[row+1, col-1];
            int rightDown = tiles[row+1, col+1];

            if(leftUp > rightDown)
            {
                tiles[row, col] = 8;
            }

            else if(rightUp > leftDown) 
            {
                tiles[row, col] = 7;
            }

            else if(leftDown > rightUp) 
            {
                tiles[row, col] = 5;
            }

            else 
            {
                tiles[row, col] = 6;
            }
        }

        private void FillShallowBeachCorner(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int rightUp = tiles[row-1, col+1];
            int leftDown = tiles[row+1, col-1];
            int rightDown = tiles[row+1, col+1];

            if(leftUp > rightDown)
            {
                tiles[row, col] = 21;
            }

            else if(rightUp > leftDown) 
            {
                tiles[row, col] = 20;
            }

            else if(leftDown > rightUp) 
            {
                tiles[row, col] = 18;
            }

            else 
            {
                tiles[row, col] = 19;
            }
        }

        private void FillBeachShallowCorner(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int rightUp = tiles[row-1, col+1];
            int leftDown = tiles[row+1, col-1];
            int rightDown = tiles[row+1, col+1];

            if(leftUp > rightDown)
            {
                tiles[row, col] = 25;
            }

            else if(rightUp > leftDown) 
            {
                tiles[row, col] = 24;
            }

            else if(leftDown > rightUp) 
            {
                tiles[row, col] = 22;
            }

            else 
            {
                tiles[row, col] = 23;
            }
        }

        private void FillBeachGrassCorner(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int rightUp = tiles[row-1, col+1];
            int leftDown = tiles[row+1, col-1];
            int rightDown = tiles[row+1, col+1];

            if(leftUp > rightDown)
            {
                tiles[row, col] = 38;
            }

            else if(rightUp > leftDown) 
            {
                tiles[row, col] = 37;
            }

            else if(leftDown > rightUp) 
            {
                tiles[row, col] = 35;
            }

            else 
            {
                tiles[row, col] = 36;
            }
        }

        private void FillGrassCorner(int row, int col)
        {
            int leftUp = tiles[row-1, col-1];
            int rightUp = tiles[row-1, col+1];
            int leftDown = tiles[row+1, col-1];
            int rightDown = tiles[row+1, col+1];

            if(leftUp > rightDown)
            {
                tiles[row, col] = 42;
            }

            else if(rightUp > leftDown) 
            {
                tiles[row, col] = 41;
            }

            else if(leftDown > rightUp) 
            {
                tiles[row, col] = 39;
            }

            else 
            {
                tiles[row, col] = 40;
            }
        }

        private void SetupWalkableTiles()
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col] < 18)
                    {
                        walkableTiles[row,col] = false;
                    } 
                    else 
                    {
                        walkableTiles[row,col] = true;
                    }
                }
            }
        }

        private void PassWalkableTilesToSystems()
        {
            ref var grid = ref World.DefaultGameObjectInjectionWorld.GetExistingSystem<ECS.Movement.Pathfinding.PathfindingSystem>().grid;
            grid = new NativeArray<bool>(walkableTiles.GetLength(0) * walkableTiles.GetLength(1), Allocator.Temp);

            World.DefaultGameObjectInjectionWorld.GetExistingSystem<ECS.Movement.Pathfinding.PathfindingSystem>()
                .gridSize = new int2(tiles.GetLength(0), tiles.GetLength(1));
            
            // Flatten
            for (int i = 0; i < grid.Length; i++)
            {
                int x = i % walkableTiles.GetLength(0);
                int y = i / walkableTiles.GetLength(1);
                grid[i] = walkableTiles[x, y];
            }
        }

        public bool [,] GetWalkableTiles()
        {
            return walkableTiles;
        }

        private void SetupGameObjects()
        {
            //Create method for this;
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    gameObjectsInGrid[row, col] = Scenary.Empty;
                }
            }




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
            SortedDictionary<Scenary, int> sortedScenary = new SortedDictionary<Scenary, int>();
            
            if (!sortedScenary.ContainsKey(CheckScenary(row-1, col-1)))
            {
                sortedScenary.Add(CheckScenary(row-1, col-1), 1);
            } 
            else 
            {
                sortedScenary[CheckScenary(row-1, col-1)]++;
            }

            if (!sortedScenary.ContainsKey(CheckScenary(row-1, col)))
            {
                sortedScenary.Add(CheckScenary(row-1, col), 1);
            } 
            else 
            {
                sortedScenary[CheckScenary(row-1, col)]++;
            }

            if (!sortedScenary.ContainsKey(CheckScenary(row-1, col+1)))
            {
                sortedScenary.Add(CheckScenary(row-1, col+1), 1);
            } 
            else 
            {
                sortedScenary[CheckScenary(row-1, col+1)]++;
            }

            if (!sortedScenary.ContainsKey(CheckScenary(row, col-1)))
            {
                sortedScenary.Add(CheckScenary(row, col-1), 1);
            } 
            else 
            {
                sortedScenary[CheckScenary(row, col-1)]++;
            }

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
            float rand = UnityEngine.Random.value;
            
            if (rand <= newBush)
            {
                GameObject ob = GetRandomBush();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                walkableTiles[row, col] = false;
                return Scenary.Bush;
            }
            else if (rand <= newTree)
            {
                GameObject ob = GetRandomTree();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                walkableTiles[row, col] = false;
                return Scenary.Tree;
            }
            else if (rand <= newRock) 
            {
                GameObject ob = GetRandomRock();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                walkableTiles[row, col] = false;
                return Scenary.Rock;
            }
            
            return Scenary.Empty;
        }

        private Scenary RandomizeDesertScenary(float value, int row, int col) 
        {
            float newRock = value / 2;
            float rand = UnityEngine.Random.value;
            
            if (rand <= newRock)
            {
                GameObject ob = GetRandomRock();
                Instantiate (ob, new Vector3(row, 0, col), Quaternion.identity);
                walkableTiles[row, col] = false;
                return Scenary.Rock;
            }
            else if (rand <= value)
            {
                Instantiate (cactus1, new Vector3(row, 0, col), Quaternion.identity);
                walkableTiles[row, col] = false;
                return Scenary.Cactus;
            }
            
            return Scenary.Empty;
        }

        private GameObject GetRandomBush()
        {
            float rand = UnityEngine.Random.value;
        
            if (rand <= 1f/9)
            {
                return bush1;
            }
            else if (rand <= 2f/9) 
            {
                return bush2;
            }
            else if (rand <= 3f/9) 
            {
                return bush3;
            }
            else if (rand <= 4f/9) 
            {
                return bush4;
            }
            else if (rand <= 5f/9) 
            {
                return bush5;
            }
            else if (rand <= 6f/9) 
            {
                return bush6;
            }
            else if (rand <= 7f/9) 
            {
                return bush7;
            }
            else if (rand <= 8f/9) 
            {
                return bush8;
            }
            else 
            {
                return bush9;
            }
        }

        private GameObject GetRandomTree()
        {
            float rand = UnityEngine.Random.value;

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
           float rand = UnityEngine.Random.value;
        
            if (rand <= 1f/12)
            {
                return rock1;
            }
            else if (rand <= 2f/12) 
            {
                return rock2;
            }
            else if (rand <= 3f/12) 
            {
                return rock3;
            }
            else if (rand <= 4f/12) 
            {
                return rock4;
            }
            else if (rand <= 5f/12) 
            {
                return rock5;
            }
            else if (rand <= 6f/12) 
            {
                return rock6;
            }
            else if (rand <= 7f/12) 
            {
                return rock7;
            }
            else if (rand <= 8f/12) 
            {
                return rock8;
            }
            else if (rand <= 9f/12)
            {
                return rock9;
            }
            else if (rand <= 10f/12) 
            {
                return rock10;
            }
            else if (rand <= 11f/12) 
            {
                return rock11;
            }
            else 
            {
                return rock12;
            }
        }
    }
}

