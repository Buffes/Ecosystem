using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class CasualState : IState {

        IAnimal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame = 0f;
        private readonly float pathfindInterval = 1f;

        public CasualState(IAnimal owner) { this.owner = owner; }

        public void Enter() {
            Vector3 currentPos = owner.GetTransform().position;
            nextTarget = RandomTarget(currentPos);
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 currentPos = owner.GetTransform().position;
            Vector3 diff = nextTarget - currentPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            if (diffLength <= 2.5f) {
                nextTarget = RandomTarget(currentPos);
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

        }
    }
}