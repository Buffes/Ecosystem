using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class CasualState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private readonly float pathfindInterval = 1f;

        public CasualState(Animal owner) { this.owner = owner; }

        public void Enter() {
            owner.GetSensors().LookForRandomTarget(true);
            nextTarget = owner.GetMovement().GetPosition();
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            if (!owner.GetSensors().FoundRandomTarget()) return;
            Vector3 currentPos = owner.GetMovement().GetPosition();
            Vector3 diff = nextTarget - currentPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            if (diffLength <= 2.5f) {
                nextTarget = owner.GetSensors().GetFoundRandomTargetInfo();
            }
            // Move owner
            owner.Move(nextTarget,0f,200);
        }


        public void Exit() {
            owner.GetSensors().LookForRandomTarget(false);
        }
    }
}