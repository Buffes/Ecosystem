using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class CasualState : IState {

        IAnimal owner;

        public CasualState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {
            // Move randomly
            Unity.Mathematics.float3 direction;
            direction.x = Random.Range(0f,1f);
            direction.y = Random.Range(0f,1f);
            direction.z = Random.Range(0f,1f);
            // TODO: move owner
        }

        public void Exit() {

        }
    }
}