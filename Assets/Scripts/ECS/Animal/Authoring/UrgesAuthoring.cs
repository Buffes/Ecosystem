using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal
{
    public class UrgesAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {

        [SerializeField]
        public float SexualUrge;
        [SerializeField]
        public float Hunger;
        [SerializeField]
        public float Thirst;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData<Needs.SexualUrgesData>(entity, new Needs.SexualUrgesData
            {
                Urge = this.SexualUrge * UnityEngine.Random.Range(0.5f, 1.5f)
            });
            dstManager.AddComponentData<Needs.HungerData>(entity, new Needs.HungerData
            {
                Hunger = this.Hunger * UnityEngine.Random.Range(0.5f, 1.5f)
            });
            dstManager.AddComponentData<Needs.ThirstData>(entity, new Needs.ThirstData
            {
                Thirst = this.Thirst * UnityEngine.Random.Range(0.5f, 1.5f)
            });
        }
    }
}