using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class MateState : IState {

        Animal owner;

        public MateState(Animal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {

        }

        public void Exit() {

        }
    }
}