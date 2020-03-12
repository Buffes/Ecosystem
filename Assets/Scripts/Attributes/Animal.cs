using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public class Animal : MonoBehaviour {

        private float hunger;
        private float hungerLimit;
        private float thirst;
        private float thirstLimit;

        [SerializeField]
        private Movement movement = default;
        [SerializeField]
        private Sensors sensors = default;

        private float changePerSecond;

        private StateMachine stateMachine;
        private IState casualState;
        private IState hungerState;
        private IState thirstState;
        private IState fleeState;

        public Animal() {
            this.stateMachine = new StateMachine();
        }

        void Start() {
            this.hunger = 0.5f;
            this.hungerLimit = 0.5f;
            this.thirst = 1f;
            this.thirstLimit = 0.5f;

            this.changePerSecond = 0.0001f;

            this.casualState = new CasualState(this);
            this.hungerState = new HungerState(this);
            this.thirstState = new ThirstState(this);
            this.fleeState = new FleeState(this);
            this.stateMachine.ChangeState(this.casualState);
            sensors.LookForPredator(true);
        }

        public void Move(Vector3 target,float reach,float range) {
            movement.Move(target,reach,range);
        }

        public void Die() {
            Destroy(this);
        }

        void Update() {
            this.hunger -= this.changePerSecond*Time.deltaTime;
            bool predatorInRange = sensors.FoundPredator();

            if (predatorInRange) {
                if (stateMachine.getCurrentState() != this.fleeState) {
                    stateMachine.ChangeState(this.fleeState);
                }
            } else if (hunger <= hungerLimit) {
                if (stateMachine.getCurrentState() != this.hungerState) {
                    stateMachine.ChangeState(this.hungerState);
                }
            } else if (thirst <= thirstLimit) {
                if (stateMachine.getCurrentState() != this.thirstState) {
                    stateMachine.ChangeState(this.thirstState);
                }
            } else if (stateMachine.getCurrentState() != this.casualState) {
                stateMachine.ChangeState(this.casualState);
            }

            stateMachine.Update();
        }

        public float GetHunger() {
            return this.hunger;
        }

        public void SetHunger(float newHunger) {
            this.hunger = newHunger;
        }

        public void SetThirst(float newThirst) {
            this.thirst = newThirst;
        }

        public Sensors GetSensors() {
            return this.sensors;
        }

        public Movement GetMovement() {
            return this.movement;
        }

        public Transform GetTransform() {
            return transform;
        }
    }
}