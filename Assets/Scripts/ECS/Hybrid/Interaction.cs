using Ecosystem.ECS.Animal;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.ECS.Hybrid
{
    public class Interaction : MonoBehaviour, IConvertGameObjectToEntity
    {
        private EntityManager entityManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            entityManager = dstManager;
        }

        /// <summary>
        /// Kills an animal.
        /// </summary>
        /// <param name="animal">The animal entity to kill</param>
        public void Kill(Entity animal)
        {
            // TODO: Death event for the animal
        }

        /// <summary>
        /// Eats a food entity and returns its food points.
        /// </summary>
        /// <param name="food">The food entity to eat</param>
        /// <returns>The food points of the food that was eaten</returns>
        public int Eat(Entity food)
        {
            // TODO: Death event for the food

            return entityManager.GetComponentData<FoodTypeData>(food).FoodPoints;
        }
    }
}
