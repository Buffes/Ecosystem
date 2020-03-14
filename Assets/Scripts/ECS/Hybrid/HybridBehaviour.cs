using System;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// Component that exposes methods that affect the ECS world to the normal GO/MB world.
    /// </summary>
    [RequireComponent(typeof(ConvertToEntity))]
    public class HybridBehaviour : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// Invoked when the entity has been created.
        /// </summary>
        public Action Converted;

        /// <summary>
        /// If the entity has been created yet.
        /// </summary>
        public bool HasConverted { get; private set; }

        protected Entity Entity
        {
            get
            {
                if (!HasConverted) throw notConvertedException;
                return _entity;
            }
        }
        protected EntityManager EntityManager
        {
            get
            {
                if (!HasConverted) throw notConvertedException;
                return _entityManager;
            }
        }

        private Entity _entity;
        private EntityManager _entityManager;
        private Exception notConvertedException => new InvalidOperationException("The entity has not been converted yet."
            + " You should listen to the Converted event or check the HasConverted bool");

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            _entity = entity;
            _entityManager = dstManager;

            HasConverted = true;
            Converted?.Invoke();
        }
    }
}
