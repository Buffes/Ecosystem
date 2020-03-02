using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public class Bird : MonoBehaviour, IAnimal {

        private float Hunger;
        private float Thirst;
        //public float Mating { get; set; }
        private float HungerLimit;
        private float ThirstLimit;
        //private float MatingLimit = Random.Range(0.3f,0.8f);

        public string FoodSource { get; } = "SMALLFISH,BIGFISH";
        //public Transform Trans { get; set; }
        public float Speed { get; set; }
        public float SprintSpeed { get; set; }
        public Movement movement;
        public Sensors Sensors;
        private float changePerFrame;

        StateMachine stateMachine;
        IState casualState;
        IState hungerState;

        public Bird() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            this.Hunger = 1f;
            this.Thirst = 1f;
            this.HungerLimit = Random.Range(0.3f,0.8f);
            this.ThirstLimit = Random.Range(0.3f,0.8f);
            this.casualState = new CasualState(this);
            this.hungerState = new HungerState(this);
            this.changePerFrame = 0.00001f;
            this.stateMachine.ChangeState(this.casualState);
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

            if (Hunger <= HungerLimit) {
                if (stateMachine.getCurrentState() != this.hungerState) {
                    stateMachine.ChangeState(this.hungerState);
                    Debug.Log("Bird hunt");
                }
            } else if (stateMachine.getCurrentState() != this.casualState) {
                stateMachine.ChangeState(this.casualState);
                Debug.Log("Bird casual");
            }

            stateMachine.Update();

            //if (this.Hunger <= 0f || this.Thirst <= 0) {
            //    Die();
            //}
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