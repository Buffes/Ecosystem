using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;

namespace Ecosystem.Attributes
{
    public class Animal : MonoBehaviour {

        private float hunger;
        private float hungerLimit;
        private float thirst;
        private float thirstLimit;
        private float mating;
        private float matingLimit;

        [SerializeField]
        private AnimalDNAAuthoring animalDNAAuthoring = default;
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
        private IState mateState;

        public Animal() {
            this.stateMachine = new StateMachine();
        }

        void Start() {
            this.hunger = 1f;
            this.hungerLimit = 0.5f;
            this.thirst = 1f;
            this.thirstLimit = 0.5f;
            this.mating = 1f;
            this.matingLimit = 0.5f;

            this.changePerSecond = 0.0001f;

            this.casualState = new CasualState(this);
            this.hungerState = new HungerState(this);
            this.thirstState = new ThirstState(this);
            this.fleeState = new FleeState(this);
            this.mateState = new MateState(this);
            this.stateMachine.ChangeState(this.casualState);
            sensors.LookForPredator(true);
        }

        /// <summary>
        /// Sets the DNA that this animal will spawn with.
        /// <para/>
        /// If not called, new DNA with default values will be created.
        /// </summary>
        public void InitDNA(DNA dna)
        {
            animalDNAAuthoring.DNA = dna;
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
            } else if (mating <= matingLimit) {
                if (stateMachine.getCurrentState() != this.mateState) {
                    stateMachine.ChangeState(this.mateState);
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