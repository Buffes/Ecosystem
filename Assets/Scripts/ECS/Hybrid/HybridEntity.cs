using System;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// Captures the resulting entity and its entity manager in the conversion of this game object.
    /// </summary>
    [RequireComponent(typeof(ConvertToEntity))]
    public class HybridEntity : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// Invoked when the entity has been created.
        /// </summary>
        public Action Converted;

        /// <summary>
        /// If the entity has been created yet.
        /// </summary>
        public bool HasConverted { get; private set; }

        public Entity Entity
        {
            get
            {
                if (!HasConverted) throw notConvertedException;
                return _entity;
            }
        }

        public EntityManager EntityManager
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
