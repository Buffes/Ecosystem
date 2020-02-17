using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class ThirstState : IState {

        IAnimal owner;

        public ThirstState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {

        }

        public void Exit() {

        }
    }
}