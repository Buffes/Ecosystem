using Ecosystem.ECS.Grid;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;




/// <summary>
/// Spawns the prefab given as seaweed and the prefab given as grass in the water and on land respectively.
/// Spwans happens at predecided intervals 
/// </summary>

namespace Ecosystem.Spawner
{

    public class ResourceReg : MonoBehaviour
    {
        [SerializeField] private GameObject grass = default;
        [SerializeField] private float GrassSpawnTime = default;

        [SerializeField] private GameObject seaweed = default;
        [SerializeField] private float SeaweedSpawnTime = default;

        private float deltaTimeGrass = 0f;
        private float deltaTimeSeaweed = 0f;

        private WorldGridSystem worldGridSystem;

        private void Awake()
        {
            worldGridSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WorldGridSystem>();
        }

        void Update()
        {
            deltaTimeGrass += Time.deltaTime;
            deltaTimeSeaweed += Time.deltaTime;

            if (deltaTimeGrass < GrassSpawnTime && deltaTimeSeaweed < SeaweedSpawnTime) return;

            var grid = worldGridSystem.Grid;
            var occupiedCells = worldGridSystem.OccupiedCells;
            var waterCells = worldGridSystem.WaterCells;

            if (deltaTimeGrass >= GrassSpawnTime)
            {
                RegeneratePlant(grid, occupiedCells, waterCells, true, false, grass);
                deltaTimeGrass = 0f;
            }

            if (deltaTimeSeaweed >= SeaweedSpawnTime)
            {
                RegeneratePlant(grid, occupiedCells, waterCells, false, true, seaweed);
                deltaTimeSeaweed = 0f;
            }
        }

        private void RegeneratePlant(GridData grid, NativeArray<bool> occupiedCells, NativeArray<bool> waterCells,
            bool landBased, bool waterBased, GameObject plant)
        {
            bool lookingForFreeTile = true;
            int searchTries = 0;

            while (lookingForFreeTile)
            {
                if (searchTries > Mathf.Sqrt(grid.Length)) break;
                searchTries += 1;

                int n = Random.Range(0, grid.Length);

                if ((waterCells[n] && !waterBased)
                    || (!waterCells[n] && !landBased)) continue;
                if (occupiedCells[n]) continue;

                int2 gridPos = grid.GetGridPositionFromIndex(n);
                Vector3 spawnPos = grid.GetWorldPosition(gridPos);
                spawnPos.y = 0f;

                Instantiate(plant, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                worldGridSystem.SetOccupiedCell(gridPos);

                lookingForFreeTile = false;
            }
        }
    }
}
