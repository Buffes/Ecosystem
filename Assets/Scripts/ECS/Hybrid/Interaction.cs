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
        }

        /// <summary>
        /// Eats a food entity and returns its food points.
        /// </summary>
        /// <param name="food">The food entity to eat</param>
        /// <returns>The food points of the food that was eaten</returns>
        public int Eat(Entity food)
        {
            if (KillEntity(food)) return EntityManager.GetComponentData<FoodTypeData>(food).FoodPoints;
            else return 0;
        }

        /// <summary>
        /// Reproduce with a partner. Attaches a reproductionEvent to both parts. Needs DNA component.
        /// </summary>
        /// <param name="partner"></param>
        /// <returns>If success</returns>
        public bool Reproduce(Entity partner)
        {
            if (!EntityManager.Exists(partner)) return false;

            EntityManager.AddComponentData(partner, new ReproductionEvent { PartnerDNA = EntityManager.GetComponentData<DNA>(Entity)});
            EntityManager.AddComponentData(Entity, new ReproductionEvent { PartnerDNA = EntityManager.GetComponentData<DNA>(partner)});
            return true;
        }

        private bool KillEntity(Entity e)
        {
            if (!EntityManager.Exists(e)) return false;
            return EntityManager.AddComponentData<DeathEvent>(e,new DeathEvent(DeathCause.Predators));
        }

    }
}
