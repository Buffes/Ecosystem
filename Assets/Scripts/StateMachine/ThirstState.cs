using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class ThirstState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public ThirstState(Animal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetMovement().GetPosition();

            nextTarget = owner.GetSensors().GetFoundWaterInfo();
            Vector3 diff = nextTarget - currentPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            if (diffLength <= 2f) {
                owner.GetNeedsStatus().SateThirst(1f);
            }

            // Move owner
            owner.Move(nextTarget,1f,100f);
        }

        public void Exit() {
            owner.GetSensors().LookForWater(false);
        }
    }
}