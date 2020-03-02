using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class ThirstState : IState {

        IAnimal owner;

        public ThirstState(IAnimal owner) { this.owner = owner; }

        public void Enter() {
            owner.Sensors.LookForWater(true);
        }

        public void Execute() {
            Vector3 target;
            Vector3 currentPos = owner.Trans.position;

            if (owner.Sensors.FoundWater()) {
                target = owner.Sensors.GetFoundWaterInfo().Position;
                Vector3 diff = target - currentPos;
                float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                if (diffLength <= 2f) {
                    owner.SetThirst(1f);
                }
            } else {
                target = currentPos;
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
            }

            // Move owner
            owner.Move(target,1f,100f);
        }

        public void Exit() {
            owner.Sensors.LookForWater(false);
        }
    }
}