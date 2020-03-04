using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public class AAnimal : MonoBehaviour {

        private float Hunger;
        //private float Thirst;
        //private float Mating
        private float HungerLimit;
        //private float ThirstLimit;
        //private float MatingLimit = Random.Range(0.3f,0.8f);

        //public float Speed { get; set; }
        //public float SprintSpeed { get; set; }
        public Movement movement;
        public Sensors Sensors;
        public bool Prey;
        private float predatorDist;
        private float changePerFrame;

        StateMachine stateMachine;
        IState casualState;
        IState hungerState;
        IState fleeState;

        public AAnimal() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            this.Hunger = 1f;
            //this.Thirst = 1f;
            this.HungerLimit = 0.99f;//Random.Range(0.3f,0.8f);
            //this.ThirstLimit = Random.Range(0.3f,0.8f);

            this.casualState = new CasualState(this);
            this.hungerState = new HungerState(this);
            this.fleeState = new FleeState(this);
            this.stateMachine.ChangeState(this.casualState);

            this.predatorDist = 5f;
            this.changePerFrame = 0.00001f;
            Sensors.LookForPredator(this.Prey);
        }

        public void Move(Vector3 target,float reach,float range) {
            movement.Move(target,reach,range);
        }

        public void Die() {
            Destroy(this);
        }

        // Update is called once per frame
        void Update() {
            this.Hunger -= this.changePerFrame;
            bool predatorInRange = PredatorInRange();

            if (predatorInRange) {
                if (stateMachine.getCurrentState() != this.fleeState) {
                    stateMachine.ChangeState(this.fleeState);
                }
            } else if (Hunger <= HungerLimit) {
                if (stateMachine.getCurrentState() != this.hungerState) {
                    stateMachine.ChangeState(this.hungerState);
                }
            } else if (stateMachine.getCurrentState() != this.casualState) {
                stateMachine.ChangeState(this.casualState);
            }

            stateMachine.Update();

            //if (this.Hunger <= 0f || this.Thirst <= 0) {
            //    Die();
            //}
        }

        private bool PredatorInRange() {
            bool ans = false;
            if (this.Prey) {
                if (Sensors.FoundPredator()) {
                    Vector3 predatorPos = Sensors.GetFoundPredatorInfo().Position;
                    Vector3 diff = predatorPos - transform.position;
                    float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                    if (diffLength < this.predatorDist) {
                        ans = true;
                    }
                }
            }
            return ans;
        }

        public float GetHunger() {
            return this.Hunger;
        }

        public void SetHunger(float newHunger) {
            this.Hunger = newHunger;
        }

        public float GetThirst() {
            return 1f; // this.Thirst;
        }

        public void SetThirst(float newThirst) {
            //this.Thirst = newThirst;
        }

        public Sensors GetSensors() {
            return this.Sensors;
        }

        public Transform GetTransform() {
            return transform;
        }
    }
}