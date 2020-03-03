using Ecosystem.Gameplay;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    public class FoodTypeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField]
        private FoodType foodType = default;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new FoodTypeData
            {
                FoodTypeId = foodType.GetInstanceID(),
                FoodPoints = foodType.FoodPoints
            });
        }
    }
}
