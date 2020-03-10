using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Targeting.Targets;
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
        public void LookForWater(bool enabled) => AddRemoveComp(enabled, new LookingForWater());
        public void LookForFood(bool enabled) => AddRemoveComp(enabled, new LookingForFood());
        public void LookForPrey(bool enabled) => AddRemoveComp(enabled, new LookingForPrey());
        public void LookForPredator(bool enabled) => AddRemoveComp(enabled, new LookingForPredator());
        public void LookForMate(bool enabled) => AddRemoveComp(enabled, new LookingForMate());


        /// <summary>
        /// Returns if water has been found. Make sure to have enabled
        /// <see cref="LookForWater(bool)"/> first.
        /// </summary>
        public bool FoundWater() => entityManager.GetComponentData<LookingForWater>(entity).HasFound;
        public bool FoundFood() => entityManager.GetComponentData<LookingForFood>(entity).HasFound;
        public bool FoundPrey() => entityManager.GetComponentData<LookingForPrey>(entity).HasFound;
        public bool FoundPredator() => entityManager.GetComponentData<LookingForPredator>(entity).HasFound;
        public bool FoundMate() => entityManager.GetComponentData<LookingForMate>(entity).HasFound;


        /// <summary>
        /// Returns the location where water has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        [Obsolete("Use GetFoundWaterInfo().Position instead")]
        public Vector3 GetWaterLocation() => entityManager.GetComponentData<LookingForWater>(entity).Position;
        [Obsolete("Use GetFoundFoodInfo().Position instead")]
        public Vector3 GetFoodLocation() => entityManager.GetComponentData<LookingForFood>(entity).Position;
        [Obsolete("Use GetFoundPreyInfo().Position instead")]
        public Vector3 GetPreyLocation() => entityManager.GetComponentData<LookingForPrey>(entity).Position;
        [Obsolete("Use GetFoundPredatorInfo().Position instead")]
        public Vector3 GetPredatorLocation() => entityManager.GetComponentData<LookingForPredator>(entity).Position;



        /// <summary>
        /// Returns info about the water that has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        public (Vector3 Position, Entity Entity) GetFoundWaterInfo()
        {
            LookingForWater info = GetComp<LookingForWater>();
            return (info.Position, info.Entity);
        }

        public (Vector3 Position, Entity Entity) GetFoundFoodInfo()
        {
            LookingForFood info = GetComp<LookingForFood>();
            return (info.Position, info.Entity);
        }

        public (Vector3 Position, Entity Entity) GetFoundPreyInfo()
        {
            LookingForPrey info = GetComp<LookingForPrey>();
            return (info.Position, info.Entity);
        }

        public (Vector3 Position, Entity Entity) GetFoundPredatorInfo()
        {
            LookingForPredator info = GetComp<LookingForPredator>();
            return (info.Position, info.Entity);
        }

        public (Vector3 Position, Entity Entity) GetFoundMateInfo()
        {
            LookingForMate info = GetComp<LookingForMate>();
            return (info.Position, info.Entity);
        }


        private T GetComp<T>() where T : struct, IComponentData
        {
            return entityManager.GetComponentData<T>(entity);
        }

        private void AddRemoveComp<T>(bool add, T component) where T : struct, IComponentData
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
