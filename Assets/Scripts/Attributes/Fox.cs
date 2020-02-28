﻿using UnityEngine;
using Ecosystem.StateMachines;
using Ecosystem.ECS.Hybrid;

namespace Ecosystem.Attributes {
    public class Fox : MonoBehaviour, IAnimal {

        public float Hunger { get; set; }
        public float Thirst { get; set; }
        //private float Mating { get; set; }
        private float HungerLimit = Random.Range(0.3f,0.8f);
        private float ThirstLimit = Random.Range(0.3f,0.8f);
        //private float MatingLimit = Random.Range(0.3f,0.8f);

        public string FoodSource { get; } = "RABBIT,FRUIT";
        public Transform Trans { get; set; }
        public float Speed { get; set; }
        public float SprintSpeed { get; set; }
        public Movement movement;
        public Sensors Sensors { get; }

        StateMachine stateMachine;
        IState casual;

        public Fox() {
            this.stateMachine = new StateMachine();
        }

        // Start is called before the first frame update
        void Start() {
            this.casual = new CasualState(this);
            this.stateMachine.ChangeState(this.casual);
        }

        public void Move(Vector3 target,float reach,float range) {
            movement.Move(target,reach,range);
        }

        public void Die() {
            Destroy(this);
        }

        // Update is called once per frame
        void Update() {
            this.Thirst -= 0.00001f;
            this.Hunger -= 0.00001f;

            // Check for stateChange
            if (Thirst <= ThirstLimit) {
                stateMachine.ChangeState(new ThirstState(this));
            } else if (Hunger <= HungerLimit) {
                stateMachine.ChangeState(new HungerState(this));
            //} else if (Mating <= MatingLimit) {
                //stateMachine.ChangeState(new MateState(this));
            } else if (this.stateMachine.getCurrentState() != this.casual) {
                this.stateMachine.ChangeState(this.casual);
            }

            stateMachine.Update();
        }
    }
}