using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class MateState : IState {

        AAnimal owner;

        public MateState(AAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {

        }

        public void Exit() {

        }
    }
}