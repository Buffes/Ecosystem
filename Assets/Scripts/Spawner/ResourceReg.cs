using Ecosystem.Grid;
using Unity.Collections;
using UnityEngine;




/// <summary>
/// Spawns the prefab given as seaweed and the prefab given as grass in the water and on land respectively.
/// Spwans happens at predecided intervals 
/// </summary>

namespace Ecosystem.Spawner
{

    public class ResourceReg : MonoBehaviour
    {

        public GameObject grass;
        public float GrassSpawnTime;
        private float deltaTimeGrass = 0f;
        private int searchTries;

        private bool lookingForFreeTile = true;
        private int gridSize;

        public GameObject seaweed;
        public float SeaweedSpawnTime;
        private float deltaTimeSeaweed = 0f;

        private NativeArray<bool> grid;
        private int[,] tiles;

        void Update()
        {

            deltaTimeGrass += Time.deltaTime;
            deltaTimeSeaweed += Time.deltaTime;

            if (deltaTimeGrass >= GrassSpawnTime)
            {
                tiles = GameZone.tiles;
                grid = GameZone.walkableTiles;
                gridSize = grid.Length;
                RegeneratePlant(grid, tiles, grass);
                deltaTimeGrass = 0f;
            }

            if (deltaTimeSeaweed >= SeaweedSpawnTime)
            {
                tiles = GameZone.tiles;
                grid = GameZone.walkableTiles;
                gridSize = grid.Length;
                RegeneratePlant(grid, tiles, seaweed);
                deltaTimeSeaweed = 0f;
            }
        }

        private void RegeneratePlant(NativeArray<bool> grid, int[,] tiles, GameObject plant)
        {
            lookingForFreeTile = true;
            searchTries = 0;

            while (lookingForFreeTile)
            {
                if (searchTries > Mathf.Sqrt(gridSize))
                    break;
                


                int n = Random.Range(0, gridSize);
                int x = n % tiles.GetLength(0);
                int y = (n - x) / tiles.GetLength(0);


                if ((grid[n] && tiles[x, y] == 51 && plant == grass) ||
                    (!grid[n] && tiles[x, y] < 34 && plant == seaweed))
                {
                    GameObject o = Instantiate(plant, new Vector3(x, 1, y), Quaternion.Euler(0, Random.Range(0, 360), 0)) as GameObject;
                    lookingForFreeTile = false;
                }
                searchTries += 1;
            }

        }
    }
}
