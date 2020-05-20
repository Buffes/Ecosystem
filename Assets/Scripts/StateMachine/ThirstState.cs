using UnityEngine;
using Ecosystem.Attributes;
using Ecosystem.ParticleSystems;

namespace Ecosystem.StateMachines {
    public class ThirstState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public ThirstState(Animal owner) { this.owner = owner; }

        public void Enter() {
            owner.GetMovement().Fly(false);
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetMovement().GetPosition();

            nextTarget = owner.GetSensors().GetFoundWaterInfo();
            float diffLength = Vector3.Distance(nextTarget, currentPos);
            if (diffLength <= 3f) {
                ParticleMono.InstantiateParticles(ParticleMono.drink, currentPos, 2f);
                owner.GetNeedsStatus().SateThirst(3f);
            }

            // Move owner
            owner.Move(nextTarget,1f,200);
        }

        public void Exit() {
            owner.GetSensors().LookForWater(false);
        }
    }
}