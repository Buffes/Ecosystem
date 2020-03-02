﻿using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FleeState : IState {

        IAnimal owner;

        public FleeState(IAnimal owner) { this.owner = owner; }

        public void Enter() {
            // Starts sprint
        }

        public void Execute() {
            Vector3 predatorPos = owner.Sensors.GetFoundPredatorInfo().Position;
            Vector3 currentPos = owner.Trans.position;
            Vector3 diff = currentPos - predatorPos;
            Vector3 target = currentPos + diff;

            owner.Move(target,0f,100f);
        }

        public void Exit() {
            // End sprint
        }
    }
}