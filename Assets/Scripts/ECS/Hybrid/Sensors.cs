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
        public bool FoundWater() => GetComp<LookingForWater>().HasFound;
        public bool FoundFood() => GetComp<LookingForFood>().HasFound;
        public bool FoundPrey() => GetComp<LookingForPrey>().HasFound;
        public bool FoundPredator() => GetComp<LookingForPredator>().HasFound;



        /// <summary>
        /// Returns info about the water that has been found.
        /// Make sure that water has been found first by checking <see cref="FoundWater()"/>.
        /// </summary>
        public Vector3 GetFoundWaterInfo()
        {
            LookingForWater info = GetComp<LookingForWater>();
            return info.Position;
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
            if (add && !entityManager.HasComponent<T>(entity))
            {
                entityManager.AddComponentData(entity, component);
            }
            else if (!add && entityManager.HasComponent<T>(entity))
            {
                entityManager.RemoveComponent<T>(entity);
            }
        }
    }
}
