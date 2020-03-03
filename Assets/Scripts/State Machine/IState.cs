using UnityEngine;

namespace Ecosystem.StateMachines {
    public interface IState {
        void Enter();
        void Execute();
        void Exit();
    }
}