using UnityEngine;

namespace Ecosystem.StateMachines {
    public class StateMachine {

        private IState currentState;
        private IState previousState;

        public void ChangeState(IState newState) {
            if (currentState != null) {
                currentState.Exit();
            }
            previousState = currentState;

            currentState = newState;
            currentState.Enter();
        }

        public void Update() {
            if (currentState != null) {
                currentState.Execute();
            }
        }

        public void SwitchToPreviousState()
        {
            currentState.Exit();
            IState tmpState = currentState;
            currentState = previousState;
            previousState = tmpState;
            currentState.Enter();
        }
    }
}