using UnityEngine;

namespace Ecosystem.Berries {
    public class Spawner : MonoBehaviour {

        [SerializeField]
        private Transform parent = default;
        [SerializeField]
        private GameObject berryPrefab = default;
        [SerializeField]
        [Range(1,10)]
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

        public void SpawnBerries() {
            SpawnPrefabs(berryPrefab,spawnAmount);
        }

        private void SpawnPrefabs(GameObject prefab,int amount) {
            for (int i = 0; i < spawnAmount; i++) {
                Instantiate(
                    prefab,
                    new Vector3(Random.Range(startX,endX),prefab.transform.position.y,Random.Range(startZ,endZ)),
                    new Quaternion(),
                    parent);
            }
        }
    }
}
