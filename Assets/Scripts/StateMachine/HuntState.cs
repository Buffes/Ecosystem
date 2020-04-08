using UnityEngine;
using Unity.Entities;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines
{
    public class HuntState : IState
    {

        Animal owner;
        private float timeSinceLastFrame = 0f;
        private float pathfindInterval = 0.1f;

        public HuntState(Animal owner) { this.owner = owner; }

        public void Enter()
        {
            // Starts sprint
            owner.GetMovement().Sprint(true);
        }

        public void Execute()
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;

            Vector3 preyPos = owner.GetSensors().GetFoundPreyInfo().Position;
            Vector3 currentPos = owner.GetMovement().GetPosition();
            Vector3 diff = currentPos - preyPos;
            float diffLength = Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2));

            if (diffLength <= 1f)
            {
                Entity prey = owner.GetSensors().GetFoundPreyInfo().Entity;
                owner.GetInteraction().Kill(prey);
                //owner.GetNeedsStatus().SateHunger(owner.GetInteraction().Eat(food));
            }
            owner.Move(preyPos, 0f, 200);
        }

        public void Exit()
        {
            // End sprint
            owner.GetMovement().Sprint(false);
            owner.GetSensors().LookForPrey(false);
        }
    }
}