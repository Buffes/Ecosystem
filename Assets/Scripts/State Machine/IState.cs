using UnityEngine;

namespace Ecosystem.StateMachines {
    public interface IState {
        public void Enter();
        public void Execute();
        public void Exit();
    }
}