using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Targeting.Targets;
using Ecosystem.ECS.Targeting.Results;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// Functionality for awareness of surroundings (e.g., vision and hearing).
    /// </summary>
    public class Sensors : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Entity entity;
        private EntityManager entityManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entity = entity;
            entityManager = dstManager;
        }

        /// <summary>
        /// Start/stop actively looking for water.
        /// </summary>
        public void LookForWater(bool enabled)
        {
            entityManager.AddComponentData(entity, new LookingForWater());
        }

        public void LookForFood(bool enabled)
        {
            entityManager.AddComponentData(entity, new LookingForFood());
        }

        public void LookForPrey(bool enabled)
        {
            entityManager.AddComponentData(entity, new LookingForPrey());
        }

        public void LookForPredator(bool enabled)
        {
            AddRemoveComponentData(enabled, new LookingForPredator());
        }


        /// <summary>
        /// Returns if water has been found. Make sure to have enabled
        /// <see cref="LookForWater(bool)"/> first.
        /// </summary>
        public bool FoundWater()
        {
            return entityManager.HasComponent<FoundWater>(entity);
        }

        public bool FoundFood()
        {
            return entityManager.HasComponent<FoundFood>(entity);
        }

        public bool FoundPrey()
        {
            return entityManager.HasComponent<FoundPrey>(entity);
        }

        public bool FoundPredator()
        {
            return entityManager.HasComponent<FoundPredator>(entity);
        }


        /// <summary>
        /// Returns the location where water has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        public Vector3 GetWaterLocation()
        {
            return entityManager.GetComponentData<FoundWater>(entity).Position;
        }

        public Vector3 GetFoodLocation()
        {
            return entityManager.GetComponentData<FoundFood>(entity).Position;
        }

        public Vector3 GetPreyLocation()
        {
            return entityManager.GetComponentData<FoundPrey>(entity).Position;
        }

        public Vector3 GetPredatorLocation()
        {
            return entityManager.GetComponentData<FoundPredator>(entity).Position;
        }


        private void AddRemoveComponentData<T>(bool add, T component) where T : struct, IComponentData
        {
            if (add)
            {
                entityManager.AddComponentData(entity, component);
            }
            else
            {
                entityManager.RemoveComponent<T>(entity);
            }
        }
    }
}
