using Unity.Entities;
using UnityEngine;
using System;

using Ecosystem.ECS.Animal.Needs;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// 
    /// </summary>
    public class NeedsStatus : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Entity entity;
        private EntityManager entityManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entity = entity;
            entityManager = dstManager;
        }

        /// <summary>
        /// Get hunger as float
        /// </summary>
        /// <returns></returns>
        public float GetHungerStatus()
        {
            HungerData value = GetComp<HungerData>();
            return value.Hunger;
        }

        /// <summary>
        /// Get thirst as float
        /// </summary>
        /// <returns></returns>
        public float GetThirstStatus()
        {
            ThirstData value = GetComp<ThirstData>();
            return value.Thirst;
        }

        /// <summary>
        /// Decrease the hunger of an animal when drinking.
        /// </summary>
        /// <param name="value">Float value between 0 and 1</param>
        public void DecreaseHunger(float value)
        {
            float cur = GetComp<HungerData>().Hunger;
            entityManager.SetComponentData<HungerData>(entity, new HungerData { Hunger = cur - value });
        }

        /// <summary>
        /// Decrease the thirst of an animal when drinking.
        /// </summary>
        /// <param name="value">Float value between 0 and 1</param>
        public void DecreaseThirst(float value)
        {
            float cur = GetComp<ThirstData>().Thirst;
            entityManager.SetComponentData<ThirstData>(entity, new ThirstData { Thirst = cur - value });
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
