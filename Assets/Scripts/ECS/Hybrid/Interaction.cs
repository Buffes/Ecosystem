using Ecosystem.ECS.Animal;
using Ecosystem.ECS.Death;
using Ecosystem.ECS.Reproduction;
using Ecosystem.Genetics;
using Unity.Entities;

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

        /// <summary>
        /// Reproduce with a partner. Attaches a reproductionEvent to both parts. Needs DNA component.
        /// </summary>
        /// <param name="partner"></param>
        public void Reproduce(Entity partner)
        {
            EntityManager.AddComponentData(partner, new ReproductionEvent { PartnerDNA = EntityManager.GetComponentData<DNA>(Entity)});
            EntityManager.AddComponentData(Entity, new ReproductionEvent { PartnerDNA = EntityManager.GetComponentData<DNA>(partner)});
        }

        private void KillEntity(Entity e)
        {
            EntityManager.AddComponent<DeathEvent>(e);
        }

    }
}
