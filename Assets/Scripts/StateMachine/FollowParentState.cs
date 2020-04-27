using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FollowParentState : IState {

        Animal owner;
        private float timeSinceLastFrame;
        private readonly float pathfindInterval = 1f;

        public FollowParentState(Animal owner) 
        { 
            this.owner = owner; 
        }

        public void Enter() 
        {
            owner.GetMovement().Fly(true);
        }

        public void Execute() 
        {
            timeSinceLastFrame += Time.deltaTime;
            if (timeSinceLastFrame < pathfindInterval) return;
            timeSinceLastFrame = 0f;
            
            if (!owner.GetSensors().FoundParent()) return;
            
            Vector3 currentPos = owner.GetMovement().GetPosition();
            Vector3 parentPos = owner.GetSensors().GetFoundParentInfo().Position;
            float diffLength = Vector3.Distance(parentPos, currentPos);

            if (diffLength > 2f)
            {
                // Move closer to parent
                owner.Move(parentPos,1f,200);
                return;
            }

            if (owner.GetNeedsStatus().GetHungerStatus() < owner.HungerLimit)
            {
                owner.GetNeedsStatus().TransferHunger(1f);
            }

            if (owner.GetNeedsStatus().GetThirstStatus() < owner.ThirstLimit)
            {
                owner.GetNeedsStatus().TransferThirst(1f);
            }

        }

        public void Exit() 
        {
        }
    }
}