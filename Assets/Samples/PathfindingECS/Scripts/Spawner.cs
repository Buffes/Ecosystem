using UnityEngine;

namespace Ecosystem.Samples
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private Transform parent = default;
        [SerializeField]
        private GameObject rabbitPrefab = default;
        [SerializeField]
        private GameObject foxPrefab = default;
        [SerializeField]
        private GameObject OOPRabbitPrefab = default;
        [SerializeField]
        [Range(1, 1000)]
        private int spawnAmount = 1;

        [Header("Random Spawn Area")]

        [SerializeField]
        private int startX = 0;
        [SerializeField]
        private int endX = 100;
        [SerializeField]
        private int startZ = 0;
        [SerializeField]
        private int endZ = 100;

        public void SpawnRabbits()
        {
            SpawnPrefabs(rabbitPrefab, spawnAmount);
        }

        public void SpawnFoxes()
        {
            SpawnPrefabs(foxPrefab, spawnAmount);
        }

        public void SpawnOOPRabbits()
        {
            SpawnPrefabs(OOPRabbitPrefab, spawnAmount);
        }

        private void SpawnPrefabs(GameObject prefab, int amound)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Instantiate(
                    prefab,
                    new Vector3(Random.Range(startX, endX), prefab.transform.position.y, Random.Range(startZ, endZ)),
                    new Quaternion(),
                    parent);
            }
        }
    }
}
