using Ecosystem.ECS.Grid.Buckets;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Targeting.Sensing
{
    public class SensedFoodAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddBuffer<BucketFoodData>(entity);
        }
    }
}
