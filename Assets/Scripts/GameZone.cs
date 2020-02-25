using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class GameZone : MonoBehaviour 
{
    public int[,] tiles = new int[100,100];

    public int TileSize = 1;
    public Vector2Int StartPoint = new Vector2Int(0,0);

    private int numberOfTiles = 2;

    // Start is called before the first frame update
    void Start() 
    {
        RandomizeTiles();
        SetupTiles();
    }

    private void SetupTiles() 
    {
        var tilemap = GetComponentInChildren<Tilemap>();

        var localTilePositions = new List<Vector3Int>();

        foreach(var pos in tilemap.cellBounds.allPositionsWithin) 
        {
            Vector3Int localPosition = new Vector3Int(pos.x, pos.y, pos.x);
            localTilePositions.Add(localPosition);
        }

        SetupPath(localTilePositions, tilemap);
    
    }

    private void RandomizeTiles()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i,j] = Random.Range(0, numberOfTiles);
            }
        }
    }

    private void SetupPath(List<Vector3Int> positions, Tilemap tilemap) 
    {
        var waterTile = TilesResourceLoader.GetShallowTile();
        var grassTile = TilesResourceLoader.GetGrassTile();
        
        for(int i = 0; i < tiles.GetLength(0); i++) 
        {
            for(int j = 0; j < tiles.GetLength(1); j++) 
            {
                Vector3Int pos = new Vector3Int(i, j, 0);
                if (tiles[i,j] == 0) 
                {
                    tilemap.SetTile(pos, waterTile);
                } else 
                {
                    tilemap.SetTile(pos, grassTile);
                }
            }
        }
    }
}
