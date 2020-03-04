using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class HungerState : IState {

        AAnimal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 1f;

        public HungerState(AAnimal owner) { this.owner = owner; }

        public void Enter() {
            owner.GetSensors().LookForFood(true);
            nextTarget = RandomTarget(owner.GetTransform().position);
        }

        public void Execute() {

            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetTransform().position;

            if (owner.GetSensors().FoundFood()) {
                nextTarget = owner.GetSensors().GetFoundFoodInfo().Position;
                Vector3 diff = nextTarget - currentPos;
                float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                if (diffLength <= 2.5f) {
                    owner.SetHunger(1f);
                    // TODO: destroy food
                }
            } else {
                Vector3 diff = nextTarget - currentPos;
                float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                if (diffLength <= 2.5f) {
                    nextTarget = RandomTarget(currentPos);
                }
            }

            // Move owner
            owner.Move(nextTarget,1f,100f);

        }

        private Vector3 RandomTarget(Vector3 currentPos) {
            Vector3 target = currentPos;
            int tile = new System.Random().Next(8);
            switch (tile) {
                case 0: target.x -= 5; target.z -= 5; break;
                case 1: target.z -= 5; break;
                case 2: target.x += 5; target.z -= 5; break;
                case 3: target.x -= 5; break;
                case 4: target.x += 5; break;
                case 5: target.x -= 5; target.z += 5; break;
                case 6: target.z += 5; break;
                case 7: target.x += 5; target.z += 5; break;
                default: break;
            }
            return target;
        }

        public void Exit() {
            owner.GetSensors().LookForFood(false);
        }
    }
}