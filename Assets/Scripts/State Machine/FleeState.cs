using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FleeState : IState {

        IAnimal owner;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public FleeState(IAnimal owner) { this.owner = owner; }

        public void Enter() {
            // Starts sprint
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 predatorPos = owner.GetSensors().GetFoundPredatorInfo().Position;
            Vector3 currentPos = owner.GetTransform().position;
            Vector3 diff = currentPos - predatorPos;
            Vector3 target = currentPos + diff;

            owner.Move(target,1f,100f);
        }

        public void Exit() {
            // End sprint
        }
    }
}