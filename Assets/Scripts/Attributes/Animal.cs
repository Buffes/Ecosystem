using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;
using Ecosystem.ECS.Animal;
using Ecosystem.Genetics;

namespace Ecosystem.Attributes
{
    public class Animal : MonoBehaviour
    {
        public StateMachine StateMachine { get => stateMachine; }

        private float hungerLimit;
        private float thirstLimit;
        private float matingLimit;

        [SerializeField]
        private AnimalDNAAuthoring animalDNAAuthoring = default;
        [SerializeField]
        private HybridEntity hybridEntity = default;
        [SerializeField]
        private Movement movement = default;
        [SerializeField]
        private Sensors sensors = default;
        [SerializeField]
        private NeedsStatus needs = default;
        [SerializeField]
        private Interaction interaction = default;

        private StateMachine stateMachine;
        private IState casualState;
        private IState hungerState;
        private IState thirstState;
        private IState fleeState;
        private IState mateState;
        private IState huntState;

        private void Awake() {
            this.stateMachine = new StateMachine();

            hybridEntity.Converted += Init;
        }

        private void OnDestroy()
        {
            hybridEntity.Converted -= Init;
        }

        private void Init() {
            this.hungerLimit = 0.5f;
            this.thirstLimit = 0.5f;
            this.matingLimit = 0.5f;

            this.casualState = new CasualState(this);
            this.hungerState = new HungerState(this);
            this.thirstState = new ThirstState(this);
            this.fleeState = new FleeState(this);
            this.mateState = new MateState(this);
            this.huntState = new HuntState(this);
            this.stateMachine.ChangeState(this.casualState);
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

        public void Move(Vector3 target, float reach, int maxTiles) {
            movement.Move(target, reach, maxTiles);
        }

        public void Die() {
            Destroy(this);
        }

        void Update() {
            if (!hybridEntity.HasConverted) return;

            float currentHunger = this.needs.GetHungerStatus();
            float currentThirst = this.needs.GetThirstStatus();
            float currentMating = this.needs.GetSexualUrgesStatus();

            sensors.LookForPredator(true);
            sensors.LookForPrey(currentHunger <= hungerLimit);
            sensors.LookForFood(currentHunger <= hungerLimit);
            sensors.LookForWater(currentThirst <= thirstLimit);
            sensors.LookForMate(currentMating <= matingLimit);

            if (sensors.FoundPredator())
            {
                sensors.LookForFleeTarget(true);
                if (stateMachine.getCurrentState() != this.fleeState)
                {
                    stateMachine.ChangeState(this.fleeState);
                }
            }
            else if (sensors.FoundFood() || sensors.FoundWater())
            {
                IState newState = sensors.FoundFood() ? this.hungerState : this.thirstState;

                if (sensors.FoundFood() && sensors.FoundWater()
                    && DiffLength(sensors.GetFoundWaterInfo())
                        < DiffLength(sensors.GetFoundFoodInfo().Position))
                {
                    newState = this.thirstState;
                }

                if (stateMachine.getCurrentState() != newState)
                {
                    stateMachine.ChangeState(newState);
                }
            }
            else if (sensors.FoundPrey())
            {
                if (stateMachine.getCurrentState() != this.huntState)
                {
                    stateMachine.ChangeState(this.huntState);
                }
            }
            else if (sensors.FoundMate())
            {
                if (stateMachine.getCurrentState() != this.mateState)
                {
                    stateMachine.ChangeState(this.mateState);
                }
            }
            else
            {
                if (stateMachine.getCurrentState() != this.casualState)
                {
                    stateMachine.ChangeState(this.casualState);
                }
            }
            stateMachine.Update();
        }

        private float DiffLength(Vector3 target) {
            Vector3 currentPos = movement.GetPosition();
            Vector3 diff = target - currentPos;
            return Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
        }

        public NeedsStatus GetNeedsStatus() {
            return this.needs;
        }

        public Sensors GetSensors() {
            return this.sensors;
        }

        public Movement GetMovement() {
            return this.movement;
        }

        public Interaction GetInteraction() {
            return this.interaction;
        }

    }
}
