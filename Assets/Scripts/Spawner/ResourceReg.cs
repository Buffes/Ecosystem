using Ecosystem.Grid;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;




/// <summary>
/// Spawns the prefab given as seaweed and the prefab given as grass in the water and on land respectively.
/// Spwans happens at predecided intervals 
/// </summary>
public class ResourceReg : MonoBehaviour
{

    public GameObject grass;
    public float GrassSpawnTime;
    private float deltaTimeGrass = 0f;
    private float searchTime = 0f;

    private bool lookingForFreeTile = true;
    private int gridSize;

    public GameObject seaweed;
    public float SeaweedSpawnTime;
    private float deltaTimeSeaweed = 0f;

    public static List<int> occupiedWaterSpace = new List<int>();

    private NativeArray<bool> grid;
    private int[,] tiles;

    void Update()
    {
        
        deltaTimeGrass += Time.deltaTime;
        deltaTimeSeaweed += Time.deltaTime;
        
        
        //bool yesboi;

        if (deltaTimeGrass >= GrassSpawnTime)
        {
            tiles = GameZone.tiles;
            grid = GameZone.walkableTiles;
            gridSize = grid.Length;
            RegenerateGrass(grid, tiles);
            deltaTimeGrass = 0f;
        }

        if(deltaTimeSeaweed >= SeaweedSpawnTime)
        {
            tiles = GameZone.tiles;
            grid = GameZone.walkableTiles;
            gridSize = grid.Length;
            RegenerateSeaweed(grid, tiles);
            deltaTimeSeaweed = 0f;    
        }  
    }

    private void RegenerateGrass(NativeArray<bool> grid, int[,] tiles)
    {
        lookingForFreeTile = true;
        searchTime = 0f;
        
        while (lookingForFreeTile)
        {
            if (searchTime > 100f)
                break;
            
            int n = UnityEngine.Random.Range(0, gridSize);
            int x = n % GameZone.tiles.GetLength(0);
            int y = (n - x) / GameZone.tiles.GetLength(0);
            if (grid[n] && tiles[x,y] == 51)
            {
                GameObject o = Instantiate(grass) as GameObject;
                o.transform.position = new Vector3(x, 1, y);
                grid[n] = false;
                lookingForFreeTile = false;
            }
            searchTime += Time.deltaTime;
        }
    }

    private void RegenerateSeaweed(NativeArray<bool> grid, int[,] tiles)
    { 
        lookingForFreeTile = true;
        searchTime = 0f;

        while (lookingForFreeTile)
        {
            if(searchTime > 100f) // To not get stuck when there's no tiles left we check ~700 tiles if they are a valid spawnpoint, if no spawnpoint is found we skip the spawning this time            {
                break;
            
            int n = UnityEngine.Random.Range(0, gridSize);
            int x = n % GameZone.tiles.GetLength(0);
            int y = (n - x) / GameZone.tiles.GetLength(0);

            if (!grid[n] && tiles[x,y] < 34  && !occupiedWaterSpace.Contains(n))
            {
                GameObject o = Instantiate(seaweed) as GameObject;
                o.transform.position = new Vector3(x, 1, y);
                lookingForFreeTile = false;
                occupiedWaterSpace.Add(n);
            }
            searchTime += Time.deltaTime;
        }
    }
}
