using UnityEngine;

namespace Ecosystem.Eatable {
    public class Spawner : MonoBehaviour {

        [SerializeField]
        private Transform parent = default;
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

        public void SpawnFruits() {
            SpawnPrefabs(fruitPrefab);
        }

        private void SpawnPrefabs(GameObject prefab) {
            for (int i = 0; i < spawnAmount; i++) {
                GameObject o = Instantiate(prefab) as GameObject;
                o.transform.position = parent.transform.position + new Vector3(Random.Range(-range,range),parent.transform.position.y,Random.Range(-range,range));
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
