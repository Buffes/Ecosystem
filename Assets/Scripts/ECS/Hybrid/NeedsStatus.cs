using Unity.Entities;

using Ecosystem.ECS.Animal.Needs;
using Ecosystem.ECS.Animal;

namespace Ecosystem.ECS.Hybrid
{
    /// <summary>
    /// 
    /// </summary>
    public class NeedsStatus : HybridBehaviour
    {
        /// <summary>
        /// Get age in seconds as float
        /// </summary>
        public float GetAge()
        {
            AgeData age = GetComp<AgeData>();
            return age.Age;
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
        /// Get sexual urge as float
        /// </summary>
        /// <returns></returns>
        public float GetSexualUrgesStatus()
        {
            SexualUrgesData value = GetComp<SexualUrgesData>();
            return value.Urge;
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
        /// Transfer hunger from the parent of this animal to it.
        /// </summary>
        /// <param name="value">Float value</param>
        public void TransferHunger(float value)
        {
            if (!EntityManager.HasComponent<ParentData>(Entity)) return; // No parent
            float cur = GetComp<HungerData>().Hunger;
            Entity ParentEntity = EntityManager.GetComponentData<ParentData>(Entity).Entity;
            EntityManager.SetComponentData(Entity, new HungerData { Hunger = cur + value });
            EntityManager.SetComponentData(ParentEntity, new HungerData { Hunger = cur - value });
        }

        private T GetComp<T>() where T : struct, IComponentData
        {
            return EntityManager.GetComponentData<T>(Entity);
        }
    }
}
