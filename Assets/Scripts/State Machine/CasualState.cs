using UnityEngine;
using Ecosystem.Attributes;
using Ecosystem.ECS.Movement;

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
            MovementInput movementInput;
            movementInput.Sprint = false;
            movementInput.Direction = direction;
            MovementStats movementStats;
            movementStats.Speed = owner.Speed;
            movementStats.SprintSpeed = owner.SprintSpeed;
            // TODO: move owner
        }

        public void Exit() {

        }
    }
}