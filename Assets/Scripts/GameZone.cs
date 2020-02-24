using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameZone : MonoBehaviour {

    public int Level = 1;
    private Dictionary<int, LevelsData.LevelData> _levelData;

    public int[,] tiles = new int[,] {
            {0,1,2},
            {1,1,0},
            {2,2,2 } 
    };

    public int TileSize = 1;
    public Vector2Int StartPoint = new Vector2Int(0,0);


    // Start is called before the first frame update
    void Start() {
        _levelData = GetComponent<LevelsDataLoader>().ReadLevelsData();

        SetupTiles();
    }

    private void SetupTiles() {

        var tilemap = GetComponentInChildren<Tilemap>();

        var localTilePositions = new List<Vector3Int>();

        foreach(var pos in tilemap.cellBounds.allPositionsWithin) {

            Vector3Int localPosition = new Vector3Int(pos.x, pos.y, pos.x);
            localTilePositions.Add(localPosition);
        }

        SetupPath(localTilePositions, tilemap);
    
    }

    private void SetupPath(List<Vector3Int> positions, Tilemap tilemap) {
        var path = _levelData[Level].path;
        var waterTile = TilesResourceLoader.GetWaterTile();
        var grassTile = TilesResourceLoader.GetGrassTile();
        var first = path.First();
        var last = path.Last();

        foreach (var pos in positions.GetRange(first, Mathf.Abs(first - last))) {
            //tilemap.SetTile(pos, waterTile);
        }


        for(int i = 0; i < 3; i++) {
            for(int j = 0; j < 3; j++) {
                Vector3Int pos = new Vector3Int(i, j, 0);
                Debug.Log(pos.x + " x and y " + pos.y);
                Debug.Log(i + " i and j " + j);
                if (tiles[i,j] == 0) {
                    tilemap.SetTile(pos, waterTile);
                } else {
                    tilemap.SetTile(pos, grassTile);
                }
            }
        }



    }


}
