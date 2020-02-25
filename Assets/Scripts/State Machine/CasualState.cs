using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class CasualState : IState {

        IAnimal owner;

        public CasualState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {
            // Random Target
            Vector3 currentPos = owner.Trans.position;
            Vector3 target = currentPos;
            int tile = new System.Random().Next(8);
            switch (tile) {
                case 0: target.x = currentPos.x - 1; target.z = currentPos.z - 1; break;
                case 1: target.z = currentPos.z - 1; break;
                case 2: target.x = currentPos.x + 1; target.z = currentPos.z - 1; break;
                case 3: target.x = currentPos.x - 1; break;
                case 4: target.x = currentPos.x + 1; break;
                case 5: target.x = currentPos.x - 1; target.z = currentPos.z + 1; break;
                case 6: target.z = currentPos.z + 1; break;
                case 7: target.x = currentPos.x + 1; target.z = currentPos.z + 1; break;
                default: break;
            }
            // Move owner
            owner.Move(target);
        }

        public void Exit() {

        }
    }
}