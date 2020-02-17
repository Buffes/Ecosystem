using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class HungerState : IState {

        IAnimal owner;

        public HungerState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {

        }

        public void Exit() {

        }
    }
}