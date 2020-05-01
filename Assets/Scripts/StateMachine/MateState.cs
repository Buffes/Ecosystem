using UnityEngine;
using Ecosystem.Attributes;
using Unity.Entities;
using Ecosystem.ParticleSystems;

namespace Ecosystem.StateMachines {
    public class MateState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private readonly float pathfindInterval = 1f;

        public MateState(Animal owner) { this.owner = owner; }

        public void Enter() {
            owner.GetMovement().Fly(false);
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetMovement().GetPosition();

            nextTarget = owner.GetSensors().GetFoundMateInfo().Position;
            Vector3 diff = nextTarget - currentPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            if (diffLength <= 2.5f) {
                // Reproduction event
                Entity partner = owner.GetSensors().GetFoundMateInfo().Entity;
                owner.GetInteraction().Reproduce(partner);
                owner.GetNeedsStatus().SateSexualUrge(1f);
                ParticleMono.InstantiateParticles(ParticleMono.breed, currentPos, 2f);
            }

            // Move owner
            owner.Move(nextTarget,1f,200);
        }

        public void Exit() {
            owner.GetSensors().LookForMate(false);
        }
    }
}