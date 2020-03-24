using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FleeState : IState {

        Animal owner;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public FleeState(Animal owner) { this.owner = owner; }

        public void Enter() {
            // Starts sprint
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 predatorPos = owner.GetSensors().GetFoundPredatorInfo().Position;
            Vector3 currentPos = owner.GetMovement().GetPosition();
            Vector3 diff = currentPos - predatorPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            Vector3 target = currentPos + 5f*diff/diffLength;
            owner.Move(target,1f,100f);
        }

        public void Exit() {
            // End sprint
        }
    }
}