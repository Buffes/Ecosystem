using UnityEngine;
using Unity.Entities;
using Ecosystem.Attributes;
using Ecosystem.ParticleSystems;

namespace Ecosystem.StateMachines {
    public class HungerState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public HungerState(Animal owner) { this.owner = owner; }

        public void Enter() {
            owner.GetMovement().Fly(false);
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetMovement().GetPosition();
            nextTarget = owner.GetSensors().GetFoundFoodInfo().Position;
            float diffLength = Vector3.Distance(nextTarget, currentPos);
            if (diffLength <= 2.5f) {
                Entity food = owner.GetSensors().GetFoundFoodInfo().Entity;
                ParticleMono.InstantiateParticles(ParticleMono.eatplant, currentPos, 2f);
                ParticleMono.InstantiateParticles(ParticleMono.eatmeat, currentPos, 2f);
                owner.GetNeedsStatus().SateHunger(owner.GetInteraction().Eat(food));
            }

            // Move owner
            owner.Move(nextTarget,1f,200);
        }

        public void Exit() {
            owner.GetSensors().LookForFood(false);
        }
    }
}