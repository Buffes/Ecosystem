using UnityEngine;

namespace Ecosystem.StateMachines {
    public class Unit : MonoBehaviour {

        StateMachine stateMachine = new StateMachine();

        // Start is called before the first frame update
        void Start() {
            stateMachine.ChangeState(new ThirstState(this));
        }

        // Update is called once per frame
        void Update() {
            stateMachine.Update();
        }
    }
}