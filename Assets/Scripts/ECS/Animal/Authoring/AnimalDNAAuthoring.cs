using Ecosystem.ECS.Stats.Base;
using Ecosystem.Genetics;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Adds DNA to the entity and applies the gene values. Optionally uses injected DNA.
    /// </summary>
    public class AnimalDNAAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// This animal's DNA.
        /// <para/>
        /// If not set, new DNA with default values will be created.
        /// </summary>
        public DNA DNA { private get; set; }

        private Entity entity;
        private EntityManager entityManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            this.entity = entity;
            this.entityManager = dstManager;

            SetGenes();
            dstManager.AddComponentData(entity, DNA);
        }

        private void SetGenes()
        {
            SetComponent((ref SexData sex) =>
            {
                if (DNA == null) DNA = DNA.DefaultGenes(sex.Sex == Sex.Male);
                sex.Sex = DNA.IsMale ? Sex.Male : Sex.Female;
            });

            SetComponent((ref BaseSpeed speed) => DNA.NextGene(ref speed.Value));
            SetComponent((ref BaseHearingRange hearingRange) => DNA.NextGene(ref hearingRange.Value));
            SetComponent((ref BaseVisionRange visionRange) => DNA.NextGene(ref visionRange.Value));
            SetComponent((ref Needs.HungerLimit hungerLimit) => DNA.NextGene(ref hungerLimit.Value));
            if (entityManager.GetComponentData<Needs.HungerLimit>(entity).Value > entityManager.GetComponentData<Needs.MaxHungerData>(entity).MaxHunger) {
                entityManager.SetComponentData<Needs.HungerLimit>(entity,new Needs.HungerLimit { Value = 0.9f * entityManager.GetComponentData<Needs.MaxHungerData>(entity).MaxHunger });
            }
            SetComponent((ref Needs.ThirstLimit thirstLimit) => DNA.NextGene(ref thirstLimit.Value));
            if (entityManager.GetComponentData<Needs.ThirstLimit>(entity).Value > entityManager.GetComponentData<Needs.MaxThirstData>(entity).MaxThirst) {
                entityManager.SetComponentData<Needs.ThirstLimit>(entity,new Needs.ThirstLimit { Value = 0.9f * entityManager.GetComponentData<Needs.MaxThirstData>(entity).MaxThirst });
            }
            SetComponent((ref Needs.MatingLimit matingLimit) => DNA.NextGene(ref matingLimit.Value));
            if (entityManager.GetComponentData<Needs.MatingLimit>(entity).Value > entityManager.GetComponentData<Needs.MaxSexualUrgesData>(entity).MaxUrge) {
                entityManager.SetComponentData<Needs.MatingLimit>(entity,new Needs.MatingLimit { Value = 0.9f * entityManager.GetComponentData<Needs.MaxSexualUrgesData>(entity).MaxUrge });
            }
        }

        private delegate void ModifyComponentDelegate<T>(ref T t) where T : struct, IComponentData;

        private void SetComponent<T>(ModifyComponentDelegate<T> modifyComponentDelegate)
            where T : struct, IComponentData
        {
            T componentData = entityManager.GetComponentData<T>(entity);
            modifyComponentDelegate(ref componentData);
            entityManager.SetComponentData(entity, componentData);
        }
    }
}
