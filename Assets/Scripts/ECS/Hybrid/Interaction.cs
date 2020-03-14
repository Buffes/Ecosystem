using Ecosystem.ECS.Animal;
using Unity.Entities;
using Ecosystem.ECS.Events;

namespace Ecosystem.ECS.Hybrid
{
    public class Interaction : HybridBehaviour
    {
        /// <summary>
        /// Kills an animal.
        /// </summary>
        /// <param name="animal">The animal entity to kill</param>
        public void Kill(Entity animal)
        {
            KillEntity(animal);

            EntityManager.CreateEntity(/*Food*/); // TODO: The animal should drop food upon death
        }

        /// <summary>
        /// Eats a food entity and returns its food points.
        /// </summary>
        /// <param name="food">The food entity to eat</param>
        /// <returns>The food points of the food that was eaten</returns>
        public int Eat(Entity food)
        {
            KillEntity(food);

            return EntityManager.GetComponentData<FoodTypeData>(food).FoodPoints;
        }

        private void KillEntity(Entity e)
        {
            EntityManager.AddComponent<DeathEvent>(e);
        }
    }
}
