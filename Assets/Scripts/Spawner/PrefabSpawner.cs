using UnityEngine;
using Ecosystem.Grid;
using Unity.Collections;
using Unity.Mathematics;

public class PrefabSpawner : MonoBehaviour
{
    // GameObject array with the prefabs
    public GameObject[] whatToSpawnPrefab;
    // How many to spawn of each prefab
    public int[] amountToSpawn;

    void Start()
    {
        //get the world
        NativeArray<bool> grid = GameZone.walkableTiles;
        var gridSize = new int2(GameZone.tiles.GetLength(0), GameZone.tiles.GetLength(1));
        int differentPrefabs = whatToSpawnPrefab.Length;
        // Spawn
        spawnPlease(grid, differentPrefabs);
    }

    void spawnPlease(NativeArray<bool> grid, int differentPrefabs)
    {
        //for each prefab find a free spot and spawn the gameobject
        for (int k = 0; k < differentPrefabs; k++)
        {

            bool lookingForFreeTile;
            int length = grid.Length;
            for (int i = 0; i < amountToSpawn[k]; i++)
            {
                lookingForFreeTile = true;
                while (lookingForFreeTile)
                {
                    int n = UnityEngine.Random.Range(0, length);
                    if (grid[n])
                    {
                        Instantiate(whatToSpawnPrefab[k], new Vector3(n % GameZone.tiles.GetLength(0), 1, (n - n % GameZone.tiles.GetLength(0)) / GameZone.tiles.GetLength(0)), Quaternion.Euler(UnityEngine.Random.Range(0, 360), 0, 0));
                        grid[n] = false;
                    }

                    lookingForFreeTile = false;
                }
            }
        }
    }
}
