using UnityEngine;
using Unity.Entities;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class HungerState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public HungerState(Animal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetMovement().GetPosition();
            nextTarget = owner.GetSensors().GetFoundFoodInfo().Position;
            Vector3 diff = nextTarget - currentPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            if (diffLength <= 2.5f) {
                Entity food = owner.GetSensors().GetFoundFoodInfo().Entity;
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