using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Targeting.Targets;
using Ecosystem.ECS.Targeting.Results;
using System;

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
        public void LookForWater(bool enabled) => AddRemoveComponentData(enabled, new LookingForWater());
        public void LookForFood(bool enabled) => AddRemoveComponentData(enabled, new LookingForFood());
        public void LookForPrey(bool enabled) => AddRemoveComponentData(enabled, new LookingForPrey());
        public void LookForPredator(bool enabled) => AddRemoveComponentData(enabled, new LookingForPredator());


        /// <summary>
        /// Returns if water has been found. Make sure to have enabled
        /// <see cref="LookForWater(bool)"/> first.
        /// </summary>
        public bool FoundWater() => entityManager.HasComponent<FoundWater>(entity);
        public bool FoundFood() => entityManager.HasComponent<FoundFood>(entity);
        public bool FoundPrey() => entityManager.HasComponent<FoundPrey>(entity);
        public bool FoundPredator() => entityManager.HasComponent<FoundPredator>(entity);


        /// <summary>
        /// Returns the location where water has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        [Obsolete("Use GetFoundWaterInfo().Position instead")]
        public Vector3 GetWaterLocation() => entityManager.GetComponentData<FoundWater>(entity).Position;
        [Obsolete("Use GetFoundFoodInfo().Position instead")]
        public Vector3 GetFoodLocation() => entityManager.GetComponentData<FoundFood>(entity).Position;
        [Obsolete("Use GetFoundPreyInfo().Position instead")]
        public Vector3 GetPreyLocation() => entityManager.GetComponentData<FoundPrey>(entity).Position;
        [Obsolete("Use GetFoundPredatorInfo().Position instead")]
        public Vector3 GetPredatorLocation() => entityManager.GetComponentData<FoundPredator>(entity).Position;


        /// <summary>
        /// Returns info about the water that has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        public FoundWater GetFoundWaterInfo() => entityManager.GetComponentData<FoundWater>(entity);
        public FoundFood GetFoundFoodInfo() => entityManager.GetComponentData<FoundFood>(entity);
        public FoundPrey GetFoundPreyInfo() => entityManager.GetComponentData<FoundPrey>(entity);
        public FoundPredator GetFoundPredatorInfo() => entityManager.GetComponentData<FoundPredator>(entity);


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
