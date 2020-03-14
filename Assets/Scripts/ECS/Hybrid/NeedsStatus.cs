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
        /// Sate the hunger of an animal when eating.
        /// </summary>
        /// <param name="value">Float value</param>
        public void SateHunger(float value)
        {
            if (value <= 0.0f) return;
            float cur = GetComp<HungerData>().Hunger;
            entityManager.SetComponentData<HungerData>(entity, new HungerData { Hunger = cur + value });
        }

        /// <summary>
        /// Sate the thirst of an animal when drinking.
        /// </summary>
        /// <param name="value">Float value</param>
        public void SateThirst(float value)
        {
            if (value <= 0.0f) return;
            float cur = GetComp<ThirstData>().Thirst;
            entityManager.SetComponentData<ThirstData>(entity, new ThirstData { Thirst = cur + value });
        }

        private T GetComp<T>() where T : struct, IComponentData
        {
            return entityManager.GetComponentData<T>(entity);
        }
    }
}
