using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FleeState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public FleeState(Animal owner) {
            this.owner = owner;
            timeSinceLastFrame = Random.Range(0f,pathfindInterval);
        }

        public void Enter() {
            // Starts sprint
            owner.GetMovement().Sprint(true);
            owner.GetMovement().Fly(true);
            owner.GetSensors().LookForFleeTarget(true);
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            if (!owner.GetSensors().FoundFleeTarget()) return;

            nextTarget = owner.GetSensors().GetFoundFleeTargetInfo();
            owner.Move(nextTarget,0f,200);
        }

        public void Exit() {
            // End sprint
            owner.GetMovement().Sprint(false);
            owner.GetSensors().LookForFleeTarget(false);
        }
    }
}