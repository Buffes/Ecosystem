using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem.ECS.Pool
{
    /// <summary>
    /// For grouping together and reusing gameObjects in pools.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {

        public Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

        static PoolManager _instance;

        public static PoolManager instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<PoolManager>();
                }
                return _instance;
            }
        }


        /// <summary>
        /// Creates a pool with objects of wanted type
        /// </summary>
        /// <param name="prefab">Type of objects contained in pool</param>
        /// <param name="poolSize">Size of new pool</param>
        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new Queue<GameObject>());
                for(int i = 0; i < poolSize; i++)
                {
                    GameObject newObject = Instantiate(prefab) as GameObject;
                    newObject.SetActive(false);
                    poolDictionary[poolKey].Enqueue(newObject);
                }
            }
        }

        /// <summary>
        /// If there is a pool of objects whose type match prefab, then reuse that GameObject and give it a new position and rotation. 
        /// </summary>
        /// <param name="prefab">Type of object to reuse</param>
        /// <param name="position">Position of new object</param>
        /// <param name="rotation">Rotation of new object</param>
        public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                GameObject objectToReuse = poolDictionary[poolKey].Dequeue();
                poolDictionary[poolKey].Enqueue(objectToReuse);

                objectToReuse.SetActive(true);
                objectToReuse.transform.position = position;
                objectToReuse.transform.rotation = rotation;
            }
        }

        void Awake()
        {
            _instance = this;
        }

    }
}
