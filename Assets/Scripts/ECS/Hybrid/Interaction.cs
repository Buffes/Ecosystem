
using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Pool;
using Unity.Entities;
using UnityEngine;
using Ecosystem.ECS.Death;

namespace Ecosystem.ECS.Hybrid
{
    public class Interaction : MonoBehaviour, IConvertGameObjectToEntity
    {
        private EntityManager entityManager;
        private EntityCommandBuffer ecb;

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
            KillEntity(animal);

            entityManager.CreateEntity(/*Food*/); // TODO: The animal should drop food upon death
        }

        /// <summary>
        /// Eats a food entity and returns its food points.
        /// </summary>
        /// <param name="food">The food entity to eat</param>
        /// <returns>The food points of the food that was eaten</returns>
        public int Eat(Entity food)
        {
            KillEntity(food);

            return entityManager.GetComponentData<FoodTypeData>(food).FoodPoints;
        }

        private void KillEntity(Entity e)
        {
            entityManager.AddComponent<DeathEvent>(e);
        }
    }
}
