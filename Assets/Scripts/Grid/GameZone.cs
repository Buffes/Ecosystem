using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using Unity.Entities;
using Unity.Mathematics;
using Ecosystem.ECS.Grid;

namespace Ecosystem.Grid
{
    public class GameZone : MonoBehaviour 
    {
        //Tiles with assets in the Grid, and the size of the Grid
        public static int[,] tiles = new int[99,99];
        
        public static Tilemap tilemap;
        public static List<Vector3Int> tilePositions;

        private TilesAssetsToTilemap tilesAssetsToTilemap;

        //The numbers of the shallow and beach tiles
        private int waterIndex = 17;
        private int landIndex = 34;
        
        //The difference between water and land
        private int diffWaterLand = 0;

        //The rate of chance for spawning an object if there is a similar object as neighbour
        public float waterNeighbourRate = 0.65f;

        //The rate of objects spawning
        public float waterSpawnRate = 0.005f;

        private WorldGridSystem worldGridSystem;

        void Awake() 
        {
            InitObjects();
            RandomizeStartGrid();
            CheckCorners();
            CheckEdges();
            CheckMiddle();
            SetupTilemap();
            tilesAssetsToTilemap = new TilesAssetsToTilemap();
            SetupWaterTiles();
        }

        private void InitObjects()
        {
            diffWaterLand = landIndex - waterIndex;

            worldGridSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WorldGridSystem>();
            worldGridSystem.InitGrid(tiles.GetLength(0), tiles.GetLength(1));
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

        private void SetupTilemap() 
        {
            tilemap = GetComponentInChildren<Tilemap>();
            tilePositions = new List<Vector3Int>();

            foreach(var pos in tilemap.cellBounds.allPositionsWithin) 
            {
                Vector3Int localPosition = new Vector3Int(pos.x, pos.y, pos.x);
                tilePositions.Add(localPosition);
            }
        }

        private void SetupWaterTiles()
        {
            for (int row = 0; row < tiles.GetLength(0); row++)
            {
                for (int col = 0; col < tiles.GetLength(1); col++)
                {
                    if (tiles[row, col] < 34) // All water tiles
                    {
                        SetWaterCell(row, col);
                    } 
                }
            }
        }

        private void SetWaterCell(int x, int y) => worldGridSystem.SetWaterCell(new int2(x, y));

        public void SetBlockedCell(int x, int y)
            => worldGridSystem.SetOccupiedCell(new int2(x, y), true);
    }
}

