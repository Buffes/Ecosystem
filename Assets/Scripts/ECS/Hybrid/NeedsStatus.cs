﻿using Unity.Entities;

using Ecosystem.ECS.Animal.Needs;
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Growth;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// 
    /// </summary>
    public class NeedsStatus : HybridBehaviour
    {
        /// <summary>
        /// If this is an adult
        /// </summary>
        public bool IsAdult()
        {
            return EntityManager.HasComponent<Adult>(Entity);
        }

        public float GetBravery()
        {
            BraveryData value = GetComp<BraveryData>();
            return value.Value;
        }

        /// <summary>
        /// Get hunger as float
        /// </summary>
        /// <returns></returns>
        public float GetHungerStatus()
        {
            if (!HasComp<HungerData>()) return float.MaxValue;
            HungerData value = GetComp<HungerData>();
            return value.Hunger;
        }

        /// <summary>
        /// Get thirst as float
        /// </summary>
        /// <returns></returns>
        public float GetThirstStatus()
        {
            if (!HasComp<ThirstData>()) return float.MaxValue;
            ThirstData value = GetComp<ThirstData>();
            return value.Thirst;
        }

        /// <summary>
        /// Get sexual urge as float
        /// </summary>
        /// <returns></returns>
        public float GetSexualUrgesStatus()
        {
            if (!HasComp<SexualUrgesData>()) return float.MaxValue;
            SexualUrgesData value = GetComp<SexualUrgesData>();
            return value.Urge;
        }

        /// <summary>
        /// Get hungerLimit as float
        /// </summary>
        /// <returns></returns>
        public float GetHungerLimit() {
            HungerLimit value = GetComp<HungerLimit>();
            return value.Value;
        }
        /// <summary>
        /// Get thistLimit as float
        /// </summary>
        /// <returns></returns>
        public float GetThirstLimit() {
            ThirstLimit value = GetComp<ThirstLimit>();
            return value.Value;
        }
        /// <summary>
        /// Get matingLimit as float
        /// </summary>
        /// <returns></returns>
        public float GetMatingLimit() {
            MatingLimit value = GetComp<MatingLimit>();
            return value.Value;
        }

        /// <summary>
        /// Sate the hunger of an animal when eating.
        /// </summary>
        /// <param name="value">Float value</param>
        public void SateHunger(float value)
        {
            if (value <= 0.0f) return;
            float cur = GetComp<HungerData>().Hunger;
            EntityManager.SetComponentData(Entity, new HungerData { Hunger = cur + value });
        }

        /// <summary>
        /// Sate the thirst of an animal when drinking.
        /// </summary>
        /// <param name="value">Float value</param>
        public void SateThirst(float value)
        {
            if (value <= 0.0f) return;
            float cur = GetComp<ThirstData>().Thirst;
            EntityManager.SetComponentData(Entity, new ThirstData { Thirst = cur + value });
        }

        /// <summary>
        /// Sate the sexual urges of an animal when reproducing.
        /// </summary>
        /// <param name="value">Float value</param>
        public void SateSexualUrge(float value)
        {
            if (value <= 0.0f) return;
            float cur = GetComp<SexualUrgesData>().Urge;
            EntityManager.SetComponentData(Entity, new SexualUrgesData { Urge = cur + value });
        }

        /// <summary>
        /// Sate the sexual urges of the partner when reproducing.
        /// </summary>
        /// <param name="value">Float value</param>
        public void SateSexualUrge(float value, Entity partner) {
            if (value <= 0.0f) return;
            if (!EntityManager.HasComponent<SexualUrgesData>(partner)) return;
            float cur = EntityManager.GetComponentData<SexualUrgesData>(partner).Urge;
            EntityManager.SetComponentData(partner,new SexualUrgesData { Urge = cur + value });
        }
        /// Transfer hunger from the parent of this animal to it.
        /// </summary>
        /// <param name="value">Float value</param>
        public void TransferHunger(float value)
        {
            if (!EntityManager.HasComponent<ParentData>(Entity)) return; // No parent
            float cur = GetComp<HungerData>().Hunger;
            Entity parentEntity = EntityManager.GetComponentData<ParentData>(Entity).Entity;
            float parentValue = EntityManager.GetComponentData<HungerData>(parentEntity).Hunger;
            EntityManager.SetComponentData(Entity, new HungerData { Hunger = cur + value });
            EntityManager.SetComponentData(parentEntity, new HungerData { Hunger = parentValue - value });
        }

        public void TransferThirst(float value)
        {
            if (!EntityManager.HasComponent<ParentData>(Entity)) return; // No parent
            float cur = GetComp<ThirstData>().Thirst;
            Entity parentEntity = EntityManager.GetComponentData<ParentData>(Entity).Entity;
            float parentValue = EntityManager.GetComponentData<ThirstData>(parentEntity).Thirst;
            EntityManager.SetComponentData(Entity, new ThirstData { Thirst = cur + value });
            EntityManager.SetComponentData(parentEntity, new ThirstData { Thirst = parentValue - value });

        }

        private T GetComp<T>() where T : struct, IComponentData => EntityManager.GetComponentData<T>(Entity);

        private bool HasComp<T>() where T : struct, IComponentData => EntityManager.HasComponent<T>(Entity);
    }
}