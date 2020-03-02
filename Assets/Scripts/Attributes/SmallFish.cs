using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public class SmallFish : MonoBehaviour, IAnimal {

        private float Hunger;
        private float Thirst;
        //public float Mating { get; set; }
        private float HungerLimit;
        private float ThirstLimit;
        //private float MatingLimit = Random.Range(0.3f,0.8f);

        public string FoodSource { get; } = "SEAGRASS";
        //public Transform Trans { get; set; }
        public float Speed { get; set; }
        public float SprintSpeed { get; set; }
        public Movement movement;
        public Sensors Sensors;
        private float predatorLength;
        private float changePerFrame;

        StateMachine stateMachine;
        IState casualState;
        IState fleeState;
        IState hungerState;

        public SmallFish() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            this.Hunger = 1f;
            this.Thirst = 1f;
            this.HungerLimit = Random.Range(0.3f,0.8f);
            this.ThirstLimit = Random.Range(0.3f,0.8f);
            this.casualState = new CasualState(this);
            this.stateMachine.ChangeState(this.casualState);
            this.predatorLength = 5f;
            this.changePerFrame = 0.00001f;
            Sensors.LookForPredator(true);
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
                    Debug.Log("SmallFish flee");
                    stateMachine.ChangeState(this.fleeState);
                }
            } else if (Hunger <= HungerLimit) {
                if (stateMachine.getCurrentState() != this.hungerState) {
                    Debug.Log("SmallFish hunt");
                    stateMachine.ChangeState(this.hungerState);
                }
            } else if (stateMachine.getCurrentState() != this.casualState) {
                Debug.Log("SmallFish casual");
                stateMachine.ChangeState(this.casualState);
            }

            stateMachine.Update();

            //if (this.Hunger <= 0f || this.Thirst <= 0) {
            //    Die();
            //}
        }

        private bool PredatorInRange() {
            bool ans = false;
            if (Sensors.FoundPredator()) {
                Vector3 predatorPos = Sensors.GetFoundPredatorInfo().Position;
                Vector3 diff = predatorPos - transform.position;
                float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                if (diffLength < this.predatorLength) {
                    ans = true;
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
            return this.Thirst;
        }

        public void SetThirst(float newThirst) {
            this.Thirst = newThirst;
        }

        public Sensors GetSensors() {
            return this.Sensors;
        }

        public Transform GetTransform() {
            return transform;
        }
    }
}