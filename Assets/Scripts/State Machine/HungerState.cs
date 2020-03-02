using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class HungerState : IState {

        IAnimal owner;

        public HungerState(IAnimal owner) { this.owner = owner; }

        public void Enter() {
            owner.Sensors.LookForFood(true);
        }

        public void Execute() {

            Vector3 target;
            Vector3 currentPos = owner.Trans.position;

            if (owner.Sensors.FoundFood()) {
                target = owner.Sensors.GetFoundFoodInfo().Position;
                Vector3 diff = target - currentPos;
                float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x,2) + Mathf.Pow(diff.z,2));
                if (diffLength <= 2f) {
                    owner.SetHunger(1f);
                    // TODO: destroy food
                }
            } else {
                target = currentPos;
                int tile = new System.Random().Next(8);
                switch (tile) {
                    case 0: target.x = currentPos.x - 1; target.z = currentPos.z - 1; break;
                    case 1: target.z = currentPos.z - 1; break;
                    case 2: target.x = currentPos.x + 1; target.z = currentPos.z - 1; break;
                    case 3: target.x = currentPos.x - 1; break;
                    case 4: target.x = currentPos.x + 1; break;
                    case 5: target.x = currentPos.x - 1; target.z = currentPos.z + 1; break;
                    case 6: target.z = currentPos.z + 1; break;
                    case 7: target.x = currentPos.x + 1; target.z = currentPos.z + 1; break;
                    default: break;
                }
            }

            

            // Move owner
            owner.Move(target,1f,100f);


            // Below is the first draft. I don't think we need it, but I did not dare remove it :)

            //bool foundFood = false;

            //while (!foundFood)
            //{
            //This will be changed to work with the field of view method
            //var hitObjects = Physics.OverlapSphere(this.owner.transform.position, 30f);
            //for (int i = 0; i < hitObjects.Length; i++)
            //{
            //    if (hitObjects[i].CompareTag(owner.FoodSource))
            //    {
            //move to hitObjects[i].transform.postion and eat it
            //goes to the first obejct in the list and does not take distance into consideration
            //        foundFood = true;
            //    }
            //    break;
            //}

            //walk at random or by memory to find food
            //}

        }

        public void Exit() {
            owner.Sensors.LookForFood(false);
        }
    }
}