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

    // Start is called before the first frame update
    void Start() 
    {
        RandomizeStartGrid();
        CheckCorners();
        CheckEdges();
        CheckMiddle();
        SetupTilemap();
    }


    private void RandomizeStartGrid()
    {
        for (int i = 0; i < tiles.GetLength(0); i += 2 )
        {
            for (int j = 0; j < tiles.GetLength(1); j += 2 )
            {
                if ((i - 2 >= 0 && tiles[i - 2,j] == 13) || (j - 2 >= 0 && tiles[i, j - 2] == 13))
                {
                    tiles[i,j] = RandomizeTile(0.7f);
                } else 
                {
                    tiles[i,j] = RandomizeTile(0.01f);
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
            return 13;
        }
        return 26;
    }


    private void CheckCorners()
    {
        if (tiles[0,0] >= 26 && tiles[0,2] >= 26 && tiles[2,0] >= 26 && tiles[2,2] >= 26)
        {
            tiles[0,0] = 39;
        } 
        else if (tiles[0,0] <= 13 && tiles[0,2] <= 13 && tiles[2,0] <= 13 && tiles[2,2] <= 13)
        {
            tiles[0,0] = 0;
        }
        
        if (tiles[0,tiles.GetLength(1)-3] >= 26 && tiles[0,tiles.GetLength(1)-1] >= 26 && tiles[2,tiles.GetLength(1)-3] >= 26 && tiles[2,tiles.GetLength(1)-1] >= 26)
        {
            tiles[0,tiles.GetLength(1)-1] = 39;
        }
        else if (tiles[0,tiles.GetLength(1)-3] <= 13 && tiles[0,tiles.GetLength(1)-1] <= 13 && tiles[2,tiles.GetLength(1)-3] <= 13 && tiles[2,tiles.GetLength(1)-1] <= 13)
        {
            tiles[0,tiles.GetLength(1)-1] = 0;
        }

        if (tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-3] >= 26 && tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-1] >= 26 && 
                tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-3] >= 26 && tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] >= 26)
        {
            tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] = 39;
        }
        else if(tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-3] <= 13 && tiles[tiles.GetLength(0)-3,tiles.GetLength(1)-1] <= 13 && 
                    tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-3] <= 13 && tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] <= 13)
        {
            tiles[tiles.GetLength(0)-1,tiles.GetLength(1)-1] = 0;
        }

        if (tiles[tiles.GetLength(0)-1, 0] >= 26 && tiles[tiles.GetLength(0)-3, 0] >= 26 && tiles[tiles.GetLength(0)-1, 2] >= 26 && tiles[tiles.GetLength(0)-3, 2] >= 26)
        {
            tiles[tiles.GetLength(0)-1, 0] = 39;
        }
        else if (tiles[tiles.GetLength(0)-1, 0] <= 13 && tiles[tiles.GetLength(0)-3, 0] <= 13 && tiles[tiles.GetLength(0)-1, 2] <= 13 && tiles[tiles.GetLength(0)-3, 2] <= 13) 
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
            if(tiles[0,i] >= 26 && tiles[0,i-2] >= 26 && tiles[0,i+2] >= 26 && 
               tiles[2,i] >= 26 && tiles[2,i-2] >= 26 && tiles[2,i+2] >= 26)
            {
                tiles[0,i] = 39;
            }
            else if(tiles[0,i] <= 13 && tiles[0,i-2] <= 13 && tiles[0,i+2] <= 13 && 
                    tiles[2,i] <= 13 && tiles[2,i-2] <= 13 && tiles[2,i+2] <= 13)
            {
                tiles[0,i] = 0;
            }
        }
    }

    private void CheckLowerEdges()
    {
        for(int i = 2; i < tiles.GetLength(1)-2; i += 2)
        {
            if(tiles[tiles.GetLength(0)-1,i] >= 26 && tiles[tiles.GetLength(0)-1,i-2] >= 26 && tiles[tiles.GetLength(0)-1,i+2] >= 26 && 
               tiles[tiles.GetLength(0)-3,i] >= 26 && tiles[tiles.GetLength(0)-3,i-2] >= 26 && tiles[tiles.GetLength(0)-3,i+2] >= 26)
            {
                tiles[tiles.GetLength(0)-1,i] = 39;
            }
            else if(tiles[tiles.GetLength(0)-1,i] <= 13 && tiles[tiles.GetLength(0)-1,i-2] <= 13 && tiles[tiles.GetLength(0)-1,i+2] <= 13 && 
                    tiles[tiles.GetLength(0)-3,i] <= 13 && tiles[tiles.GetLength(0)-3,i-2] <= 13 && tiles[tiles.GetLength(0)-3,i+2] <= 13)
            {
                tiles[tiles.GetLength(0)-1,i] = 0;
            }
        }
    }

    private void CheckRightEdges()
    {
        for(int i = 2; i < tiles.GetLength(0)-2; i += 2)
        {
            if(tiles[i-2,tiles.GetLength(1)-1] >= 26 && tiles[i,tiles.GetLength(1)-1] >= 26 && tiles[i+2,tiles.GetLength(1)-1] >= 26 && 
               tiles[i-2,tiles.GetLength(1)-3] >= 26 && tiles[i,tiles.GetLength(1)-3] >= 26 && tiles[i+2,tiles.GetLength(1)-3] >= 26)
            {
                tiles[i, tiles.GetLength(1)-1] = 39;
            }
            else if(tiles[i-2,tiles.GetLength(1)-1] <= 13 && tiles[i,tiles.GetLength(1)-1] <= 13 && tiles[i+2,tiles.GetLength(1)-1] <= 13 && 
                    tiles[i-2,tiles.GetLength(1)-3] <= 13 && tiles[i,tiles.GetLength(1)-3] <= 13 && tiles[i+2,tiles.GetLength(1)-3] <= 13)
            {
                tiles[i, tiles.GetLength(1)-1] = 0;
            }
        }
    }

    private void CheckLeftEdges()
    {
        for(int i = 2; i < tiles.GetLength(0)-2; i += 2)
        {
            if(tiles[i-2,0] >= 26 && tiles[i,0] >= 26 && tiles[i+2,0] >= 26 && 
               tiles[i-2,2] >= 26 && tiles[i,2] >= 26 && tiles[i+2,2] >= 26)
            {
                tiles[i,0] = 39;
            }
            else if(tiles[i-2,0] <= 13 && tiles[i,0] <= 13 && tiles[i+2,0] <= 13 && 
                    tiles[i-2,2] <= 13 && tiles[i,2] <= 13 && tiles[i+2,2] <= 13)
            {
                tiles[i,0] = 0;
            }
        }
    }

    private void CheckMiddle()
    {
        for (int i = 2; i < tiles.GetLength(0) - 3; i += 2)
        {
            for (int j = 2; j < tiles.GetLength(1) - 3; j += 2) 
            { 
                if (tiles[i-2,j-2] >= 26 && tiles[i-2,j] >= 26 && tiles[i-2,j+2] >= 26 && 
                    tiles[i,  j-2] >= 26 && tiles[i,  j] >= 26 && tiles[i,  j+2] >= 26 && 
                    tiles[i+2,j-2] >= 26 && tiles[i+2,j] >= 26 && tiles[i+2,j+2] >= 26) 
                {
                    tiles[i,j] = 39;
                }
                else if (tiles[i-2,j-2] <= 13 && tiles[i-2,j] <= 13 && tiles[i-2,j+2] <= 13 && 
                    tiles[i,  j-2] <= 13 && tiles[i,  j] <= 13 && tiles[i,  j+2] <= 13 && 
                    tiles[i+2,j-2] <= 13 && tiles[i+2,j] <= 13 && tiles[i+2,j+2] <= 13)
                {
                    tiles[i,j] = 0;
                }
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
                if (currentpos == 0) 
                {
                    tilemap.SetTile(pos, deepTile);
                } 
                else if (currentpos == 13)
                {
                    tilemap.SetTile(pos, shallowTile);
                } 
                else if (currentpos == 26)
                {
                    tilemap.SetTile(pos, beachTile);
                }
                else if (currentpos == 39)
                {
                    tilemap.SetTile(pos, grassTile);
                }
            }
        }
    }
}
