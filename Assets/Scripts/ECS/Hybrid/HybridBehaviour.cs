using System;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    [RequireComponent(typeof(ConvertToEntity))]
    public class HybridBehaviour : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Action Converted;
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
