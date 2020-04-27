using UnityEngine;
using Ecosystem.ECS.Grid;
using Unity.Entities;
using Random = UnityEngine.Random;
using Ecosystem.ECS.Animal;
using Ecosystem;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField]
    private SimulationSettings settings = default;

    private WorldGridSystem worldGridSystem;

    private void Awake()
    {
        worldGridSystem = World.DefaultGameObjectInjectionWorld
            .GetOrCreateSystem<WorldGridSystem>();
    }

    private void Start()
    {
        SpawnInitialPopulation();
    }

    private void SpawnInitialPopulation()
    {
        // For each prefab find a free spot and spawn the gameobject
        foreach (var population in settings.InitialPopulations)
        {
            Spawn(population.Value.Prefab, population.Value.Amount);
        }
    }

    public void Spawn(GameObject prefab, int amount) => Spawn(prefab, amount, worldGridSystem);

    public static void Spawn(GameObject prefab, int amount, WorldGridSystem worldGridSystem)
    {
        GridData grid = worldGridSystem.Grid;
        MovementTerrainAuthoring movementTerrain = prefab.GetComponentInChildren<MovementTerrainAuthoring>();
        
        bool lookingForFreeTile;
        int length = grid.Length;
        for (int i = 0; i < amount; i++)
        {
            lookingForFreeTile = true;
            while (lookingForFreeTile)
            {
                int n = Random.Range(0, length);
                if (worldGridSystem.IsWalkable(movementTerrain.MovesOnLand, movementTerrain.MovesOnWater,
                    grid.GetGridPositionFromIndex(n)))
                {
                    Vector3 spawnPos = grid.GetWorldPosition(grid.GetGridPositionFromIndex(n));
                    spawnPos.y = 1f;
                    Instantiate(prefab, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    lookingForFreeTile = false;
                }
            }
        }
    }
}
