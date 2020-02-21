using UnityEngine;
using Ecosystem.StateMachines;

namespace Ecosystem.Attributes {
    public class Fox : MonoBehaviour, IAnimal {

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Mating { get; set; }
        private float HungerLimit = Random.Range((float)0.3,(float)0.8);
        private float ThirstLimit = Random.Range((float)0.3,(float)0.8);
        private float MatingLimit = Random.Range((float)0.3,(float)0.8);
        public string FoodSource { get; } = "RABBIT,FRUIT";
        public Transform Trans { get; set; }

        StateMachine stateMachine;

        public Fox() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            stateMachine.ChangeState(new CasualState(this));
        }

        // Update is called once per frame
        void Update() {
            // Check for stateChange
            if (Thirst <= ThirstLimit) {
                stateMachine.ChangeState(new ThirstState(this));
            } else if (Hunger <= HungerLimit) {
                stateMachine.ChangeState(new HungerState(this));
            } else if (Mating <= MatingLimit) {
                stateMachine.ChangeState(new MateState(this));
            }
            stateMachine.Update();
        }
    }
}