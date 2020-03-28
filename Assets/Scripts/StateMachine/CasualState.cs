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
            Vector3 currentPos = owner.GetMovement().GetPosition();
            nextTarget = RandomTarget(currentPos);
        }

        public void Execute() {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;
            Debug.Log("Moving to: " + nextTarget);

            Vector3 currentPos = owner.GetMovement().GetPosition();
            Vector3 diff = nextTarget - currentPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
            if (diffLength <= 2.5f) {
                nextTarget = RandomTarget(currentPos);
            }
            // Move owner
            owner.Move(nextTarget,1f,50);
        }

        private Vector3 RandomTarget(Vector3 currentPos) {
            Vector3 target = currentPos;
            int tile = new System.Random().Next(8);

            int value = 5;
            switch (tile) {
                case 0: target.x -= value; target.z -= value; break;
                case 1: target.z -= value; break;
                case 2: target.x += value; target.z -= value; break;
                case 3: target.x -= value; break;
                case 4: target.x += value; break;
                case 5: target.x -= value; target.z += value; break;
                case 6: target.z += value; break;
                case 7: target.x += value; target.z += value; break;
                default: break;
            }


            if (target.x >= Ecosystem.Grid.GameZone.tiles.GetLength(0)) {
                target.x = Ecosystem.Grid.GameZone.tiles.GetLength(0) - 0.1f;
            } else if (target.x <= 0) {
                target.x = 0.1f;
            }

            if (target.z >= Ecosystem.Grid.GameZone.tiles.GetLength(1)) {
                target.z = Ecosystem.Grid.GameZone.tiles.GetLength(1) - 0.1f;
            } else if (target.z <= 0) {
                target.z = 0.1f;
            }

            return target;
        }

        public void Exit() {

        }
    }
}