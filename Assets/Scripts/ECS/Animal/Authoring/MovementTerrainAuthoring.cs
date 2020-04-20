using Ecosystem.ECS.Movement;
using Unity.Entities;
using UnityEngine;


namespace Ecosystem.ECS.Animal
{
    public class MovementTerrainAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField]
        public bool MovesOnLand;
        [SerializeField]
        public bool MovesOnWater;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MovementTerrain {
                MovesOnLand = this.MovesOnLand,
                MovesOnWater = this.MovesOnWater
            });
        }
    }
}