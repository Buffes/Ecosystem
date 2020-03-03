using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class MateState : IState {

        IAnimal owner;

        public MateState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {

        }

        public void Exit() {

        }
    }
}