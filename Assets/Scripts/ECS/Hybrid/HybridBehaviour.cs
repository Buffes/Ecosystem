using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// Component that exposes methods that affect the ECS world to the normal GO/MB world.
    /// </summary>
    [RequireComponent(typeof(HybridEntity))]
    public class HybridBehaviour : MonoBehaviour
    {
        protected Entity Entity => hybridEntity.Entity;
        protected EntityManager EntityManager => hybridEntity.EntityManager;

        private HybridEntity hybridEntity;

        private void Awake()
        {
            hybridEntity = GetComponent<HybridEntity>();
        }
    }
}
