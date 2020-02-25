using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid.Movement;

namespace Ecosystem.Attributes {
    public class Bird : MonoBehaviour, IAnimal {

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        public float Mating { get; set; }
        private float HungerLimit = Random.Range((float)0.3,(float)0.8);
        private float ThirstLimit = Random.Range((float)0.3,(float)0.8);
        private float MatingLimit = Random.Range((float)0.3,(float)0.8);
        public string FoodSource { get; } = "SMALLFISH,BIGFISH";
        public Transform Trans { get; set; }
        public float Speed { get; set; }
        public float SprintSpeed { get; set; }
        public Movement movement;

        StateMachine stateMachine;
        IState casual;

        public Bird() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            this.casual = new CasualState(this);
            this.stateMachine.ChangeState(this.casual);
        }
        public void Move(Vector3 target) {
            movement.Move(target);
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
            } else if (this.stateMachine.getCurrentState() != this.casual) {
                this.stateMachine.ChangeState(this.casual);
            }

            stateMachine.Update();
        }
    }
}