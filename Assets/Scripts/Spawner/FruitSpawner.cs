using Ecosystem.ECS.Grid;
using Ecosystem.Grid;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Ecosystem.Eatable {
    public class FruitSpawner : MonoBehaviour {

        [SerializeField]
        private GameObject fruitPrefab = default;
        [SerializeField]
        [Range(1,10)]
        private int spawnAmount = 1;
        private float deltaTime = 0f;
        [SerializeField]
        private float spawnTime = 2f;

        [Header("Random Spawn Area")]

        [SerializeField]
        private float range = 1.5f;
        
        private WorldGridSystem worldGridSystem;
        void Start()
        {
            worldGridSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WorldGridSystem>();
        }

        public void SpawnFruits() {
            SpawnPrefabs(fruitPrefab);
        }

        private void SpawnPrefabs(GameObject prefab) {
            for (int i = 0; i < spawnAmount; i++) {
                float x = transform.position.x + UnityEngine.Random.Range(-range, range);
                float z = transform.position.z + UnityEngine.Random.Range(-range, range);
                float3 position = new float3(x, 0, z);
                if (!worldGridSystem.Grid.IsInBounds(position)) return;

                float y = GroundCollisionSystem.GetGroundLevel(position, worldGridSystem.HeightMap, worldGridSystem.Grid);
                
                Instantiate(prefab,
                    new Vector3(x, y, z),
                    Quaternion.identity);
            }
        }

        private void Update() {
            deltaTime += Time.deltaTime;
            if (deltaTime > spawnTime) {
                deltaTime = 0f;
                SpawnFruits();
            }
        }
    }
}
