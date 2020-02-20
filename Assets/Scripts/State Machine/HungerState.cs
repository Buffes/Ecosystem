using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class HungerState : IState {

        IAnimal owner;

        public HungerState(IAnimal owner) { this.owner = owner; }

        public void Enter() {

        }

        public void Execute() {
            bool foundFood = false;

            while (!foundFood)
            {
                //This will be changed to work with the field of view method
                var hitObjects = Physics.OverlapSphere(this.owner.transform.position, 30f);
                for (int i = 0; i < hitObjects.Length; i++)
                {
                    if (hitObjects[i].CompareTag(owner.FoodSource))
                    {
                        //move to hitObjects[i].transform.postion and eat it
                        //goes to the first obejct in the list and does not take distance into consideration
                        foundFood = true;
                    }
                    break;
                }

                //walk at random or by memory to find food
            }

        }

        public void Exit() {

        }
    }
}