using Ecosystem.ECS.Hybrid;
using UnityEngine;

namespace Ecosystem.Samples
{
    public class SyncTransform : MonoBehaviour
    {
        [SerializeField]
        private Movement movement = default;

        private void Update()
        {
            transform.position = movement.GetPosition();
            transform.rotation = movement.GetRotation();
        }
    }
}
