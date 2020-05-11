using Ecosystem.ECS.Stats.Base;
using Ecosystem.Genetics;
using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Animal.Needs;

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
            SetComponent((ref HungerLimit hungerLimit) => DNA.NextGene(ref hungerLimit.Value));
            if (entityManager.GetComponentData<HungerLimit>(entity).Value > entityManager.GetComponentData<MaxHungerData>(entity).MaxHunger)
            {
                entityManager.SetComponentData<HungerLimit>(entity,new HungerLimit { Value = 0.9f * entityManager.GetComponentData<MaxHungerData>(entity).MaxHunger });
            }
            SetComponent((ref ThirstLimit thirstLimit) => DNA.NextGene(ref thirstLimit.Value));
            if (entityManager.GetComponentData<ThirstLimit>(entity).Value > entityManager.GetComponentData<MaxThirstData>(entity).MaxThirst) {
                entityManager.SetComponentData<ThirstLimit>(entity,new ThirstLimit { Value = 0.9f * entityManager.GetComponentData<MaxThirstData>(entity).MaxThirst });
            }
            SetComponent((ref MatingLimit matingLimit) => DNA.NextGene(ref matingLimit.Value));
            if (entityManager.GetComponentData<MatingLimit>(entity).Value > entityManager.GetComponentData<MaxSexualUrgesData>(entity).MaxUrge) {
                entityManager.SetComponentData<MatingLimit>(entity,new MatingLimit { Value = 0.9f * entityManager.GetComponentData<MaxSexualUrgesData>(entity).MaxUrge });
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
