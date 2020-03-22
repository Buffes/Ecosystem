using Ecosystem.ECS.Hybrid;
using UnityEngine;

namespace Ecosystem.Samples
{
    public class SyncTransform : MonoBehaviour
    {
        [SerializeField]
        private HybridEntity hybridEntity = default;
        [SerializeField]
        private Movement movement = default;

        private void Update()
        {
            if (!hybridEntity.HasConverted) return;

            transform.position = movement.GetPosition();
            transform.rotation = movement.GetRotation();
        }
    }
}
