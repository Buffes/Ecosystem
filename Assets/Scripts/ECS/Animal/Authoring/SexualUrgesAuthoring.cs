using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Animal {
    public class SexualUrgesAuthoring : MonoBehaviour, IConvertGameObjectToEntity {
        
        [SerializeField]
        public float Urge;

        public void Convert(Entity entity,EntityManager dstManager,GameObjectConversionSystem conversionSystem) {
            dstManager.AddComponentData<Needs.SexualUrgesData>(entity,new Needs.SexualUrgesData {
                Urge = this.Urge * UnityEngine.Random.Range(0.5f,1.5f)
            });
        }
    }
}