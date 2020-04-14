using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Targeting.Targets;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// Functionality for awareness of surroundings (e.g., vision and hearing).
    /// </summary>
    public class Sensors : HybridBehaviour
    {
        /// <summary>
        /// Start/stop actively looking for water.
        /// </summary>
        public void LookForWater(bool enabled) => AddRemoveComp(enabled, new LookingForWater());
        public void LookForFood(bool enabled) => AddRemoveComp(enabled, new LookingForFood());
        public void LookForPrey(bool enabled) => AddRemoveComp(enabled, new LookingForPrey());
        public void LookForPredator(bool enabled) => AddRemoveComp(enabled, new LookingForPredator());
        public void LookForMate(bool enabled) => AddRemoveComp(enabled, new LookingForMate());
        public void LookForRandomTarget(bool enabled) => AddRemoveComp(enabled, new LookingForRandomTarget());


        /// <summary>
        /// Returns if water has been found. Make sure to have enabled
        /// <see cref="LookForWater(bool)"/> first.
        /// </summary>
        public bool FoundWater() => HasComp<LookingForWater>() && GetComp<LookingForWater>().HasFound;
        public bool FoundFood() => HasComp<LookingForFood>() && GetComp<LookingForFood>().HasFound;
        public bool FoundPrey() => HasComp<LookingForPrey>() && GetComp<LookingForPrey>().HasFound;
        public bool FoundPredator() => HasComp<LookingForPredator>() && GetComp<LookingForPredator>().HasFound;
        public bool FoundMate() => HasComp<LookingForMate>() && GetComp<LookingForMate>().HasFound;
        public bool FoundRandomTarget() => HasComp<LookingForRandomTarget>() && GetComp<LookingForRandomTarget>().HasFound;



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

        public (Vector3 Position, Vector3 PredictedPosition, Entity Entity) GetFoundPreyInfo()
        {
            LookingForPrey info = GetComp<LookingForPrey>();
            return (info.Position, info.PredictedPosition, info.Entity);
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

        public Vector3 GetFoundRandomTargetInfo()
        {
            LookingForRandomTarget info = GetComp<LookingForRandomTarget>();
            return info.Position;
        }


        private bool HasComp<T>() where T : struct, IComponentData
            => EntityManager.HasComponent<T>(Entity);

        private T GetComp<T>() where T : struct, IComponentData
        {
            return EntityManager.GetComponentData<T>(Entity);
        }

        private void AddRemoveComp<T>(bool add, T component) where T : struct, IComponentData
        {
            if (add && !EntityManager.HasComponent<T>(Entity))
            {
                EntityManager.AddComponentData(Entity, component);
            }
            else if (!add && EntityManager.HasComponent<T>(Entity))
            {
                EntityManager.RemoveComponent<T>(Entity);
            }
        }
    }
}
