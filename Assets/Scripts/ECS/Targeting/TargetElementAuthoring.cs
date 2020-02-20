using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Ecosystem.ECS.Targeting
{
    public class TargetElementAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity e, EntityManager em, GameObjectConversionSystem cs)
        {
            em.AddBuffer<Target>(e);
        }
    }
}
