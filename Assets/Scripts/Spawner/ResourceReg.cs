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

    private bool lookingForFreeTile = true;
    private int gridSize;

    public GameObject seaweed;
    public float SeaweedSpawnTime;
    private float deltaTimeSeaweed = 0f;

    private List<int> occupiedWaterSpace = new List<int>();

 
    void Update()
    {
        int[,] tiles = GameZone.tiles;
        NativeArray<bool> grid = GameZone.walkableTiles;
        gridSize = grid.Length;
        deltaTimeGrass += Time.deltaTime;
        deltaTimeSeaweed += Time.deltaTime;
        if (deltaTimeGrass >= GrassSpawnTime)
        {
            RegenerateGrass(grid, tiles);
            deltaTimeGrass = 0f;
        }

        
        if(deltaTimeSeaweed >= SeaweedSpawnTime)
        {
            RegenerateSeaweed(grid, tiles);
            deltaTimeSeaweed = 0f;
        }  
    }

    private void RegenerateGrass(NativeArray<bool> grid, int[,] tiles)
    {
        GameObject o = Instantiate(grass) as GameObject;
        lookingForFreeTile = true;
        while (lookingForFreeTile)
        {
            int n = UnityEngine.Random.Range(0, gridSize);
            int x = n % GameZone.tiles.GetLength(0);
            int y = (n - x) / GameZone.tiles.GetLength(0);
            if (grid[n] && tiles[x,y] == 51)
            {
                o.transform.position = new Vector3(x, 1, y);
                grid[n] = false;
                lookingForFreeTile = false;
            }
        }
    }

    private void RegenerateSeaweed(NativeArray<bool> grid, int[,] tiles)
    {
        GameObject o = Instantiate(seaweed) as GameObject;
        lookingForFreeTile = true;
        
        while (lookingForFreeTile)
        {
            int n = UnityEngine.Random.Range(0, gridSize);
            int x = n % GameZone.tiles.GetLength(0);
            int y = (n - x) / GameZone.tiles.GetLength(0);
           

            if (!grid[n] && tiles[x,y] < 34  && !occupiedWaterSpace.Contains(n))
            {
                o.transform.position = new Vector3(x, 1, y);
                lookingForFreeTile = false;
                occupiedWaterSpace.Add(n);
               
            }
        }
    }
    

}
