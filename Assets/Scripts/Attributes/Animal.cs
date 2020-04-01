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

        private void Awake() {
            this.stateMachine = new StateMachine();

            hybridEntity.Converted += Init;
        }

        private void OnDestroy()
        {
            hybridEntity.Converted -= Init;
        }

        private void Init() {
            this.needs.SateHunger(1f);
            this.needs.SateThirst(1f);
            this.needs.SateSexualUrge(1f);

            this.hungerLimit = 0.5f;
            this.thirstLimit = 0.5f;
            this.matingLimit = 0.5f;

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

        public void Move(Vector3 target, float reach, int maxTiles) {
            movement.Move(target, reach, maxTiles);
        }

        public void Die() {
            Destroy(this);
        }

        void Update() {
            if (!hybridEntity.HasConverted) return;

            float diffHunger = 100f;
            float diffThirst = 100f;
            float currentHunger = this.needs.GetHungerStatus();
            float currentThirst = this.needs.GetThirstStatus();
            float currentMating = this.needs.GetSexualUrgesStatus();

            if (sensors.FoundPredator()) {
                if (stateMachine.getCurrentState() != this.fleeState) {
                    stateMachine.ChangeState(this.fleeState);
                }
            } else if ((currentHunger <= hungerLimit) || (currentThirst <= thirstLimit)) {
                if (currentHunger <= hungerLimit) {
                    sensors.LookForFood(true);
                    if (sensors.FoundFood()) {
                        diffHunger = DiffLength(sensors.GetFoundFoodInfo().Position);
                    }
                }
                if (currentThirst <= thirstLimit) {
                    sensors.LookForWater(true);
                    if (sensors.FoundWater()) {
                        diffThirst = DiffLength(sensors.GetFoundWaterInfo());
                    }
                }
                IState closest = (diffHunger <= diffThirst) ? this.hungerState : this.thirstState;
                if (stateMachine.getCurrentState() != closest) {
                    stateMachine.ChangeState(closest);
                }
            } else if (currentMating <= matingLimit) {
                sensors.LookForMate(true);
                if (sensors.FoundMate() && stateMachine.getCurrentState() != this.mateState) {
                    stateMachine.ChangeState(this.mateState);
                }
            } else {
                if (stateMachine.getCurrentState() != this.casualState) {
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
