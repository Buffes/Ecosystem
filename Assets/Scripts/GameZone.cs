using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class GameZone : MonoBehaviour 
{
    public int[,] tiles = new int[99,99];

    public int TileSize = 1;
    public Vector2Int StartPoint = new Vector2Int(0,0);

    private int waterIndex = 17;
    private int landIndex = 34;
    private int diffWaterLand = 0;

    private float waterSpawnRate = 0.7f;
    private float waterNeighbourRate = 0.005f;

    private bool [,] walkableTiles;

    // Start is called before the first frame update
    void Start() 
    {
        diffWaterLand = landIndex - waterIndex;
        walkableTiles = new bool [tiles.GetLength(0), tiles.GetLength(1)];
        RandomizeStartGrid();
        CheckCorners();
        CheckEdges();
        CheckMiddle();
        SetupTilemap();
        SetupWalkableTiles();
    }


    private void RandomizeStartGrid()
    {
        for (int i = 0; i < tiles.GetLength(0); i += 2 )
        {
            for (int j = 0; j < tiles.GetLength(1); j += 2 )
            {
                if ((i - 2 >= 0 && tiles[i - 2,j] == waterIndex) || (j - 2 >= 0 && tiles[i, j - 2] == waterIndex))
                {
                    tiles[i,j] = RandomizeTile(waterSpawnRate);
                } else 
                {
                    tiles[i,j] = RandomizeTile(waterNeighbourRate);
                }
            }
        }
    }

    //The probability of getting water, otherwise create grass. 
    private int RandomizeTile(float water)
    {
        float rand = Random.value;
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

    public bool [,] GetWalkableTiles()
    {
        return walkableTiles;
    }
}
