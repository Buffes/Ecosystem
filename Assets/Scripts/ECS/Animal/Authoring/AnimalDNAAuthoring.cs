using Ecosystem.ECS.Animal.Stats;
using Ecosystem.Genetics;
using System;
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

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (DNA == null) DNA = DNA.DefaultGenes(isMale);

            dstManager.AddComponentData(entity, DNA);
            dstManager.AddComponentData(entity, new SexData { Sex = DNA.IsMale ? Sex.Male : Sex.Female });
            dstManager.AddComponentData(entity, new BaseSpeed { Value = DNA.NextGene(baseSpeed) });
            dstManager.AddComponentData(entity, new BaseHearing { Range = DNA.NextGene(baseHearingRange) });
            dstManager.AddComponentData(entity, new BaseVision { Range = DNA.NextGene(baseVisionRange), Angle = baseVisionSpan * (float)Math.PI * 2 });
        }
    }
}
