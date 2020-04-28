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

        public float HungerLimit {get; private set;}
        public float ThirstLimit {get; private set;}
        public float MatingLimit {get; private set;}
        public float BraveryLevel { get; private set;}

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
        private IState followParentState;

        private void Awake() {
            this.stateMachine = new StateMachine();

            hybridEntity.Converted += Init;
        }

        private void OnDestroy()
        {
            hybridEntity.Converted -= Init;
        }

        private void Init() {
            this.HungerLimit = needs.GetHungerLimit();
            this.ThirstLimit = needs.GetThirstLimit();
            this.MatingLimit = needs.GetMatingLimit();
            this.BraveryLevel = needs.GetBravery();

            this.casualState = new CasualState(this);
            this.hungerState = new HungerState(this);
            this.thirstState = new ThirstState(this);
            this.fleeState = new FleeState(this);
            this.mateState = new MateState(this);
            this.huntState = new HuntState(this);
            this.followParentState = new FollowParentState(this);
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

            bool shouldFlee = false;

            if (sensors.FoundPredator())
            {
                float distanceToPredator = DiffLength(sensors.GetFoundPredatorInfo().Position);
                shouldFlee = sensors.FoundPredator() && (ShouldFlee(HungerLimit, currentHunger, BraveryLevel, distanceToPredator) || ShouldFlee(ThirstLimit, currentThirst, BraveryLevel, distanceToPredator));
            }

            sensors.LookForPredator(true);
            sensors.LookForFleeTarget(shouldFlee);
            sensors.LookForParent(!needs.IsAdult());
            sensors.LookForPrey(currentHunger <= HungerLimit);
            sensors.LookForFood(currentHunger <= HungerLimit);
            sensors.LookForWater(currentThirst <= ThirstLimit);
            sensors.LookForMate(currentMating <= MatingLimit);

            if (sensors.FoundFleeTarget())
            {
                ChangeState(this.fleeState);
            }
            else if (!needs.IsAdult())
            {
                if (sensors.FoundParent())
                {
                    ChangeState(this.followParentState);
                }
                else 
                {
                    ChangeState(this.casualState);
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

                ChangeState(newState);
            }
            else if (sensors.FoundPrey())
            {
                ChangeState(this.huntState);
            }
            else if (sensors.FoundMate())
            {
                ChangeState(this.mateState);
            }
            else
            {
                ChangeState(this.casualState);
            }
            stateMachine.Update();
        }

        private float DiffLength(Vector3 target) {
            Vector3 currentPos = movement.GetPosition();
            Vector3 diff = target - currentPos;
            return Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
        }

        private bool ShouldFlee(float limit, float current, float bravery, float distanceToPredator)
        {
            if (distanceToPredator <= 0)
                return true;

            float need = (current / limit);
            float m = Mathf.Pow(need / bravery, 2.0f)*0.1f;
            m = Mathf.Clamp(m, 0.0f, 1.0f);
            bool flee = m * distanceToPredator < 0.25f;
            return flee;
        }

        private void ChangeState(IState state)
        {
            if (stateMachine.getCurrentState() != state)
            {
                stateMachine.ChangeState(state);
            }
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
