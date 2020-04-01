using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Debugging
{
    public class StateMachineRefAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField]
        private Attributes.Animal animal = default;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new StateMachineRef { StateMachine = animal.StateMachine });
        }
    }
}
