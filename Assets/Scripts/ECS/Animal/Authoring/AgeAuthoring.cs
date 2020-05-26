using Unity.Entities;
using UnityEngine;


namespace Ecosystem.ECS.Animal
{
    public class AgeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField]
        public float Age;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new AgeData {
                Age = this.Age
            });
        }
    }
}