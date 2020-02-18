using UnityEngine;
using Ecosystem.StateMachines;

namespace Ecosystem.Attributes {
    public class Rabbit : MonoBehaviour, IAnimal {

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Mating { get; set; }
        private float hungerLimit = Random.Range((float)0.3,(float)0.8);
        private float thirstLimit = Random.Range((float)0.3,(float)0.8);
        private float matingLimit = Random.Range((float)0.3,(float)0.8);
        public string FoodSource { get; } = "GRASS,BERRIES";

        StateMachine stateMachine;

        public Rabbit() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            stateMachine.ChangeState(new CasualState(this));
        }

        // Update is called once per frame
        void Update() {
            // TODO: Check for enemies, change state to flee

            // Check for stateChange
            if (Thirst <= thirstLimit) {
                stateMachine.ChangeState(new ThirstState(this));
            } else if (Hunger <= hungerLimit) {
                stateMachine.ChangeState(new HungerState(this));
            } else if (Mating <= matingLimit) {
                stateMachine.ChangeState(new MateState(this));
            }
            stateMachine.Update();
        }
    }
}