using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FleeState : IState {

        IAnimal owner;

        public FleeState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {

        }

        public void Exit() {

        }
    }
}