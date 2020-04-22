using UnityEngine;
using Ecosystem.Attributes;

namespace Ecosystem.StateMachines {
    public class FollowParentState : IState {

        Animal owner;
        Vector3 nextTarget;
        private float timeSinceLastFrame;
        private readonly float pathfindInterval = 3f;

        public FollowParentState(Animal owner) 
        { 
            this.owner = owner; 
        }

        public void Enter() 
        {
            owner.GetSensors().LookForParent(true);
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

            if (diffLength > 3f)
            {
                // Move closer to parent
                owner.Move(nextTarget,0f,200);
                return;
            }

            if (owner.GetNeedsStatus().GetHungerStatus() < owner.HungerLimit)
            {
                owner.GetNeedsStatus().TransferHunger(1f);
            }

        }


        public void Exit() 
        {
            owner.GetSensors().LookForParent(false);
        }
    }
}