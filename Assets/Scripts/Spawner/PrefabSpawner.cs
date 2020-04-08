using UnityEngine;
using Ecosystem.ECS.Grid;
using Unity.Entities;
using Random = UnityEngine.Random;

public class PrefabSpawner : MonoBehaviour
{
    // GameObject array with the prefabs
    public GameObject[] whatToSpawnPrefab;
    // How many to spawn of each prefab
    public int[] amountToSpawn;

    private WorldGridSystem worldGridSystem;

    private void Awake()
    {
        worldGridSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<WorldGridSystem>();
    }

    void Start()
    {
        GridData grid = worldGridSystem.Grid;
        int differentPrefabs = whatToSpawnPrefab.Length;
        // Spawn
        spawnPlease(grid, differentPrefabs, true, false);
    }

    void spawnPlease(GridData grid, int differentPrefabs,
        bool land, bool water)
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
                    int n = Random.Range(0, length);
                    if (worldGridSystem.IsWalkable(land, water,
                        grid.GetGridPositionFromIndex(n)))
                    {
                        Vector3 spawnPos = grid.GetWorldPosition(grid.GetGridPositionFromIndex(n));
                        spawnPos.y = 1f;
                        Instantiate(whatToSpawnPrefab[k], spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                        lookingForFreeTile = false;
                    }
                }
            }
        }
    }
}
