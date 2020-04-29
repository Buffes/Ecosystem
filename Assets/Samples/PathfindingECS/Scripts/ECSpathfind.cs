using Ecosystem.ECS.Hybrid;
using UnityEngine;
using Unity.Mathematics;

namespace Ecosystem.Samples
{
    public class ECSpathfind : MonoBehaviour
    {
        [SerializeField]
        private Movement movement = default;
        [SerializeField]
        private Sensors sensors = default;

        [SerializeField]
        private float reach = 0f;
        [SerializeField]
        private float range = 100f;
        [SerializeField]

        private Vector3 target;

        private void Start()
        {
            target = transform.position;
        }

        private void Update()
        {
            if (!movement.HasPath) 
            {
                // Randomize new target
                target = GetWorldPosition(new int2(UnityEngine.Random.Range(1, 99), UnityEngine.Random.Range(1, 99)));
                movement.Move(target, reach, range);
            }
        }

        private static int2 GetGridCoords(float3 worldPosition)
        {
            int x = (int)worldPosition.x - (worldPosition.x < 0 ? 1 : 0);
            int z = (int)worldPosition.z - (worldPosition.z < 0 ? 1 : 0);
            return new int2(x, z);
        }

        private static Vector3 GetWorldPosition(int2 gridCoords)
        {
            float x = gridCoords.x + 0.5f;
            float z = gridCoords.y + 0.5f;
            return new Vector3(x, 0f, z);
        }
    }
}
