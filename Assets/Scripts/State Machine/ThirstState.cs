﻿using UnityEngine;
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
                target = owner.Sensors.GetWaterLocation();
                if (target == currentPos) { // change to "in range"
                    owner.Thirst = 1f;
                    // TODO: drink water
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
            owner.Move(target);
        }

        public void Exit() {
            owner.Sensors.LookForWater(false);
        }
    }
}