using Ecosystem.ECS.Animal.Stats;
using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    /// <summary>
    /// Adds animal base stats and DNA to the entity. Optionally applies injected DNA to affect the
    /// base stats.
    /// </summary>
    public class AnimalDNAAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField]
        private bool isMale = default;
        [SerializeField]
        private float baseSpeed = default;
        [SerializeField]
        private float baseHearingRange = default;
        [SerializeField]
        private float baseVisionRange = default;
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("0 to 360 degrees")]
        private float baseVisionSpan = default;

        public DNA DNA { private get; set; }

        private void ApplyGenes()
        {
            isMale = DNA.IsMale;

            DNA.NextGene(ref baseSpeed);
            DNA.NextGene(ref baseHearingRange);
            DNA.NextGene(ref baseVisionRange);
            DNA.NextGene(ref baseVisionSpan);

            DNA.UpdateGenes();
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (DNA == null) DNA = DNA.DefaultGenes(isMale);
            ApplyGenes();

            dstManager.AddComponentData(entity, DNA);
            dstManager.AddComponentData(entity, new SexData { Sex = isMale ? Sex.Male : Sex.Female });
            dstManager.AddComponentData(entity, new BaseSpeed { Value = baseSpeed });
            dstManager.AddComponentData(entity, new BaseHearing { Range = baseHearingRange });
            dstManager.AddComponentData(entity, new BaseVision { Range = baseVisionRange, Angle = baseVisionSpan * (float)Math.PI * 2 });
        }
    }
}
