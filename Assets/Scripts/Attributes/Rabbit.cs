using UnityEngine;
using Ecosystem.StateMachines;

namespace Ecosystem.Attributes {
    public class Rabbit : MonoBehaviour, IAnimal {

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Mating { get; set; }

        StateMachine stateMachine;

        public Rabbit() {
            stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            stateMachine.ChangeState(new CasualState(this));
        }

        // Update is called once per frame
        void Update() {
            stateMachine.Update();
        }
    }
}