using UnityEngine;

namespace Ecosystem.StateMachines {
    public class StateMachine {
        IState currentState;

        public void ChangeState(IState newState) {
            if (currentState != null) {
                currentState.Exit();
            }
            currentState = newState;
            currentState.Enter();
        }

        public void Update() {
            if (currentState != null) {
                currentState.Execute();
            }
        }
    }
}